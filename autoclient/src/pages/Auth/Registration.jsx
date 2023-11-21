import {useState} from "react";
import {Link, useNavigate} from "react-router-dom";
import axios from "axios";
import {Container} from "react-bootstrap";

function Registration(){
const [values, setValues] = useState({
    email: '',
    password: '',
    confirmPassword: ''
})

const navigate = useNavigate();
const handleRegistration = (event) =>{
    event.preventDefault();
    axios.post('https://localhost:5275/api/Auth/Registration', values)
        .then(res => {
            console.log(res)
            navigate('/Auth/Login')
        })
        .catch(err => console.log(err));
}
return (
    <Container className="mt-5" style={{width: '33%'}}>
        <h1 className="text-center">Регистрация</h1>
        <form onSubmit={handleRegistration} className="border p-4 rounded" style={{backgroundColor:'white'}}>
            <div className="form-group">
                <label htmlFor="email">Email</label>
                <input
                    type="text"
                    name="email"
                    required
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
                    required
                    className="form-control"
                    onChange={(e) =>
                        setValues({ ...values, password: e.target.value })
                    }
                />
            </div>
            <div className="form-group">
                <label htmlFor="confirmPassword">Подтвердите пароль</label>
                <input
                    type="password"
                    required
                    name="confirmPassword"
                    className="form-control"
                    onChange={(e) =>
                        setValues({ ...values, confirmPassword: e.target.value })
                    }
                />
            </div>
            <button type="submit" style={{backgroundColor:'#7e5539', borderColor:'#7e5539'}} className="btn btn-primary mt-2">
                Регистрация
            </button>
        </form>
    </Container>
    )
}

export default Registration