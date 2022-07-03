using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TestovoeImage.Models;
using Microsoft.AspNetCore.Hosting;

namespace TestovoeImage.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImageController : ControllerBase
    {
        private readonly ImageDbContext _context;
        private readonly IWebHostEnvironment _hostEnviroment;

        public ImageController(ImageDbContext context, IWebHostEnvironment hostEnviroment)
        {
            _context = context;
            this._hostEnviroment = hostEnviroment;
        }

        // GET: api/Image
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ImageModel>>> GetImage()
        {
            return await _context.Image
                .Select(x => new ImageModel()
                {
                    ImageID = x.ImageID,
                    Description = x.Description,
                    ImageName = x.ImageName,
                    ImageSrc = String.Format("{0}://{1}{2}/Images/{3}", Request.Scheme, Request.Host,Request.PathBase,x.ImageName)
                })
                .ToListAsync();
        }

        // GET: api/Image/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ImageModel>> GetImageModel(int id)
        {
            var imageModel = await _context.Image.FindAsync(id);

            if (imageModel == null)
            {
                return NotFound();
            }

            return imageModel;
        }

        // PUT: api/Image/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutImageModel(int id, [FromForm]ImageModel imageModel)
        {
            if (id != imageModel.ImageID)
            {
                return BadRequest();
            }
            if (imageModel.ImageFile != null )
            {
                DeleteImage(imageModel.ImageName);
                imageModel.ImageName = await SaveImage(imageModel.ImageFile);
            }
            _context.Entry(imageModel).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ImageModelExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Image
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<ImageModel>> PostImageModel([FromForm]ImageModel imageModel)
        {
            imageModel.ImageName = await SaveImage(imageModel.ImageFile); 
            _context.Image.Add(imageModel);
            await _context.SaveChangesAsync();

            return StatusCode(201);
        }

        // DELETE: api/Image/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteImageModel(int id)
        {
            var imageModel = await _context.Image.FindAsync(id);
            if (imageModel == null)
            {
                return NotFound();
            }
            DeleteImage(imageModel.ImageName);
            _context.Image.Remove(imageModel);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ImageModelExists(int id)
        {
            return _context.Image.Any(e => e.ImageID == id);
        }

        [NonAction]
        public async Task< string> SaveImage(IFormFile imageFile)//сохранение фотографии в папку и в бд
        {
            string imageName = new String(Path.GetFileNameWithoutExtension(imageFile.FileName).Take(10).ToArray()).Replace(' ', '-');
            imageName = imageName + DateTime.Now.ToString("yymmssfff") + Path.GetExtension(imageFile.FileName);
            var imagePath = Path.Combine(_hostEnviroment.ContentRootPath, "Images", imageName);
            using (var fileStream = new FileStream(imagePath, FileMode.Create))
            {
                await imageFile.CopyToAsync(fileStream);
            }
        return imageName;
        }
        [NonAction]
        public void DeleteImage(string imageName)
        {
            var imagePath = Path.Combine(_hostEnviroment.ContentRootPath, "Images", imageName);
            if(System.IO.File.Exists(imagePath))
                System.IO.File.Delete(imagePath);
        }
    }
}
