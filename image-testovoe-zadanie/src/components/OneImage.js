
import React , {useEffect} from "react"
import { useLocation , useNavigate } from "react-router-dom";


export default function OneImage() {
    const location = useLocation();

    const navigate = useNavigate();

    const ButtonHome = (i) => {
        navigate('/', {replace: true})
    }  
    useEffect(() => {
        console.log(location)
    }, []);
    
    return (
        <div className="row">
        <div className="col-md-8    ">
            <div className="jumbotron jumbotron-fluid py-4">
                <div className="container text-center">
                    <h1 className="display-4"> <span> {location.state.data.description}</span> </h1>
                    
                    <img src={location.state.data.imageSrc} alt="Source" crossorigin className="card-img-top" />  
                    
                    <br/>   
                    <button type="submit" className="btn btn-light"  onClick={()=>{ButtonHome()}}>
                     Вернуться на главную страницу
                </button>
                </div>
            </div>
        </div>
        </div>
    );
    }