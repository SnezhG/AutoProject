import {Route, useNavigate, useParams} from "react-router-dom";
import {useEffect, useState} from "react";
import axios from "axios";
import {Col, Container, Form, Row} from "react-bootstrap";

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
                return res.data
            })
            .then((firstRes) =>{
                navigate(`/Ticket/${firstRes}`)
            })
            .catch(err => console.log(err))
    }

    const handleBuying = async (event) =>{
        event.preventDefault()
        setTicketValues({...ticketValues, trip: tripValues.tripId})
        await axios.post(`https://localhost:7089/api/Tickets/BuyTicket`, ticketValues)
            .then(res => {
                console.log(res)
                return res.data
            })
            .then((firstRes) =>{
                navigate(`/TicketPay/${firstRes}`)
            })
            .catch(err => console.log(err))
    }
    
    return(
        <Container className="mt-5" style={{ width: '70%' }}>
            <Row className="mb-3">
                <Col>
                    <div className="card mb-3">
                        <div className="card-body">
                            <Row className="col-4" style={{ width: '100%' }}>
                                <Col className="d-flex flex-column align-items-center justify-content-center">
                                    <p><strong>Время отправки</strong></p>
                                    <p>{tripValues.depTime}</p>
                                    <p>{routeValues.depCity}</p>
                                </Col>
                                <Col className="d-flex flex-column align-items-center justify-content-center">
                                    <p><strong>Время прибытия</strong></p>
                                    <p>{tripValues.depTime}</p>
                                    <p>{routeValues.arrCity}</p>
                                </Col>
                                <Col className="d-flex flex-column align-items-center justify-content-center">
                                    <p><strong>Цена</strong></p>
                                    <p>{tripValues.price}</p>
                                </Col>
                            </Row>
                        </div>
                    </div>
                </Col>
            </Row>
            <Row>
                <Col>
                    <Form onSubmit={handleBooking} style={{width: '70%'}}>
                        <Form.Group className="mb-3">
                            <Form.Label htmlFor="name">Пассажир</Form.Label>
                            <div className="d-flex">
                                <Form.Control
                                    type="text"
                                    name="lastName"
                                    placeholder="Фамилия"
                                    value={ticketValues.lastName}
                                    onChange={(e) => 
                                        setTicketValues({ ...ticketValues, lastName: e.target.value })}
                                    className="me-2"
                                />
                                <Form.Control
                                    type="text"
                                    name="name"
                                    placeholder="Имя"
                                    value={ticketValues.name}
                                    onChange={(e) => 
                                        setTicketValues({ ...ticketValues, name: e.target.value })}
                                    className="me-2"
                                />
                                <Form.Control
                                    type="text"
                                    name="patronymic"
                                    placeholder="Отчество"
                                    value={ticketValues.patronymic}
                                    onChange={(e) => 
                                        setTicketValues({ ...ticketValues, patronymic: e.target.value })}
                                />
                            </div>
                        </Form.Group>
                        <Form.Group className="mb-3">
                            <Form.Label htmlFor="passSeriesNum">Серия и номер паспорта</Form.Label>
                            <div className="d-flex">
                                <Form.Control
                                    type="text"
                                    name="passSeries"
                                    placeholder="Серия"
                                    value={ticketValues.passSeries}
                                    onChange={(e) => 
                                        setTicketValues({ ...ticketValues, passSeries: e.target.value })}
                                    className="me-2"
                                />
                                <Form.Control
                                    type="text"
                                    name="passNum"
                                    placeholder="Номер"
                                    value={ticketValues.passNum}
                                    onChange={(e) => 
                                        setTicketValues({ ...ticketValues, passNum: e.target.value })}
                                />
                            </div>
                        </Form.Group>
                        <Form.Group className="mb-3">
                            <Form.Label htmlFor="dateOfBirth">Дата рождения</Form.Label>
                            <Form.Control
                                type="date"
                                name="dateOfBirth"
                                value={ticketValues.dateOfBirth}
                                onChange={(e) =>
                                    setTicketValues({ ...ticketValues, dateOfBirth: e.target.value })}
                            />
                        </Form.Group>
                        <Form.Group className="mb-3">
                            <Form.Label htmlFor="sex">Пол</Form.Label>
                            <Form.Control
                                as="select"
                                name="sex"
                                value={ticketValues.sex}
                                onChange={(e) =>
                                    setTicketValues({ ...ticketValues, sex: e.target.value })}
                            >
                                <option value="" disabled>
                                    Пол
                                </option>
                                <option value="Male">Мужской</option>
                                <option value="Female">Женский</option>
                            </Form.Control>
                        </Form.Group>
                        <Form.Group className="mb-3">
                            <Form.Label htmlFor="phoneNum">Номер телефона</Form.Label>
                            <Form.Control
                                type="text"
                                name="phoneNum"
                                placeholder="Номер телефона"
                                value={ticketValues.phoneNum}
                                onChange={(e) => 
                                    setTicketValues({ ...ticketValues, phoneNum: e.target.value })}
                            />
                        </Form.Group>
                        <Form.Group className="mb-3">
                            <Form.Label htmlFor="seat">Место</Form.Label>
                            <Form.Control
                                as="select"
                                name="seat"
                                value={ticketValues.seat}
                                onChange={(e) => 
                                    setTicketValues({ ...ticketValues, seat: e.target.value })}
                            >
                                <option value="" disabled>
                                    Место
                                </option>
                                {seatsValues
                                    .filter((seat) => seat.available)
                                    .map((seat) => (
                                        <option key={seat.seatId} value={seat.num}>
                                            {seat.num}
                                        </option>
                                    ))}
                            </Form.Control>
                        </Form.Group>
                        <button type="submit" className="btn btn-primary" onClick={handleBuying}>
                            Купить
                        </button>
                        <button type="button" className="btn btn-secondary ms-2" onClick={handleBuying}>
                            Забронировать
                        </button>
                    </Form>
                </Col>
            </Row>
        </Container>
    )
}

export default IssueTripTicket