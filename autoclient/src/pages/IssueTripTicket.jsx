import {Route, useNavigate, useParams} from "react-router-dom";
import {useEffect, useState} from "react";
import axios from "axios";

function IssueTripTicket(){
    const {id} = useParams();

    const [tripValues, setTripValues] = useState( [])
    const [routeValues, setRouteValues] = useState( [])
    const [seatsValues, setSeatsValues] = useState( [])

    const [ticketValues, setTicketValues] = useState({
        lastName: '',
        name: '',
        patronymic: '',
        sex: '',
        dateOfBirth: '',
        passSeries: '',
        passNum: '',
        phoneNum: '',
        seat: '',
        trip: ''
    })
    
    useEffect(() => {
        axios.get(`https://localhost:7089/api/Trips/${id}`)
            .then(res => {
                setTripValues(res.data)
                console.log(res.data)
                return res.data
            })
            .then((firstRes) =>{
                axios.get(`https://localhost:7089/api/Busroutes/${firstRes.routeId}`)
                    .then(res => {
                        setRouteValues(res.data)
                        console.log(res.data)
                    })
                    .catch(err => console.log(err))
                    axios.get(`https://localhost:7089/api/Buses/BusSeats/${firstRes.busId}`)
                    .then(res => {
                        setSeatsValues(res.data)
                        console.log(res.data)
                    })
                    .catch(err => console.log(err))
            })
            .catch(err => console.log(err))
       
    }, []);

    const navigate = useNavigate();
    const handleBooking = (event) =>{
        event.preventDefault()
        setTicketValues({...ticketValues, trip: tripValues.tripId})
        console.log(ticketValues)
        axios.post(`https://localhost:7089/api/Tickets/BookTicket`, ticketValues)
            .then(res => {
                console.log(res);
                navigate('/Account')
            })
            .catch(err => console.log(err))
    }

    const handleBuying = (event) =>{
        event.preventDefault()
        setTicketValues({...ticketValues, trip: tripValues.tripId})
        axios.post(`https://localhost:7089/api/Tickets/BuyTicket`, ticketValues)
            .then(res => {
                console.log(res);
                return res.data
            })
            .then((firstRes) =>{
                navigate(`/TicketPay/${firstRes}`)
            })
            .catch(err => console.log(err))
    }
    
    return(
        <div>
            <div className="tripInfo">
                <form>
                    <div>
                        <input type="text" readOnly name='depCity' value={routeValues.depCity}/>
                        <input type="text" readOnly name='arrCity' value={routeValues.arrCity}/>
                        <input type="text" readOnly name='depDate' value={tripValues.depTime}/>
                    </div>
                </form>
            </div>
            <div className="ticketForm">
                <form onSubmit={handleBooking}>
                    <div className="ticketField">
                        <label htmlFor="lastName">Фамилия</label>
                        <input type="text" name='lastName' 
                               value={ticketValues.lastName}
                               onChange={e =>
                                   setTicketValues({...ticketValues, lastName: e.target.value})}/>
                    </div>
                    <div className="ticketField">
                        <label htmlFor="name">Имя</label>
                        <input type="text" name='name' 
                               value={ticketValues.name}
                               onChange={e =>
                                   setTicketValues({...ticketValues, name: e.target.value})}/>
                    </div>
                    <div className="ticketField">
                        <label htmlFor="patronymic">Отчество</label>
                        <input type="text" name='patronymic'
                               value={ticketValues.patronymic}
                               onChange={e =>
                                   setTicketValues({...ticketValues, patronymic: e.target.value})}/>
                    </div>
                    <div className="ticketField">
                        <label htmlFor="sex">Пол</label>
                        <input type="text" name='sex' 
                               value={ticketValues.sex}
                               onChange={e =>
                                   setTicketValues({...ticketValues, sex: e.target.value})}/>
                    </div>
                    <div className="ticketField">
                        <label htmlFor="dateOfBirth">Дата рождения</label>
                        <input type="text" name='dateOfBirth' 
                               value={ticketValues.dateOfBirth}
                               onChange={e =>
                                   setTicketValues({...ticketValues, dateOfBirth: e.target.value})}/>
                    </div>
                    <div className="ticketField">
                        <label htmlFor="passSeries">Серия паспорта</label>
                        <input type="text" name='passSeries' 
                               value={ticketValues.passSeries}
                               onChange={e =>
                                   setTicketValues({...ticketValues, passSeries: e.target.value})}/>
                    </div>
                    <div className="ticketField">
                        <label htmlFor="passNum">Номер паспорта</label>
                        <input type="text" name='passNum'
                               value={ticketValues.passNum}
                               onChange={e =>
                                   setTicketValues({...ticketValues, passNum: e.target.value})}/>
                    </div>
                    <div className="ticketField">
                        <label htmlFor="phoneNum">Номер телефона</label>
                        <input type="text" name='phoneNum' 
                               value={ticketValues.phoneNum}
                               onChange={e =>
                                   setTicketValues({...ticketValues, phoneNum: e.target.value})}/>
                    </div>
                    <div className="ticketField">
                        <label htmlFor="seat">Место</label>
                        <select type="text" name='seat' 
                                value={ticketValues.seat}
                                onChange={e =>
                                    setTicketValues({...ticketValues, seat: e.target.value})}>
                            <option value="" disabled>Select a seat</option>
                            {seatsValues
                                .filter(seat => seat.available)
                                .map(seat => (
                                    <option key={seat.seatId} value={seat.num}>
                                        {seat.num}
                                    </option>
                                ))}
                        </select>
                    </div>
                    <button type="submit">Купить</button>
                    <button onClick={handleBuying}>Забронировать</button>
                </form>
            </div>
        </div>
    )
}

export default IssueTripTicket