import axios from "axios";
import { useEffect, useState } from "react";
import {Link} from "react-router-dom";
import {Card, CardImg, Col, Row} from "react-bootstrap";
import DeleteConfirmationModal from "@/pages/DeleteConfirmation.jsx";
import {AuthProvider, useAuth} from "@/pages/Auth/AuthContext.jsx";

function Buses() {
    const userRole = localStorage.getItem('role')
    const [showConfirmation, setShowConfirmation] = useState(false)
    const [busIdToDelete, setBusIdToDelete] = useState(null)
    const [confirmationMessage, setConfirmationMessage] = useState('')
    
    const removeData = (id, model) => {
        setBusIdToDelete(id)
        setConfirmationMessage(`Вы уверены, что хотите удалить автобус с моделью ${model}?`);
        setShowConfirmation(true)
    }

    const confirmDelete = () => {
        if (busIdToDelete) {
            axios.delete(`https://localhost:7089/api/Buses/${busIdToDelete}`)
                .then((res) => {
                console.log("deleted^ ", res.data)
                usingAxios()
            })
            setShowConfirmation(false)
            setBusIdToDelete(null)
        }
    }
    
    const [buses, setBuses] = useState([]);
    const usingAxios = () => {
        axios.get("https://localhost:7089/api/Buses")
            .then((response) => {
            setBuses(response.data);
        })
    }

    useEffect(() => {
        usingAxios();
    }, [])

    return (
        <div className="container" style={{marginTop: '2rem'}}>
            {userRole === 'admin' && (
                <div className="mb-3">
                    <Link to="/CreateBus" className="btn btn-primary">
                        Создать
                    </Link>
                </div>
            )}
            <Row className="row-cols-4 gy-3">
                    {buses.map((bus) => (
                        <Col key={bus.busId} className="h-100">
                            <Card className="h-100">
                                <Card.Body>
                                    <Card.Text>
                                        <p><strong>Модель:</strong></p>
                                        <p>{bus.model}</p>
                                        <p><strong>Кол-во мест:</strong></p>
                                        <p>{bus.seatCapacity}</p>
                                        <p><strong>Харакетристики:</strong></p>
                                        <p>{bus.specs}</p>
                                    </Card.Text>
                                </Card.Body>
                                {userRole === 'admin' && (
                                    <Card.Footer className="d-flex justify-content-end p-2">
                                        <Link to={`/EditBus/${bus.busId}`} className="btn btn-primary m-1">
                                            Изменить
                                        </Link>
                                        <button
                                            onClick={() => removeData(bus.busId, bus.model)}
                                            className="btn btn-danger m-1"
                                        >
                                            Удалить
                                        </button>
                                    </Card.Footer>
                                )}
                            </Card>
                        </Col>
                    ))}
            </Row>
            <DeleteConfirmationModal
                show={showConfirmation}
                onHide={() => setShowConfirmation(false)}
                onConfirm={confirmDelete}
                confirmationMessage={confirmationMessage}
            />
        </div>
    )
}
export default Buses