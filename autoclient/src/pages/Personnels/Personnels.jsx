import axios from "axios";
import { useEffect, useState } from "react";
import {Link} from "react-router-dom";
import {Card, Col, Row} from "react-bootstrap";
import DeleteConfirmationModal from "@/pages/DeleteConfirmation.jsx";

function Personnels() {
    const userRole = localStorage.getItem('role')
    const [showConfirmation, setShowConfirmation] = useState(false)
    const [persIdToDelete, setPersIdToDelete] = useState(null)
    const [confirmationMessage, setConfirmationMessage] = useState('')

    const removeData = (id, name, surname, post) => {
        setPersIdToDelete(id)
        setConfirmationMessage(`Вы уверены, что хотите удалить сотрудника ${name} ${surname} на должности ${post}?`);
        setShowConfirmation(true)
    }

    const confirmDelete = () => {
        if (persIdToDelete) {
            axios.delete(`https://localhost:7089/api/Personnels/${persIdToDelete}`)
                .then((res) => {
                    console.log("deleted^ ", res.data)
                    usingAxios()
                })
            setShowConfirmation(false)
            setPersIdToDelete(null)
        }
    }
    
    const [personnels, setPersonnels] = useState([]);
    const usingAxios = () => {
        axios.get("https://localhost:7089/api/Personnels")
            .then((response) => {
            setPersonnels(response.data);
        })
    }

    useEffect(() => {
        usingAxios();
    }, []);

    return (
        <div className="container" style={{marginTop: '2rem'}}>
            {userRole === 'admin' && (            
                <div className="mb-3">
                    <Link to="/CreatePersonnel" className="btn btn-primary">
                        Создать
                    </Link>
                </div>
            )}
            <Row className="row-cols-4 gy-3">
                {personnels.map((pers) => (
                    <Col key={pers.personnelId} className="h-100">
                        <Card className="h-100">
                            <Card.Body>
                                <Card.Text>
                                    <p><strong>{pers.post}</strong></p>
                                    <p><strong>ФИО</strong></p>
                                    <p>{pers.surname} {pers.name} {pers.patronimyc}</p>
                                    <p><strong>Стаж (в годах)</strong></p>
                                    <p>{pers.experience}</p>
                                </Card.Text>
                            </Card.Body>
                            {userRole === 'admin' && (
                                <Card.Footer className="d-flex justify-content-end p-2">
                                    <Link to={`/EditPersonnel/${pers.personnelId}`} className="btn btn-primary m-1">
                                        Изменить
                                    </Link>
                                    <button
                                        onClick={() => removeData(pers.personnelId,
                                            pers.name, pers.surname, pers.post)}
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

export default Personnels