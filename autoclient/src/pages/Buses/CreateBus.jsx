import {Link, useNavigate} from "react-router-dom";
import {useState} from "react";
import axios from "axios";

function CreateBus(){
    const [values, setValues] = useState({
        seatCapacity: '',
        model: '',
        specs: ''
    })
    
    const navigate = useNavigate();
    const handleSubmit = (event) =>{
        event.preventDefault();
        axios.post('https://localhost:7089/api/Buses', values)
            .then(res => {
                console.log(res);
                navigate('/Buses')
            })
            .catch(err => console.log(err));
    }
    
    return (
        <div>
            <h1>Create new bus</h1>
            <form onSubmit={handleSubmit} className="formCreate">
                <div>
                    <label htmlFor="seatCapacity">Seat capacity</label>
                    <input type="text" name='seatCapacity' 
                           placeholder="Enter seat capacity"
                           onChange={e => 
                               setValues({...values, seatCapacity: e.target.value})}/>
                </div>
                <div>
                    <label htmlFor="model">Seat capacity</label>
                    <input type="text" name='model' placeholder="Enter model" 
                           onChange={e => setValues({...values, model: e.target.value})}/>
                </div>
                <div>
                    <label htmlFor="specs">Seat capacity</label>
                    <input type="text" name='specs' placeholder="Enter specs"
                           onChange={e => setValues({...values, specs: e.target.value})}/>
                </div>
                <button>Submit</button>
                <Link to="/Buses">Back</Link>
            </form>
        </div>
    )
}

export default CreateBus