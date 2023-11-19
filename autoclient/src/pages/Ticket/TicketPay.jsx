import {useNavigate, useParams} from "react-router-dom";
import axios from "axios";

function TicketPay(){
    const {id} = useParams()
    const navigate = useNavigate()
    const handlePaying = () => {
        axios.post(`https://localhost:7089/api/Tickets/PayForTicket`, JSON.stringify(id), {
            headers: {
                'Content-Type': 'application/json',
            },
        })
            .then(res => {
            console.log(res)
            return res.data
        })
            .then((firstRes) =>{
                navigate(`/Ticket/${firstRes}`)
            })
            .catch(err => console.log(err))
    }
    
    return(
        <div className="container mt-5" style={{width: '70%'}}>
            <div className="card p-3 rounded">
                <h1 className="text-center">Оплата билета</h1>
                <div className="mb-3">
                    <label className="form-label">Номер карты</label>
                    <input type="text" className="form-control" name="cardNumber" />
                </div>
                <div className="mb-3">
                    <label className="form-label">Владелец</label>
                    <input type="text" className="form-control" name="cardName" />
                </div>
                <div className="mb-3">
                    <label className="form-label">CVV</label>
                    <input type="text" className="form-control" name="cardCVV" />
                </div>
                <button 
                    onClick={() => handlePaying()} className="btn btn-primary">
                    Оплатить
                </button>
            </div>
        </div>
    )
}

export default TicketPay