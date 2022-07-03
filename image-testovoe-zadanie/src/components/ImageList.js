import React, {useState, useEffect} from "react";
import Image from './Image';
import { useNavigate } from "react-router-dom";
import axios from "axios";


export default function ImageList(){
    const [imageList, setImageList] = useState([])
    const [recordForEdit, setRecordForEdit] = useState(null)
    useEffect(() => {
        refreshImageList();
    }, [])
    
    const imageAPI = (url = 'https://localhost:44334/api/Image/') => {
    return {
        fetchAll: () => axios.get(url),
        create: newRecord => axios.post(url, newRecord),
        update: (id, updatedRecord) => axios.put(url + id, updatedRecord),
        delete: id => axios.delete(url+id)
     }  
    }
    function refreshImageList() {
        imageAPI().fetchAll() 
        .then(res=> setImageList(res.data))
        .catch(err => console.log(err))
    }

    const addOrEdit = (formData, onSuccess) => {
        if(formData.get('imageID') =="0")
            imageAPI().create(formData)
            .then(res => {
            onSuccess();
            refreshImageList();
          })
            .catch(err => console.log(err))
        else
            imageAPI().update(formData.get('imageID'),formData)
             .then(res => {
                onSuccess();
                refreshImageList();
            })
        .catch(err => console.log(err))
    }
    const showRecordDetailts = data =>{
        setRecordForEdit(data)
    }
    const onDelete = (e,id) => {
        e.stopPropagation();
        if(window.confirm('Вы хотите удалить это изображение?'))
        imageAPI().delete(id)
        .then(res =>  refreshImageList())
        .catch(err => console.log(err))
    }


    const navigate = useNavigate();

    const showViewPhoto = (i) => {
        navigate('/oneimage', {state:{id:i, data: imageList[i]}})
    }   


       return(
        <div className="row">
            <div className="col-md-12">
                <div className="jumbotron jumbotron-fluid py-4">
                    <div className="container text-center">
                        <h1 className="display-4"> Галерея</h1>
                    </div>
                </div>
            </div>
            <div className="col-md-4">
            <Image
            addOrEdit = {addOrEdit}
            recordForEdit={recordForEdit}
             />
            </div>
           <div className="col-md-4">
            <table>
            <tbody>
            
                {     
                imageList.map((data,i) =>
                <tr key={i}>
                    <td>  <div className="card">
            <img src={data.imageSrc} className="card-img-top" />
            <div className="card-body">
                <span> {data.description}</span> 
                <br/>
                <button type="submit" className="btn btn-light"  onClick={()=>{showViewPhoto(i)}}>
                     <i className="fa fa-search-plus"></i>
                </button>
                <button type="submit" className="btn btn-light" onClick={()=>{showRecordDetailts(data)}}>
                     <i className="fa fa-pencil"></i>
                </button>
                <button className="btn btn-light" onClick={e => onDelete(e,parseInt(data.imageID))}>
                    <i className="far fa-trash-alt"></i>
                </button>
            </div>

        </div></td>
                </tr>
                )
                }
            </tbody>
            </table>
            
            </div>
        </div>
    )
}

