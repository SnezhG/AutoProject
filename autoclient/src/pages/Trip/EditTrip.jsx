import {Link, useNavigate, useParams} from "react-router-dom";
import {useEffect, useState} from "react";
import axios from "axios";
import {Container} from "react-bootstrap";

function EditTrip(){
    const {id} = useParams();
    
    const [tripValues, setTripValues] = useState({
        routeId: '',
        busId: '',
        driverId: '',
        conductorId:'',
        price:'',
        depTime:'',
        arrTime:''
    })

    const [defaultBus, setDefaultBus] = useState('')
    const [defaultDriver, setDefaultDriver] = useState('')
    const [defaultCond, setDefaultCond] = useState('')
    const [routeValues, setRouteValues] = useState([])
    const [busValues, setBusValues] = useState([])
    const [driverValues, setDriverValues] = useState([])
    const [condValues, setCondValues] = useState([])
    const getData = () => {
        axios.get(`https://localhost:5275/api/Trips/${id}`)
            .then(res =>{
                setTripValues(res.data)
                setDefaultBus(res.data.busId)
                setDefaultDriver(res.data.driverId)
                setDefaultCond(res.data.conductorId)
            })
            .catch(err => console.log(err))
        axios.get("https://localhost:5275/api/Busroutes")
            .then((response) => {
                setRouteValues(response.data.map(route => ({
                    label: `${route.depCity} - ${route.arrCity}`,
                    value: route.routeId
                })));
            });
        axios.get("https://localhost:5275/api/Buses")
            .then((response) => {
                setBusValues(response.data
                    .map(bus =>({
                        label:bus.model,
                        value:bus.busId,
                        status: bus.available
                    })));
            });
        axios.get("https://localhost:5275/api/Personnels")
            .then((response) => {
                setDriverValues(response.data
                    .filter(person => person.post == 'driver')
                    .map(driver =>({
                        label: `${driver.name} ${driver.surname}`,
                        value: driver.personnelId,
                        status: driver.available
                    })));
                setCondValues(response.data
                    .filter(person => person.post == 'conductor')
                    .map(cond =>({
                        label: `${cond.name} ${cond.surname}`,
                        value: cond.personnelId,
                        status: cond.available
                    })));
            });
    };

    useEffect(() => {
        getData();
    }, []);

    
    const navigate = useNavigate();
    const handleSubmit = (event) =>{
        event.preventDefault();
        axios.put('https://localhost:5275/api/Trips', tripValues, {
            headers:{
                Authorization: `Bearer ${localStorage.getItem('token')}`
            }
        })
            .then(res => {
                console.log(res);
                navigate('/')
            })
            .catch(err => console.log(err));
    }

    return (
        <Container className="mt-5" style={{ width: '40%' }}>
            <h1 className="text-center">Редактировать данные о рейсе</h1>
            <form onSubmit={handleSubmit} className="border p-4 rounded" style={{backgroundColor:'white'}}>
                <div className="mb-3">
                    <label htmlFor="routeId" className="form-label">Маршрут</label>
                    <select
                        className="form-select"
                        onChange={(e) =>
                            setTripValues({ ...tripValues, routeId: e.target.value })}
                        value={tripValues.routeId}
                    >
                        <option value={tripValues.routeId} disabled hidden>
                            {tripValues.routeId ? routeValues.find((option) =>
                                option.value == tripValues.routeId)?.label : 'Select route'}
                        </option>
                        {routeValues.map((option) => (
                            <option key={option.value} value={option.value}>
                                {option.label}
                            </option>
                        ))}
                    </select>
                </div>
                <div className="mb-3">
                    <label htmlFor="busId" className="form-label">Автобус</label>
                    <select
                        className="form-select"
                        onChange={(e) =>
                            setTripValues({ ...tripValues, busId: e.target.value })}
                        value={tripValues.busId}
                    >
                        <option value={tripValues.busId} disabled hidden>
                            {tripValues.busId ? busValues.find((option) =>
                                option.value == tripValues.busId)?.label : 'Select bus'}
                        </option>
                        {busValues
                            .filter(option => option.status || option.value === defaultBus)
                            .map((option) => (
                                <option key={option.value} value={option.value}>
                                    {option.label}
                                </option>
                            ))}
                    </select>
                </div>
                <div className="mb-3">
                    <label htmlFor="driverId" className="form-label">Водитель</label>
                    <select
                        className="form-select"
                        onChange={(e) =>
                            setTripValues({ ...tripValues, driverId: e.target.value })}
                        value={tripValues.driverId}
                    >
                        <option value={tripValues.driverId} disabled hidden>
                            {tripValues.driverId ? driverValues.find((option) =>
                                option.value == tripValues.driverId)?.label : 'Select driver'}
                        </option>
                        {driverValues
                            .filter(option => option.status || option.value === defaultDriver)
                            .map((option) => (
                                <option key={option.value} value={option.value}>
                                    {option.label}
                                </option>
                            ))}
                    </select>
                </div>
                <div className="mb-3">
                    <label htmlFor="conductorId" className="form-label">Кондуктор</label>
                    <select
                        onChange={(e) =>
                            setTripValues({ ...tripValues, conductorId: e.target.value })}
                        value={tripValues.conductorId}
                        className="form-select"
                    >
                        <option value={tripValues.conductorId} disabled hidden>
                            {tripValues.conductorId ? condValues.find((option) =>
                                option.value == tripValues.conductorId)?.label : 'Select conductor'}
                        </option>
                        {condValues
                            .filter(option => option.status || option.value === defaultCond)
                            .map((option) => (
                                <option key={option.value} value={option.value}>
                                    {option.label}
                                </option>
                            ))}
                    </select>
                </div>
                <div className="mb-3">
                    <label htmlFor="price" className="form-label">Цена</label>
                    <input
                        type="text"
                        name='price'
                        className="form-control"
                        required
                        value={tripValues.price}
                        onChange={e =>
                            setTripValues({...tripValues, price: e.target.value})}
                    />
                </div>
                <div className="mb-3">
                    <label htmlFor="depTime" className="form-label">Дата и время отправки</label>
                    <input
                        type="datetime-local"
                        name='depTime'
                        className="form-control"
                        required
                        value={tripValues.depTime}
                        onChange={e =>
                            setTripValues({...tripValues, depTime: e.target.value})}
                    />
                </div>
                <div className="mb-3">
                    <label htmlFor="arrTime" className="form-label">Дата и время прибытия</label>
                    <input
                        type="datetime-local"
                        name='arrTime'
                        className="form-control"
                        required
                        value={tripValues.arrTime}
                        onChange={e =>
                            setTripValues({...tripValues, arrTime: e.target.value})}
                    />
                </div>
                <button type="submit" className="btn btn-primary" style={{backgroundColor:'#7e5539', borderColor:'#7e5539'}}>Изменить</button>
                <Link to="/" className="btn btn-secondary ms-2">Назад</Link>
            </form>
        </Container>
    )
}

export default EditTrip