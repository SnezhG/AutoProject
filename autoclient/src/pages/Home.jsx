import React, {useState} from "react";
import axios from "axios";
import {useNavigate} from "react-router-dom";
import {Button, Col, Container, Form, InputGroup, Row} from "react-bootstrap";

function Home(){
    const [values, setValues] = useState({
        arrCity: '',
        depCity: '',
        depDate: ''
    })

    const [searchResults, setSearchResults] = useState([])
    const [showResults, setShowResults] = useState(false)
    const handleSubmit = (event) =>{
        event.preventDefault();
        axios.post('https://localhost:7089/api/Trips/FindTrips', values)
            .then(res => {
                console.log(res);
                setSearchResults(res.data);
                setShowResults(true);
            })
            .catch(err => console.log(err));
    }

    const navigator = useNavigate()
    const buyTicket = (id) => {
        navigator(`/IssueTripTicket/${id}`)
    }

    return (
        <Container className="mt-5 textt-center" style={{ width: '80%', border: '1px solid' }}>
            <Row>
                <Col>
                    <h1 className="mb-4">Билеты на автобус</h1>
                    <Form onSubmit={handleSubmit}>
                                <InputGroup className="mb-3" style={{ width: '100%', border: '1px solid' }}>
                                    <Form.Control
                                        type="text"
                                        name="depCity"
                                        placeholder="Откуда"
                                        onChange={(e) =>
                                            setValues({ ...values, depCity: e.target.value })}
                                    />
                                    <Button
                                        type="button"
                                        variant="secondary"
                                        onClick={() => {
                                            setValues({
                                                ...values,
                                                depCity: values.arrCity,
                                                arrCity: values.depCity,
                                            });
                                        }}
                                        className="mb-3"
                                    >
                                        Поменять местами
                                    </Button>
                                    <Form.Control
                                        type="text"
                                        name="arrCity"
                                        placeholder="Куда"
                                        onChange={(e) =>
                                            setValues({ ...values, arrCity: e.target.value })}
                                    />
                                    <Form.Control
                                        type="date"
                                        name="depDate"
                                        placeholder="Когда"
                                        onChange={(e) =>
                                            setValues({ ...values, depDate: e.target.value })}
                                    />
                                    <Button type="submit" variant="primary" className="mb-3">
                                        Найти
                                    </Button>
                                </InputGroup>
                    </Form>
                </Col>
            </Row>
            {showResults && (
                <Row>
                    <Col>
                        {searchResults.length > 0 ? (
                            <div>
                                {searchResults.map((trip) => (
                                    <div key={trip.tripId} className="card mb-3">
                                        <div className="card-body">
                                            <Row className="col-4" style={{ width: '100%' }}>
                                                <Col className="d-flex flex-column align-items-center justify-content-center">
                                                    <p className="font-weight-bold">{trip.depTime}</p>
                                                    <p>{trip.depCity}</p>
                                                </Col>
                                                <Col className="d-flex flex-column align-items-center justify-content-center">
                                                    <p className="font-weight-bold">{trip.arrTime}</p>
                                                    <p>{trip.arrCity}</p>
                                                </Col>
                                                <Col className="d-flex flex-column align-items-center justify-content-center">
                                                    <p>Цена</p>
                                                    <p>{trip.price}</p>
                                                </Col>
                                                <Col className="d-flex flex-column align-items-center justify-content-center">
                                                    <Button onClick={() => buyTicket(trip.tripId)} variant="primary">
                                                        Выбрать
                                                    </Button>
                                                </Col>
                                            </Row>
                                        </div>
                                    </div>
                                ))}
                            </div>
                        ) : (
                            <h1>Рейсы не найдены</h1>
                        )}
                    </Col>
                </Row>
            )}
        </Container>
    )
}

export default Home