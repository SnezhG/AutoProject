import {Link, useNavigate} from "react-router-dom";
import {useState} from "react";
import axios from "axios";
import {Container} from "react-bootstrap";

function CreateBus(){
    const [values, setValues] = useState({
        seatCapacity: '',
        model: '',
        specs: ''
    })
    
    const navigate = useNavigate();
    const handleSubmit = (event) =>{
        event.preventDefault();
        axios.post('https://localhost:5275/api/Buses', values, {
            headers:{
                Authorization: `Bearer ${localStorage.getItem('token')}`
            }
        })
            .then(res => {
                console.log(res);
                navigate('/Buses')
            })
            .catch(err => console.log(err));
    }
    
    return (
        <Container className="mt-5" style={{width: '40%'}}>
            <h2 className="text-center">Добавить новый автобус</h2>
            <form onSubmit={handleSubmit} className="border p-4 rounded" style={{backgroundColor:'white'}}>
                <div className="mb-3">
                    <label htmlFor="model" className="form-label">Модель</label>
                    <input
                        type="text"
                        name='model'
                        className="form-control"
                        placeholder="Введите модель"
                        required
                        onChange={(e) => 
                            setValues({ ...values, model: e.target.value })}
                    />
                </div>
                <div className="mb-3">
                    <label htmlFor="seatCapacity" className="form-label">Кол-во мест</label>
                    <input
                        type="text"
                        name='seatCapacity'
                        className="form-control"
                        placeholder="Введите кол-во мест"
                        required
                        onChange={(e) =>
                            setValues({ ...values, seatCapacity: e.target.value })}
                    />
                </div>
                <div className="mb-3">
                    <label htmlFor="specs" className="form-label">Характеристики</label>
                    <input
                        type="text"
                        name='specs'
                        className="form-control"
                        placeholder="Введите характеристик"
                        required
                        onChange={(e) => 
                            setValues({ ...values, specs: e.target.value })}
                    />
                </div>
                <button type="submit" className="btn btn-primary" style={{backgroundColor:'#7e5539', borderColor: '#7e5539'}}>Добавить</button>
                <Link to="/Buses" className="btn btn-secondary ms-2">Назад</Link>
            </form>
        </Container>
    )
}

export default CreateBus