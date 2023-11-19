import {Link, useNavigate, useParams} from "react-router-dom";
import {useEffect, useState} from "react";
import axios from "axios";
import {Container} from "react-bootstrap";

function EditPersonnel(){
    const {id} = useParams();

    const [values, setValues] = useState({
        surname: '',
        name: '',
        patronimyc: '',
        post:'',
        experience:''
    })

    useEffect(() => {
        axios.get(`https://localhost:7089/api/Personnels/${id}`)
            .then(res => 
                setValues(res.data))
            .catch(err => console.log(err))
    }, []);

    const navigate = useNavigate();
    const handleUpdate = (event) =>{
        event.preventDefault();
        axios.put(`https://localhost:7089/api/Personnels/${id}`, values)
            .then(res => {
                console.log(res);
                navigate('/Personnels');
            })
            .catch(err => console.log(err))
    }

    return (
        <Container className="mt-5" style={{ width: '40%' }}>
            <h1 className="text-center">Редактировать данные о сотруднике</h1>
            <form onSubmit={handleUpdate} className="border p-4 rounded">
                <div className="mb-3">
                    <label htmlFor="surname" className="form-label">Фамилия</label>
                    <input
                        type="text"
                        name='surname'
                        className="form-control"
                        value={values.surname}
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
                        value={values.name}
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
                        value={values.patronimyc}
                        onChange={(e) =>
                            setValues({ ...values, patronimyc: e.target.value })}
                    />
                </div>
                <div className="mb-3">
                    <label htmlFor="post" className="form-label">Должность</label>
                    <select
                        name='post'
                        className="form-select"
                        value={values.post}
                        onChange={(e) =>
                            setValues({ ...values, post: e.target.value })}
                        required
                    >
                        <option value="" disabled>Должность</option>
                        <option value="Кондуктор">Кондуктор</option>
                        <option value="Водитель">Водитель</option>
                    </select>
                </div>
                <div className="mb-3">
                    <label htmlFor="experience" className="form-label">Стаж</label>
                    <input
                        type="text"
                        name='experience'
                        className="form-control"
                        value={values.experience}
                        onChange={(e) =>
                            setValues({ ...values, experience: e.target.value })}
                    />
                </div>
                <button type="submit" className="btn btn-primary">Изменить</button>
                <Link to="/Personnels" className="btn btn-secondary ms-2">Назад</Link>
            </form>
        </Container>
    )
}

export default EditPersonnel