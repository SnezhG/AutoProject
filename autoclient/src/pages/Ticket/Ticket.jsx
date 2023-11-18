import {Link, useNavigate, useParams} from "react-router-dom";
import {useEffect, useState} from "react";
import axios from "axios";
import {Container} from "react-bootstrap";

function Ticket(){
    const {id} = useParams();

    const [values, setValues] = useState([])

    useEffect(() => {
        axios.get(`https://localhost:7089/api/Tickets/${id}`)
            .then(res => 
                setValues(res.data))
            .catch(err => console.log(err))
    }, []);

    return (
        <div className="container mt-5" style={{width: '40%'}}>
            <div className="card p-3 rounded">
                <h1 className="text-center mb-4">Ticket Information</h1>
                <div className="mb-3">
                    <label className="form-label">Пассажир</label>
                    <p>{`${values.name} ${values.surname} ${values.patronymic}`}</p>
                </div>
                <div className="mb-3">
                    <label className="form-label">Маршрут</label>
                    <p>{`${values.depCity} - ${values.arrCity}`}</p>
                </div>
                <div className="mb-3">
                    <label className="form-label">Время отправки</label>
                    <p>{`${values.depDate} ${values.depTime}`}</p>
                </div>

                {/* Arrival Time Information */}
                <div className="mb-3">
                    <label className="form-label">Время прибытия</label>
                    <p>{`${values.arrDate} ${values.arrTime}`}</p>
                </div>
                <div className="mb-3">
                    <label className="form-label">Место</label>
                    <p>{values.seat}</p>
                </div>
                <div className="mb-3">
                    <label className="form-label">Цена</label>
                    <p>{values.price}</p>
                </div>
                <div className="mb-3">
                    <label className="form-label">Статус</label>
                    <p>{values.status}</p>
                </div>
            </div>
        </div>
    )
}

export default Ticket