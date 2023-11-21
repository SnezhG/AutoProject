import {Link, useNavigate, useParams} from "react-router-dom";
import {useEffect, useState} from "react";
import axios from "axios";
import {Container} from "react-bootstrap";

function EditBus(){
    const {id} = useParams();
    
    const [values, setValues] = useState({
        seatCapacity: '',
        model: '',
        specs: ''
    })
    
    useEffect(() => {
        axios.get(`https://localhost:5275/api/Buses/${id}`)
            .then(res => setValues(res.data))
            .catch(err => console.log(err))
    }, []);

    const navigate = useNavigate();
    const handleUpdate = (event) =>{
        event.preventDefault();
        axios.put(`https://localhost:5275/api/Buses/${id}`, values, {
            headers:{
                Authorization: `Bearer ${localStorage.getItem('token')}`
            }
        })
            .then(res => {
                console.log(res);
                navigate('/Buses');
            })
            .catch(err => console.log(err))
    }
    
    return (
        <Container className="mt-5" style={{ width: '40%' }}>
            <h1 className="text-center">Редактировать данные об автобусе</h1>
            <form onSubmit={handleUpdate} className="border p-4 rounded" style={{backgroundColor:'white'}}>
                <div className="mb-3">
                    <label htmlFor="model" className="form-label">Модель</label>
                    <input
                        type="text"
                        name='model'
                        className="form-control"
                        placeholder="Введите модель"
                        required
                        value={values.model}
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
                        value={values.seatCapacity}
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
                        value={values.specs}
                        onChange={(e) => 
                            setValues({ ...values, specs: e.target.value })}
                    />
                </div>
                <button type="submit" className="btn btn-primary" style={{backgroundColor:'#7e5539', borderColor:'#7e5539'}}>Изменить</button>
                <Link to="/Buses" className="btn btn-secondary ms-2">Назад</Link>
            </form>
        </Container>
    )
}

export default EditBus
