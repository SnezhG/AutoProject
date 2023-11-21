import React, {useState} from "react";
import axios from "axios";
import {Link, useNavigate} from "react-router-dom";
import iconPath from '../assets/swap.png'
import {Button, Col, Container, Form, InputGroup, Row} from "react-bootstrap";

function Home(){
    const isUserLoggedIn = localStorage.getItem('isUserLoggedIn');
    const userRole = localStorage.getItem('role')
    const [values, setValues] = useState({
        arrCity: '',
        depCity: '',
        depDate: ''
    })

    const [searchResults, setSearchResults] = useState([])
    const [showResults, setShowResults] = useState(false)
    const handleSubmit = (event) =>{
        event.preventDefault();
        axios.post('https://localhost:5275/api/Trips/FindTrips', values)
            .then(res => {
                console.log(res);
                setSearchResults(res.data);
                setShowResults(true);
            })
            .catch(err => console.log(err));
    }

    const navigator = useNavigate()
    const buyTicket = (id) => {
        console.log(isUserLoggedIn)
        if(isUserLoggedIn === 'true'){
            navigator(`/IssueTripTicket/${id}`);
        } else {
            navigator("/Auth/Login")
        }
    }

    return (
        <Container className="mt-5 text-center border p-4 rounded" style={{ width: '80%'}}>
            <Row>
                <Col>
                    <h1 className="mb-4">Билеты на автобус</h1>
                    <Form onSubmit={handleSubmit}>
                                <InputGroup className="mb-3" style={{ width: '100%' }}>
                                    <Form.Control
                                        type="text"
                                        name="depCity"
                                        placeholder="Откуда"
                                        onChange={(e) =>
                                            setValues({ ...values, depCity: e.target.value })}
                                        style={{ borderColor: '#7e5539', borderWidth:'medium'}}
                                    />
                                    <Form.Control
                                        type="text"
                                        name="arrCity"
                                        placeholder="Куда"
                                        onChange={(e) =>
                                            setValues({ ...values, arrCity: e.target.value })}
                                        style={{ borderColor: '#7e5539', borderWidth:'medium'}}
                                    />
                                    <Form.Control
                                        type="date"
                                        name="depDate"
                                        placeholder="Когда"
                                        onChange={(e) =>
                                            setValues({ ...values, depDate: e.target.value })}
                                        style={{ borderColor: '#7e5539', borderWidth:'medium'}}
                                    />
                                    <Button type="submit" style={{ borderColor: '#7e5539', backgroundColor: '#7e5539'}}>
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
                                                    <p style={{fontSize: '1.3rem'}}><strong>{trip.depTime}</strong></p>
                                                    <p>{trip.depDate}</p>
                                                    <p>{trip.depCity}</p>
                                                </Col>
                                                <Col className="d-flex flex-column align-items-center justify-content-center">
                                                    <p style={{fontSize: '1.3rem'}}><strong>{trip.arrTime}</strong></p>
                                                    <p>{trip.arrDate}</p>
                                                    <p>{trip.arrCity}</p>
                                                </Col>
                                                <Col className="d-flex flex-column align-items-center justify-content-center">
                                                    <p><strong>Цена</strong></p>
                                                    <p>{trip.price}р.</p>
                                                </Col>
                                                <Col className="d-flex flex-column align-items-center justify-content-center">
                                                    <Button onClick={() => buyTicket(trip.tripId)} 
                                                            style={{ borderColor: '#7e5539', backgroundColor: '#7e5539'}}>
                                                        Выбрать
                                                    </Button>
                                                    {userRole === 'dispatcher' && (
                                                        <Link to={`/EditTrip/${trip.tripId}`}
                                                              className="btn m-2"
                                                                style={{ borderColor: '#7e5539', backgroundColor: '#7e5539', color:'white'}}>
                                                            Изменить
                                                        </Link>
                                                    )}
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