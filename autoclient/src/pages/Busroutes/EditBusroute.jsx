import {Link, useNavigate, useParams} from "react-router-dom";
import {useEffect, useState} from "react";
import axios from "axios";
import {Container} from "react-bootstrap";

function EditBusroute(){
    const {id} = useParams();

    const [values, setValues] = useState({
        depCity: '',
        arrCity: ''
    })

    useEffect(() => {
        axios.get(`https://localhost:7089/api/Busroutes/${id}`)
            .then(res => 
                setValues(res.data))
            .catch(err => console.log(err))
    }, []);

    const navigate = useNavigate();
    const handleUpdate = (event) =>{
        event.preventDefault();
        axios.put(`https://localhost:7089/api/Busroutes/${id}`, values)
            .then(res => {
                console.log(res);
                navigate('/Busroutes');
            })
            .catch(err => console.log(err))
    }

    return (
        <Container className="mt-5" style={{ width: '40%' }}>
            <h1 className="text-center">Редактировать данные о маршруте</h1>
            <form onSubmit={handleUpdate} className="border p-4 rounded">
                <div className="mb-3">
                    <label htmlFor="depCity" className="form-label">Город отправки</label>
                    <input
                        type="text"
                        name='depCity'
                        className="form-control"
                        placeholder="Введите город"
                        required
                        value={values.depCity}
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
                        value={values.arrCity}
                        onChange={(e) =>
                            setValues({ ...values, arrCity: e.target.value })}
                    />
                </div>
                <button type="submit" className="btn btn-primary">Изменить</button>
                <Link to="/Busroutes" className="btn btn-secondary ms-2">Назад</Link>
            </form>
        </Container>
    )
}

export default EditBusroute