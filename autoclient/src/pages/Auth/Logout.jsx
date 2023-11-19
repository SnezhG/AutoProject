import {useNavigate} from "react-router-dom";
import {useEffect, useState} from "react";
import {Container} from "react-bootstrap";
import {useAuth} from "./AuthContext.jsx";

function Logout(){
    const navigate = useNavigate();
    const { logout } = useAuth();
    const handleLogout = (event) => {
        event.preventDefault()
        localStorage.removeItem("token")
        localStorage.removeItem("role")
        localStorage.setItem('isUserLoggedIn', 'false')
        logout()
        navigate("/")
    };

    return (
        <Container className="mt-5" style={{width: '50%'}}>
            <div className="alert alert-warning" role="alert">
                <h4 className="alert-heading">Вы уверены, что хотите выйти?</h4>
                <p>
                    При выходе из аккаунта вы будете перенаправлены на главную страницу.
                </p>
                <hr />
                <div className="d-flex justify-content-between">
                    <button className="btn btn-danger" onClick={handleLogout}>
                        Выйти
                    </button>
                    <button className="btn btn-primary" onClick={() => navigate("/")}>
                        Вернуться на главную
                    </button>
                </div>
            </div>
        </Container>
    )
}

export default Logout