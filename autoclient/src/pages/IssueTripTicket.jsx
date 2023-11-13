import {Route, useNavigate, useParams} from "react-router-dom";
import {useEffect, useState} from "react";
import axios from "axios";

function IssueTripTicket(){
    const {id} = useParams();

    const [tripValues, setTripValues] = useState( [])
    const [routeValues, setRouteValues] = useState( [])
    const [seatsValues, setSeatsValues] = useState( [])

    const bus = tripValues.busId
    const route = tripValues.routeId

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
        trip: tripValues.tripId
    })
    
    useEffect(() => {
        axios.get(`https://localhost:7089/api/Trips/${id}`)
            .then(res => {
                setTripValues(res.data)
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
        event.preventDefault();
        axios.post(`https://localhost:7089/api/Tickets/BookTicket`, ticketValues)
            .then(res => {
                console.log(res);
                navigate('/Account')
            })
            .catch(err => console.log(err))
    }

    const handleBuying = (event) =>{
        event.preventDefault();
        axios.post(`https://localhost:7089/api/Tickets/BuyTicket`, ticketValues)
            .then(res => {
                console.log(res);
                navigate('/TicketPay')
            })
            .catch(err => console.log(err))
    }
    
    return(
        <div>
            <h1>Trip</h1>
            <form>
                <div>
                    <input type="text" name='depCity' value={routeValues.depCity}/>
                    <input type="text" name='arrCity' value={routeValues.arrCity}/>
                    <input type="text" name='depDate' value={tripValues.depTime}/>
                </div>
            </form>

            <h1>Issue Ticket</h1>
            <form>
                <div>
                    <label htmlFor="lastName">Name</label>
                    <input type="text" name='lastName' placeholder="Enter seat capacity"
                           value={ticketValues.lastName}
                           onChange={e =>
                               setTicketValues({...ticketValues, lastName: e.target.value})}/>
                </div>
                <div>
                    <label htmlFor="name">Last name</label>
                    <input type="text" name='name' placeholder="Enter seat capacity"
                           value={ticketValues.name}
                           onChange={e =>
                               setTicketValues({...ticketValues, name: e.target.value})}/>
                </div>
                <div>
                    <label htmlFor="patronymic">Patronimyc</label>
                    <input type="text" name='patronymic' placeholder="Enter seat capacity"
                           value={ticketValues.patronymic}
                           onChange={e =>
                               setTicketValues({...ticketValues, patronymic: e.target.value})}/>
                </div>
                <div>
                    <label htmlFor="sex">Sex</label>
                    <input type="text" name='sex' placeholder="Enter seat capacity"
                           value={ticketValues.sex}
                           onChange={e =>
                               setTicketValues({...ticketValues, sex: e.target.value})}/>
                </div>
                <div>
                    <label htmlFor="dateOfBirth">Date of birth</label>
                    <input type="text" name='dateOfBirth' placeholder="Enter seat capacity"
                           value={ticketValues.dateOfBirth}
                           onChange={e =>
                               setTicketValues({...ticketValues, dateOfBirth: e.target.value})}/>
                </div>
                <div>
                    <label htmlFor="passSeries">Pass series</label>
                    <input type="text" name='passSeries' placeholder="Enter seat capacity"
                           value={ticketValues.passSeries}
                           onChange={e =>
                               setTicketValues({...ticketValues, passSeries: e.target.value})}/>
                </div>
                <div>
                    <label htmlFor="passNum">Pass number</label>
                    <input type="text" name='passNum' placeholder="Enter seat capacity"
                           value={ticketValues.passNum}
                           onChange={e =>
                               setTicketValues({...ticketValues, passNum: e.target.value})}/>
                </div>
                <div>
                    <label htmlFor="phoneNum">Phone number</label>
                    <input type="text" name='phoneNum' placeholder="Enter seat capacity"
                           value={ticketValues.phoneNum}
                           onChange={e =>
                               setTicketValues({...ticketValues, phoneNum: e.target.value})}/>
                </div>
                <div>
                    <label htmlFor="seat">Seat</label>
                    <select type="text" name='seat' placeholder="Enter seat capacity"
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
                <button onClick={() => handleBooking()}>Book</button>
                <button onClick={() => handleBuying()}>Buy</button>
            </form>
        </div>
    )
}

export default IssueTripTicket