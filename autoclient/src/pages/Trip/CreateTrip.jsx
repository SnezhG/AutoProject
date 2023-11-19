import {useEffect, useState} from "react";
import {Link, useNavigate} from "react-router-dom";
import axios from "axios";
import {Container} from "react-bootstrap";

function CreateTrip(){
    const [tripValues, setTripValues] = useState({
        routeId: '',
        busId: '',
        driverId: '',
        condId:'',
        price:'',
        depTime:'',
        arrTime:''
    })

    const [routeValues, setRouteValues] = useState([])
    const [busValues, setBusValues] = useState([])
    const [driverValues, setDriverValues] = useState([])
    const [condValues, setCondValues] = useState([])
    const getData = () => {
        axios.get("https://localhost:7089/api/Busroutes")
            .then((response) => {
            setRouteValues(response.data.map(route => ({
                label: `${route.depCity} - ${route.arrCity}`,
                value: route.routeId
            })));
        });
        axios.get("https://localhost:7089/api/Buses")
            .then((response) => {
                setBusValues(response.data
                    .filter(bus => bus.available)
                    .map(bus =>({
                    label:bus.model,
                    value:bus.busId
                })));
            });
        axios.get("https://localhost:7089/api/Personnels")
            .then((response) => {
                setDriverValues(response.data
                    .filter(person => person.post == 'Водитель' && person.available)
                    .map(driver =>({
                        label: `${driver.name} ${driver.surname}`,
                        value: driver.personnelId
                    })));
                setCondValues(response.data
                    .filter(person => person.post == 'Кондуктор' && person.available)
                    .map(cond =>({
                        label: `${cond.name} ${cond.surname}`,
                        value: cond.personnelId
                    })));
            });
    };

    useEffect(() => {
        getData();
    }, []);

    const navigate = useNavigate();
    const handleSubmit = (event) =>{
        event.preventDefault();
        axios.post('https://localhost:7089/api/Trips', tripValues)
            .then(res => {
                console.log(res);
                navigate('/')
            })
            .catch(err => console.log(err));
    }

    return (
        <Container className="mt-5" style={{width: '40%'}}>
            <h2 className="text-center">Добавить новый рейс</h2>
            <form onSubmit={handleSubmit} className="border p-4 rounded">
                <div className="mb-3">
                    <label htmlFor="routeId" className="form-label">Маршрут</label>
                    <select
                        className="form-select"
                        onChange={(e) =>
                            setTripValues({ ...tripValues, routeId: e.target.value })}
                        value={tripValues.routeId}
                    >
                        <option value="">Маршрут</option>
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
                        <option value="">Автобус</option>
                        {busValues.map((option) => (
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
                        <option value="">Водитель</option>
                        {driverValues.map((option) => (
                            <option key={option.value} value={option.value}>
                                {option.label}
                            </option>
                        ))}
                    </select>
                </div>
                <div className="mb-3">
                    <label htmlFor="driverId" className="form-label">Кондуктор</label>
                    <select
                        className="form-select"
                        onChange={(e) =>
                            setTripValues({ ...tripValues, condId: e.target.value })}
                        value={tripValues.condId}
                    >
                        <option value="">Кондуктор</option>
                        {condValues.map((option) => (
                            <option key={option.value} value={option.value}>
                                {option.label}
                            </option>
                        ))}
                    </select>
                </div>
                <div className="mb-3">
                    <label htmlFor="driverId" className="form-label">Цена</label>
                    <input 
                        type="text" 
                        name='price'
                        className="form-control"
                        required
                        placeholder="Цена"
                           onChange={e =>
                               setTripValues({...tripValues, price: e.target.value})}/>
                </div>
                <div className="mb-3">
                    <label htmlFor="driverId" className="form-label">Время отправки</label>
                    <input 
                        type="datetime-local" 
                        name='depTime'
                        className="form-control"
                        required
                        onChange={e =>
                               setTripValues({...tripValues, depTime: e.target.value})}/>
                </div>
                <div className="mb-3">
                    <label htmlFor="driverId" className="form-label">Время прибытия</label>
                    <input 
                        type="datetime-local" 
                        name='arrTime'
                        className="form-control"
                        required
                        onChange={e =>
                               setTripValues({...tripValues, arrTime: e.target.value})}/>
                </div>
                <button type="submit" className="btn btn-primary">Добавить</button>
                <Link to="/" className="btn btn-secondary ms-2">Назад</Link>
            </form>
        </Container>
    )
}

export default CreateTrip