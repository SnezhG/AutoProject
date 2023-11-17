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
        <Container className="mt-5" style={{ width: '33%' }}>
            <Row className="mb-3">
                <Col>
                    <div className="border p-3">
                        <Form>
                            <Form.Group className="mb-3">
                                <Form.Control
                                    type="text"
                                    readOnly
                                    plaintext
                                    value={`${routeValues.depCity} - ${routeValues.arrCity}`}
                                />
                            </Form.Group>
                            <Form.Group className="mb-3">
                                <Form.Control type="text" readOnly plaintext 
                                              value={`Время отправки: ${tripValues.depTime}`} />
                            </Form.Group>
                            <Form.Group className="mb-3">
                                <Form.Control type="text" readOnly plaintext 
                                              value={`Цена: ${tripValues.price}`} />
                            </Form.Group>
                        </Form>
                    </div>
                </Col>
            </Row>
            <Row>
                <Col>
                    <Form onSubmit={handleBooking}>
                        <Form.Group className="mb-3">
                            <Form.Label htmlFor="name">Фамилия Имя Отчество</Form.Label>
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
                            <Form.Label htmlFor="passSeriesNum">Серия и Номер паспорта</Form.Label>
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
                                    Выберите пол
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
                                    Выберите место
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