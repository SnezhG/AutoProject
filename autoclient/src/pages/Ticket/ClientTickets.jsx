import {useEffect, useState} from "react";
import axios from "axios";

function ClientTickets(){
    
    const [values, setValues] = useState([]);
    const usingAxios = () => {
        axios.get("https://localhost:7089/api/Tickets")
            .then((response) => {
                setValues(response.data);
            });
    };

    useEffect(() => {
        usingAxios();
    }, []);

    const getStatusColor = (status) => {
        switch (status) {
            case "paid":
                return "text-success";
            case "expaired":
            case "cancelled":
                return "text-danger";
            case "booked":
                return "text-primary";
            default:
                return "";
        }
    };

    return (
        <div>
            {tickets.length > 0 ? (
                tickets.map((ticket, index) => (
                    <div key={index} className="mb-3 border p-3 rounded">
                        <p>{`${ticket.depCity} - ${ticket.arrCity}`}</p>
                        <p>{`${ticket.depTime} - ${ticket.arrTime}`}</p>
                        <p className={getStatusColor(ticket.status)}>
                            {ticket.status === "paid" ? "Оплачен" :
                                ticket.status === "expaired" ? "Истек" :
                                    ticket.status === "cancelled" ? "Отменен" :
                                        ticket.status === "booked" ? "Забронирован" : ""}
                        </p>
                        <Button variant="info">Детали</Button>
                    </div>
                ))
            ) : (
                <p>Билеты не найдены</p>
            )}
        </div>
    )
}

export default ClientTickets