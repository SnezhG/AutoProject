import {Link, useNavigate, useParams} from "react-router-dom";
import {useEffect, useState} from "react";
import axios from "axios";
import {Button, Card, CardFooter, Container} from "react-bootstrap";
import DeleteConfirmationModal from "@/pages/DeleteConfirmation.jsx";

function Ticket(){
    const {id} = useParams()

    const [showConfirmation, setShowConfirmation] = useState(false)
    const [ticektIdToCancell, setTicektIdToCancell] = useState(null)
    const [confirmationMessage, setConfirmationMessage] = useState('')

    const removeData = (id) => {
        setTicektIdToCancell(id)
        setConfirmationMessage(`Вы уверены, что хотите отменить бронирование?`);
        setShowConfirmation(true)
    }

    const confirmDelete = () => {
        if (ticektIdToCancell) {
            axios.put(`https://localhost:7089/api/Tickets/CancelBooking`, ticektIdToCancell)
                .then((res) => {
                    console.log("deleted^ ", res.data)
                    usingAxios()
                })
            setShowConfirmation(false)
            setTicektIdToCancell(null)
        }
    }
    

    const [values, setValues] = useState([])

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
    
/*    const handleCancelling = (id) => {
        axios.put(`https://localhost:7089/api/Tickets/CancelBooking`, id)
            .then(res =>{
            console.log("Cancelled ", res.data)
        })
    }*/

    const usingAxios = () => {
        axios.get(`https://localhost:7089/api/Tickets/${id}`)
            .then(res =>
                setValues(res.data))
            .catch(err => console.log(err))
    }

    useEffect(() => {
        usingAxios()
    }, [])

    return (
        <Container className="mt-5" style={{width: '40%'}}>
            <Card className="p-3 rounded">
                <h1 className="text-center mb-4">Билет</h1>
                <div>
                    <label className="form-label" style={{fontWeight: 'bold'}}>Пассажир</label>
                    <p>{`${values.name} ${values.surname} ${values.patronymic}`}</p>
                </div>
                <div>
                    <label className="form-label" style={{fontWeight: 'bold'}}>Маршрут</label>
                    <p>{`${values.depCity} - ${values.arrCity}`}</p>
                </div>
                <div>
                    <label className="form-label" style={{fontWeight: 'bold'}}>Дата и время отправки</label>
                    <p>{`${values.depDate} ${values.depTime}`}</p>
                </div>
                <div>
                    <label className="form-label" style={{fontWeight: 'bold'}}>Дата и время прибытия</label>
                    <p>{`${values.arrDate} ${values.arrTime}`}</p>
                </div>
                <div>
                    <label className="form-label" style={{fontWeight: 'bold'}}>Место</label>
                    <p>{values.seat}</p>
                </div>
                <div>
                    <label className="form-label" style={{fontWeight: 'bold'}}>Цена</label>
                    <p>{values.price}</p>
                </div>
                <div>
                    <label className="form-label" style={{fontWeight: 'bold'}}>Статус</label>
                    <p className={getStatusColor(values.status)}>
                        {values.status === "paid" ? "Оплачен" :
                            values.status === "expired" ? "Истек" :
                                values.status === "cancelled" ? "Отменен" :
                                    values.status === "booked" ? "Забронирован" :
                                        values.status === "issued" ? "Оформлен" : ""}
                    </p>
                </div>
                <CardFooter>
                    <Link className="btn m-1" to={`/TicketPay/${values.id}`}>Оплатить</Link>
                    <Button
                        className="btn-danger m-1"
                        onClick={() => removeData(values.id)}>
                        Отменить бронь
                    </Button>
                    <Link className="btn m-1" to={`/ClientTickets`}>Назад</Link>
                </CardFooter>
            </Card>
            <DeleteConfirmationModal
                show={showConfirmation}
                onHide={() => setShowConfirmation(false)}
                onConfirm={confirmDelete}
                confirmationMessage={confirmationMessage}
            />
        </Container>
    )
}

export default Ticket