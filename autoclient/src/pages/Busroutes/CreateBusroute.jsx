import {useState} from "react";
import {Link, useNavigate} from "react-router-dom";
import axios from "axios";
import {Container} from "react-bootstrap";

function CreateBusroute(){
    const [values, setValues] = useState({
        depCity: '',
        arrCity: ''
    })

    const navigate = useNavigate();
    const handleSubmit = (event) =>{
        event.preventDefault();
        axios.post('https://localhost:7089/api/Busroutes', values)
            .then(res => {
                console.log(res);
                navigate('/Busroutes')
            })
            .catch(err => console.log(err));
    }

    return (
        <Container className="mt-5" style={{width: '40%'}}>
            <h2 className="text-center">Добавить новый маршрут</h2>
            <form onSubmit={handleSubmit} className="border p-4 rounded">
                <div className="mb-3">
                    <label htmlFor="depCity" className="form-label">Город отправки</label>
                    <input
                        type="text"
                        name='depCity'
                        className="form-control"
                        placeholder="Введите город"
                        required
                        onChange={(e) =>
                            setValues({ ...values, depCity: e.target.value })}
                    />
                </div>
                <div className="mb-3">
                    <label htmlFor="arrCity" className="form-label">Город прибытия</label>
                    <input
                        type="text"
                        name='arrCity'
                        className="form-control"
                        placeholder="Введите город"
                        required
                        onChange={(e) =>
                            setValues({ ...values, arrCity: e.target.value })}
                    />
                </div>
                <button type="submit" className="btn btn-primary">Добавить</button>
                <Link to="/Busroutes" className="btn btn-secondary ms-2">Назад</Link>
            </form>
        </Container>
    )
}

export default CreateBusroute