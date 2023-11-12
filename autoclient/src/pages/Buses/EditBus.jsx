import {Link, useNavigate, useParams} from "react-router-dom";
import {useEffect, useState} from "react";
import axios from "axios";

function EditBus(){
/*    const [data, setData] = useState([])*/
    const {id} = useParams();
    
    const [values, setValues] = useState({
        seatCapacity: '',
        model: '',
        specs: ''
    })
    
    useEffect(() => {
        axios.get(`https://localhost:7089/api/Buses/${id}`)
            .then(res => setValues(res.data))
            .catch(err => console.log(err))
    }, []);

    const navigate = useNavigate();
    const handleUpdate = (event) =>{
        event.preventDefault();
        axios.put(`https://localhost:7089/api/Buses/${id}`, values)
            .then(res => {
                console.log(res);
                navigate('/Buses');
            })
            .catch(err => console.log(err))
    }
    
    return (
        <div>
            <h1>Edit bus</h1>
            <form onSubmit={handleUpdate}>
                <div>
                    <label htmlFor="seatCapacity">Seat capacity</label>
                    <input type="text" name='seatCapacity' placeholder="Enter seat capacity"
                           value={values.seatCapacity} onChange={e => setValues({...values, seatCapacity: e.target.value})}/>
                </div>
                <div>
                    <label htmlFor="model">Seat capacity</label>
                    <input type="text" name='model' placeholder="Enter model"
                           value={values.model} onChange={e => setValues({...values, model: e.target.value})}/>
                </div>
                <div>
                    <label htmlFor="specs">Seat capacity</label>
                    <input type="text" name='specs' placeholder="Enter specs"
                           value={values.specs} onChange={e => setValues({...values, specs: e.target.value})}/>
                </div>
                <button>Edit</button>
                <Link to="/Buses">Back</Link>
            </form>
        </div>
    )
}

export default EditBus
