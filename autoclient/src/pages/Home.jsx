import React, {useState} from "react";
import axios from "axios";
import {useNavigate} from "react-router-dom";
import {Button, Col, Container, Form, Row} from "react-bootstrap";

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
        <Container className="mt-5">
            <Row>
                <Col>
                    <h1 className="mb-4">Билеты на автобус</h1>
                    <Form onSubmit={handleSubmit}>
                        <Row className="mb-3">
                            <Col>
                                <Form.Control
                                    type="text"
                                    name="depCity"
                                    placeholder="Откуда"
                                    onChange={(e) => 
                                        setValues({ ...values, depCity: e.target.value })}
                                />
                            </Col>
                            <Col>
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
                            </Col>
                            <Col>
                                <Form.Control
                                    type="text"
                                    name="arrCity"
                                    placeholder="Куда"
                                    onChange={(e) => 
                                        setValues({ ...values, arrCity: e.target.value })}
                                />
                            </Col>
                            <Col>
                                <Form.Control
                                    type="date"
                                    name="depDate"
                                    placeholder="Когда"
                                    onChange={(e) => 
                                        setValues({ ...values, depDate: e.target.value })}
                                />
                            </Col>
                            <Col>
                                <Button type="submit" variant="primary" className="mb-3">
                                    Найти
                                </Button>
                            </Col>
                        </Row>
                    </Form>
                </Col>
            </Row>
            {showResults && (
                <Row>
                    <Col>
{/*                        <h1 className="mb-4">Найденные рейсы</h1>*/}
                        {searchResults.length > 0 ? (
                            <div>
                                {searchResults.map((trip) => (
                                    <div key={trip.tripId} className="card mb-3">
                                        <div className="card-body">
                                            <p>{`${trip.depCity} - ${trip.arrCity}`}</p>
                                            <p>{`Отправление: ${trip.depTime} - Прибытие: ${trip.arrTime}`}</p>
                                            <p>{`Цена: ${trip.price}`}</p>
                                            <Button onClick={() => 
                                                buyTicket(trip.tripId)} variant="primary">
                                                Купить
                                            </Button>
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