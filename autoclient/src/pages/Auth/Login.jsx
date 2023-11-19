import {useState} from "react";
import {Link, useNavigate} from "react-router-dom";
import axios from "axios";
import {Container} from "react-bootstrap";
import {useAuth} from "./AuthContext.jsx";

function Login(){
    const [values, setValues] = useState({
        email: '',
        password: ''
    })

    const navigate = useNavigate();
    const { login } = useAuth();
    const handleLogin = (event) =>{
        event.preventDefault();
        axios.post('https://localhost:7069/api/Auth/Login', values)
            .then(res => {
                console.log(res)
                localStorage.setItem('isUserLoggedIn', 'true')
                localStorage.setItem("token", res.data.token)
                localStorage.setItem("role", res.data.role)
                login()
                navigate('/')
            })
            .catch(err => console.log(err));
    }
    return (
        <Container className="mt-5" style={{width: '50%'}}>
            <h1 className="text-center">Вход</h1>
            <form onSubmit={handleLogin} className="border p-4 rounded">
                <div className="form-group">
                    <label htmlFor="email">Email</label>
                    <input
                        type="text"
                        name="email"
                        className="form-control"
                        onChange={(e) =>
                            setValues({ ...values, email: e.target.value })
                        }
                    />
                </div>
                <div className="form-group">
                    <label htmlFor="password">Пароль</label>
                    <input
                        type="password"
                        name="password"
                        className="form-control"
                        onChange={(e) =>
                            setValues({ ...values, password: e.target.value })
                        }
                    />
                </div>
                <button type="submit" className="btn btn-primary mt-2">
                    Войти
                </button>
                <p>Нет аккаунта? <Link to={'/Auth/Registration'}>Зарегистрироваться</Link></p>
            </form>
        </Container>
    )
}

export default Login