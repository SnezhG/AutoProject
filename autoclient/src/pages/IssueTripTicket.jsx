import {useNavigate, useParams} from "react-router-dom";
import {useEffect, useState} from "react";
import axios from "axios";

function IssueTripTicket(){
    const {id} = useParams();

    const [tripValues, setTripValues] = useState( {
        depCity: '',
        arrCity: '',
        depTime: '',
        bus: '',
        seats: ''
    })

    const seats = tripValues.bus ? tripValues.bus.seats : []

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
            .then(res => setTripValues(res.data))
            .catch(err => console.log(err))
    }, []);
    
    const navigate = useNavigate();
    const handleBooking = (event) =>{
        event.preventDefault();
        axios.post(`https://localhost:7089/api/Tickets/BookTicket`, ticketValues)
            .then(res => {
                console.log(res);
            })
            .catch(err => console.log(err))
    }
    return(
        <div>
            <h1>Trip</h1>
            <form>
                <div>
                    <input type="text" name='depCity' value={tripValues.depCity}/>
                    <input type="text" name='arrCity' value={tripValues.arrCity}/>
                    <input type="text" name='depDate' value={tripValues.depTime}/>
                </div>
            </form>

            <h1>Issue Ticket</h1>
            <form onSubmit={handleBooking}>
                <div>
                    <label htmlFor="lastName">Name</label>
                    <input type="text" name='lastName' placeholder="Enter seat capacity"
                           value={ticketValues.lastName}
                           onChange={e =>
                               setTicketValues({...values, lastName: e.target.value})}/>
                </div>
                <div>
                    <label htmlFor="name">Last name</label>
                    <input type="text" name='name' placeholder="Enter seat capacity"
                           value={ticketValues.name}
                           onChange={e =>
                               setTicketValues({...values, name: e.target.value})}/>
                </div>
                <div>
                    <label htmlFor="patronymic">Patronimyc</label>
                    <input type="text" name='patronymic' placeholder="Enter seat capacity"
                           value={ticketValues.patronymic}
                           onChange={e =>
                               setTicketValues({...values, patronymic: e.target.value})}/>
                </div>
                <div>
                    <label htmlFor="sex">Sex</label>
                    <input type="text" name='sex' placeholder="Enter seat capacity"
                           value={ticketValues.sex}
                           onChange={e =>
                               setTicketValues({...values, sex: e.target.value})}/>
                </div>
                <div>
                    <label htmlFor="dateOfBirth">Date of birth</label>
                    <input type="text" name='dateOfBirth' placeholder="Enter seat capacity"
                           value={ticketValues.dateOfBirth}
                           onChange={e =>
                               setTicketValues({...values, dateOfBirth: e.target.value})}/>
                </div>
                <div>
                    <label htmlFor="passSeries">Pass series</label>
                    <input type="text" name='passSeries' placeholder="Enter seat capacity"
                           value={ticketValues.passSeries}
                           onChange={e =>
                               setTicketValues({...values, passSeries: e.target.value})}/>
                </div>
                <div>
                    <label htmlFor="passNum">Pass number</label>
                    <input type="text" name='passNum' placeholder="Enter seat capacity"
                           value={ticketValues.passNum}
                           onChange={e =>
                               setTicketValues({...values, passNum: e.target.value})}/>
                </div>
                <div>
                    <label htmlFor="phoneNum">Phone number</label>
                    <input type="text" name='phoneNum' placeholder="Enter seat capacity"
                           value={ticketValues.phoneNum}
                           onChange={e =>
                               setTicketValues({...values, phoneNum: e.target.value})}/>
                </div>
                <div>
                    <label htmlFor="seat">Seat</label>
                    <select type="text" name='seat' placeholder="Enter seat capacity"
                            value={ticketValues.seat}
                            onChange={e =>
                                setTicketValues({...values, seat: e.target.value})}>
                        <option value="" disabled>Select a seat</option>
                        {seats.map((seat) => (
                            <option key={seat.seatId} value={seat.seatId} disabled={!seat.available}>
                                {seat.num}
                            </option>
                        ))}
                    </select>
                </div>
                <button>Book</button>
            </form>
        </div>
    )
}

export default IssueTripTicket