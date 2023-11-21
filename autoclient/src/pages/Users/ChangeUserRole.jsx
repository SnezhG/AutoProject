import {Link, useNavigate, useParams} from "react-router-dom";
import {useEffect, useState} from "react";
import axios from "axios";
import {Container} from "react-bootstrap";

function ChangeUserRole(){
    const {id} = useParams();

    const [values, setValues] = useState({
            userEmail: '',
            allRoles: [],
            userRole: ''
        }
    )

    useEffect(() => {
        axios.get(`https://localhost:5275/api/AutoUser/GetUser/${id}`, {
            headers:{
                Authorization: `Bearer ${localStorage.getItem('token')}`
            }
        })
            .then(res => setValues(res.data))
            .catch(err => console.log(err))
    }, []);

    const navigate = useNavigate();
    const handleChangeRole = (event) =>{
        event.preventDefault();
        axios.post(`https://localhost:5275/api/AutoUser/ChangeUserRole`, values, {
            headers:{
                Authorization: `Bearer ${localStorage.getItem('token')}`
            }
        })
            .then(res => {
                console.log(res);
                navigate('/Users');
            })
            .catch(err => console.log(err))
    }

    return (
        <Container className="mt-5" style={{ width: '40%' }}>
            <h1 className="text-center">Изменить роль пользователя</h1>
            <form onSubmit={handleChangeRole} className="border p-4 rounded" style={{backgroundColor:'white'}}>
                <div className="mb-3">
                    <label htmlFor="userEmail" className="form-label">Пользователь</label>
                    <input
                        type="text"
                        name='userEmail'
                        className="form-control"
                        value={values.userEmail}
                        readOnly
                        onChange={(e) =>
                            setValues({ ...values, userEmail: e.target.value })}
                    />
                </div>
                <div className="mb-3">
                    <label htmlFor="userRole" className="form-label">Роль</label>
                    <select 
                        name='userRole'
                        className="form-select"
                        value={values.userRole}
                        onChange={e =>
                                setValues({...values, userRole: e.target.value})}>
                        <option value="" disabled>Роль</option>
                        {values.allRoles
                            .map(role => (
                                <option key={role.id} value={role.name}>
                                    {role.name}
                                </option>
                            ))}
                    </select>
                </div>
                <button type="submit" className="btn btn-primary" style={{backgroundColor:'#7e5539', borderColor:'#7e5539'}}>Изменить</button>
                <Link to="/Users" className="btn btn-secondary ms-2">Назад</Link>
            </form>
        </Container>
    )
}

export default ChangeUserRole
