import {Link, useNavigate, useParams} from "react-router-dom";
import {useEffect, useState} from "react";
import axios from "axios";

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
        axios.get(`https://localhost:7089/api/Trips/${id}`)
            .then(res =>{
                setTripValues(res.data)
                setDefaultBus(res.data.busId)
                setDefaultDriver(res.data.driverId)
                setDefaultCond(res.data.conductorId)
            })
            .catch(err => console.log(err))
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
                    .map(bus =>({
                        label:bus.model,
                        value:bus.busId,
                        status: bus.available
                    })));
            });
        axios.get("https://localhost:7089/api/Personnels")
            .then((response) => {
                setDriverValues(response.data
                    .filter(person => person.post == 'Водитель')
                    .map(driver =>({
                        label: `${driver.name} ${driver.surname}`,
                        value: driver.personnelId,
                        status: driver.available
                    })));
                setCondValues(response.data
                    .filter(person => person.post == 'Кондуктор')
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
        axios.put('https://localhost:7089/api/Trips', tripValues)
            .then(res => {
                console.log(res);
                navigate('/')
            })
            .catch(err => console.log(err));
    }

    return (
        <div>
            <h1>Edit trip</h1>
            <form onSubmit={handleSubmit} className="formCreate">
                <div>
                    <label htmlFor="routeId">Route</label>
                    <select
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
                <div>
                    <label htmlFor="busId">Bus</label>
                    <select
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
                <div>
                    <label htmlFor="driverId">Driver</label>
                    <select
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
                <div>
                    <label htmlFor="condId">Conductor</label>
                    <select
                        onChange={(e) =>
                            setTripValues({ ...tripValues, conductorId: e.target.value })}
                        value={tripValues.conductorId}
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
                <div>
                    <label htmlFor="price">Price</label>
                    <input type="text" name='price' 
                           value={tripValues.price}
                           onChange={e =>
                               setTripValues({...tripValues, price: e.target.value})}/>
                </div>
                <div>
                    <label htmlFor="depTime">Departure time</label>
                    <input type="datetime-local" name='depTime'
                           value={tripValues.depTime}
                           onChange={e =>
                               setTripValues({...tripValues, depTime: e.target.value})}/>
                </div>
                <div>
                    <label htmlFor="arrTime">Arrival time</label>
                    <input type="datetime-local" name='arrTime'
                           value={tripValues.arrTime}
                           onChange={e =>
                               setTripValues({...tripValues, arrTime: e.target.value})}/>
                </div>
                <button>Edit</button>
                <Link to="/">Back</Link>
            </form>
        </div>
    )
}

export default EditTrip