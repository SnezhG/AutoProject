import axios from "axios";
import {useEffect, useState} from "react";
import {Link} from "react-router-dom";
import {Card, Col, Row} from "react-bootstrap";
import DeleteConfirmationModal from "@/pages/DeleteConfirmation.jsx";

function Busroutes(){
    const userRole = localStorage.getItem('role')
    const [showConfirmation, setShowConfirmation] = useState(false)
    const [routeIdToDelete, setRouteIdToDelete] = useState(null)
    const [confirmationMessage, setConfirmationMessage] = useState('')

    const removeData = (id, depCity, arrCity) => {
        setRouteIdToDelete(id)
        setConfirmationMessage(`Вы уверены, что хотите удалить маршрут ${depCity} - ${arrCity}?`);
        setShowConfirmation(true)
    }

    const confirmDelete = () => {
        if (routeIdToDelete) {
            axios.delete(`https://localhost:5275/api/Busroutes/${routeIdToDelete}`)
                .then((res) => {
                    console.log("deleted^ ", res.data)
                    usingAxios()
                })
            setShowConfirmation(false)
            setRouteIdToDelete(null)
        }
    }

    const [busroutes, setBusroutes] = useState([]);
    const usingAxios = () => {
        axios.get("https://localhost:5275/api/Busroutes", {
            headers:{
                Authorization: `Bearer ${localStorage.getItem('token')}`
            }
        })
            .then((response) => {
            setBusroutes(response.data);
        })
    }

    useEffect(() => {
        usingAxios();
    }, [])

    return (
        <div className="container" style={{marginTop: '2rem'}}>
            {userRole === 'admin' && (
                <div className="mb-3">
                    <Link to="/CreateBusroute" className="btn btn-primary" style={{backgroundColor:'#7e5539', borderColor:'#7e5539'}}>
                        Создать
                    </Link>
                </div>
            )}
            <Row className="row-cols-4 gy-3">
                {busroutes.map((route) => (
                    <Col key={route.routeId} className="h-100">
                        <Card className="h-100">
                            <Card.Body>
                                <Card.Text>
                                    <p><strong>Город отправки:</strong></p>
                                    <p>{route.depCity}</p>
                                    <p><strong>Город прибытия:</strong></p>
                                    <p>{route.arrCity}</p>
                                </Card.Text>
                            </Card.Body>
                            {userRole === 'admin' && (
                                <Card.Footer className="d-flex justify-content-end p-2">
                                    <Link to={`/EditBusroute/${route.routeId}`} style={{backgroundColor:'#7e5539', borderColor:'#7e5539'}} className="btn btn-primary m-1">
                                        Изменить
                                    </Link>
                                    <button
                                        onClick={() => removeData(route.routeId, route.depCity, route.arrCity)}
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

export default Busroutes