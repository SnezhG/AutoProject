import {useEffect, useState} from "react";
import axios from "axios";
import {Button, Col, Container, Row} from "react-bootstrap";
import {Link} from "react-router-dom";

function ClientTickets(){
    
    const [tickets, setTickets] = useState([]);
    const usingAxios = () => {
        axios.get("https://localhost:5275/api/Tickets", {
            headers: {
                Authorization: `Bearer ${localStorage.getItem('token')}`
            }
        })
            .then((response) => {
                setTickets(response.data);
            });
    };

    useEffect(() => {
        usingAxios();
    }, []);

    const getStatusColor = (status) => {
        switch (status) {
            case "paid":
                return "text-success";
            case "expired":
            case "cancelled":
                return "text-danger";
            case "booked":
                return "text-primary";
            case "issued":
                return "text-primary"
            default:
                return "";
        }
    }

    return (
        <Container className="mt-5">
            <Row className="row-cols-4 gy-3">
                {tickets.length > 0 ? (
                    tickets.map((ticket) => (
                        <Col>
                            <div key={ticket.id} className="mb-3 border p-3 rounded" style={{backgroundColor:'white'}}>
                                <label className="form-label" style={{fontWeight: 'bold'}}>Маршрут</label>
                                <p>{`${ticket.depCity} - ${ticket.arrCity}`}</p>
                                <label className="form-label" style={{fontWeight: 'bold'}}>Время</label>
                                <p>{`${ticket.depTime} - ${ticket.arrTime}`}</p>
                                <label className="form-label" style={{fontWeight: 'bold'}}>Статус</label>
                                <p className={getStatusColor(ticket.status)}>
                                    <strong>
                                        {ticket.status === "paid" ? "Оплачен" :
                                            ticket.status === "expired" ? "Истек" :
                                                ticket.status === "cancelled" ? "Отменен" :
                                                    ticket.status === "booked" ? "Забронирован" :
                                                        ticket.status === "issued" ? "Оформлен" : ""}
                                    </strong>
                                </p>
                                <Link className="btn m-1"
                                      style={{backgroundColor:'#7e5539', borderColor:'#7e5539', color:'white'}}      
                                      to={`/Ticket/${ticket.id}`}>Детали</Link>
                            </div>
                        </Col>
                    ))
                ) : (
                    <p>Билеты не найдены</p>
                )}
            </Row>
        </Container>
    )
}

export default ClientTickets