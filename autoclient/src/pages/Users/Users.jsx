import axios from "axios";
import {useEffect, useState} from "react";
import {Link} from "react-router-dom";
import {Card, Col, Row} from "react-bootstrap";
import DeleteConfirmationModal from "@/pages/DeleteConfirmation.jsx";

function Users(){
    
    const [showConfirmation, setShowConfirmation] = useState(false)
    const [userIdToDelete, setUserIdToDelete] = useState(null)
    const [confirmationMessage, setConfirmationMessage] = useState('')

    const removeData = (id, email) => {
        setUserIdToDelete(id)
        setConfirmationMessage(`Вы уверены, что хотите удалить пользователя ${email}?`);
        setShowConfirmation(true)
    }

    const confirmDelete = () => {
        if (userIdToDelete) {
            axios.delete(`https://localhost:7069/api/AutoUser/${userIdToDelete}`)
                .then((res) => {
                    console.log("deleted^ ", res.data)
                    usingAxios()
                })
            setShowConfirmation(false)
            setUserIdToDelete(null)
        }
    }

    const [users, serUsers] = useState([]);
    const usingAxios = () => {
        axios.get("https://localhost:7069/api/AutoUser/GetUsers")
            .then((response) => {
            serUsers(response.data);
        });
    };

    useEffect(() => {
        usingAxios();
    }, []);

    return (
        <div className="container" style={{marginTop: '2rem'}}>
            <div className="mb-3">
                <Link to="/CreateUser" className="btn btn-primary">
                    Создать
                </Link>
            </div>
            <Row className="row-cols-4 gy-3">
                {users.map((user) => (
                    <Col key={user.id} className="h-100">
                        <Card className="h-100">
                            <Card.Body>
                                <Card.Text>
                                    <p><strong>{user.role}</strong></p>
                                    <p><strong>Email</strong></p>
                                    <p>{user.email}</p>
                                </Card.Text>
                            </Card.Body>
                            <Card.Footer className="d-flex justify-content-end p-2">
                                <Link to={`/ChangeUserRole/${user.id}`} className="btn btn-primary m-1">
                                    Изменить
                                </Link>
                                <button
                                    onClick={() => removeData(user.id, user.email)}
                                    className="btn btn-danger m-1"
                                >
                                    Удалить
                                </button>
                            </Card.Footer>
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

export default Users