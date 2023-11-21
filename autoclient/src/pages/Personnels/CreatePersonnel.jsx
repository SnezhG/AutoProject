import {useState} from "react";
import {Link, useNavigate} from "react-router-dom";
import axios from "axios";
import {Container} from "react-bootstrap";

function CreatePersonnel(){
    const [values, setValues] = useState({
        surname: '',
        name: '',
        patronimyc: '',
        post:'',
        experience:''
    })

    const navigate = useNavigate();
    const handleSubmit = (event) =>{
        event.preventDefault();
        axios.post('https://localhost:5275/api/Personnels', values, {
            headers:{
                Authorization: `Bearer ${localStorage.getItem('token')}`
            }
        })
            .then(res => {
                console.log(res);
                navigate('/Personnels')
            })
            .catch(err => console.log(err));
    }

    return (
        <Container className="mt-5" style={{width: '40%'}}>
            <h2 className="text-center">Добавить нового сотрудника</h2>
            <form onSubmit={handleSubmit} className="border p-4 rounded" style={{backgroundColor:'white'}}>
                <div className="mb-3">
                    <label htmlFor="surname" className="form-label">Фамилия</label>
                    <input
                        type="text"
                        name='surname'
                        className="form-control"
                        placeholder="Введите фамилию"
                        required
                        onChange={(e) =>
                            setValues({ ...values, surname: e.target.value })}
                    />
                </div>
                <div className="mb-3">
                    <label htmlFor="name" className="form-label">Имя</label>
                    <input
                        type="text"
                        name='name'
                        className="form-control"
                        placeholder="Введите имя"
                        required
                        onChange={(e) =>
                            setValues({ ...values, name: e.target.value })}
                    />
                </div>
                <div className="mb-3">
                    <label htmlFor="patronimyc" className="form-label">Отчество</label>
                    <input
                        type="text"
                        name='patronimyc'
                        className="form-control"
                        placeholder="Введите отчество"
                        onChange={(e) =>
                            setValues({ ...values, patronimyc: e.target.value })}
                    />
                </div>
                <div className="mb-3">
                    <label htmlFor="post" className="form-label">Должность</label>
                    <select
                        name='post'
                        className="form-select"
                        onChange={(e) => {
                            setValues({ ...values, post: e.target.value });
                        }}
                        required
                    >
                        <option value="" disabled>Должность</option>
                        <option value="conductor">Кондуктор</option>
                        <option value="driver">Водитель</option>
                    </select>
                </div>
                <div className="mb-3">
                    <label htmlFor="experience" className="form-label">Стаж</label>
                    <input
                        type="text"
                        name='experience'
                        className="form-control"
                        placeholder="Введите стаж"
                        onChange={(e) =>
                            setValues({ ...values, experience: e.target.value })}
                    />
                </div>
                <button type="submit" className="btn btn-primary" style={{backgroundColor:'#7e5539', borderColor:'#7e5539'}}>Добавить</button>
                <Link to="/Personnels" className="btn btn-secondary ms-2">Назад</Link>
            </form>
        </Container>
    )
}

export default CreatePersonnel