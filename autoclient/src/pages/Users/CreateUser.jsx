import {useEffect, useState} from "react";
import {Link, useNavigate} from "react-router-dom";
import axios from "axios";
import {Container} from "react-bootstrap";

function CreateUser(){
    const [values, setValues] = useState({
        email: '',
        password: '',
        confirmPassword: '',
        userRole: ''
    })

    const navigate = useNavigate();

    const [roleValues, setRoleValues] = useState([])
    useEffect(() => {
        axios.get(`https://localhost:5275/api/AutoUser/GetRoles`, {
            headers:{
                Authorization: `Bearer ${localStorage.getItem('token')}`
            }
        })
            .then(res =>{
                setRoleValues(res.data)
            })
            .catch(err => console.log(err))
    }, []);
    const handleSubmit = (event) =>{
        event.preventDefault();
        axios.post('https://localhost:5275/api/AutoUser/CreateUser', values, {
            headers:{
                Authorization: `Bearer ${localStorage.getItem('token')}`
            }
        })
            .then(res => {
                console.log(res);
                navigate('/Users')
            })
            .catch(err => console.log(err));
    }

    return (
        <Container className="mt-5" style={{width: '40%'}}>
            <h2 className="text-center">Добавить нового пользователя</h2>
            <form onSubmit={handleSubmit} className="border p-4 rounded" style={{backgroundColor:'white'}}>
                <div className="mb-3">
                    <label htmlFor="email" className="form-label">Email</label>
                    <input
                        type="email"
                        name='email'
                        className="form-control"
                        placeholder="Email"
                        required
                        onChange={(e) =>
                            setValues({ ...values, email: e.target.value })}
                    />
                </div>
                <div className="mb-3">
                    <label htmlFor="password" className="form-label">Пароль</label>
                    <input
                        type="password"
                        name='password'
                        className="form-control"
                        placeholder="Пароль"
                        required
                        onChange={(e) =>
                            setValues({ ...values, password: e.target.value })}
                    />
                </div>
                <div className="mb-3">
                    <label htmlFor="confirmPassword" className="form-label">Подтвердите пароль</label>
                    <input
                        type="password"
                        name='confirmPassword'
                        className="form-control"
                        onChange={(e) =>
                            setValues({ ...values, confirmPassword: e.target.value })}
                    />
                </div>
                <div className="mb-3">
                    <label htmlFor="userRole" className="form-label">Роль</label>
                    <select 
                        type="text" 
                        name='userRole'
                        className="form-select"
                        value={values.userRole}
                        onChange={e =>
                                setValues({...values, userRole: e.target.value})}>
                        <option value="" disabled>Роль</option>
                        {roleValues
                            .map(role => (
                                <option key={role.id} value={role.name}>
                                    {role.name}
                                </option>
                            ))}
                    </select>
                </div>
                <button type="submit" className="btn btn-primary" style={{backgroundColor:'#7e5539', borderColor:'#7e5539'}}>Добавить</button>
                <Link to="/Users" className="btn btn-secondary ms-2">Назад</Link>
            </form>
        </Container>
    )
}

export default CreateUser