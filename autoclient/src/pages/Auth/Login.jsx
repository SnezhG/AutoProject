import {useState} from "react";
import {useNavigate} from "react-router-dom";
import axios from "axios";

function Login(){
    const [values, setValues] = useState({
        email: '',
        password: '',
        confirmPassword: ''
    })

    const navigate = useNavigate();
    const handleLogin = (event) =>{
        event.preventDefault();
        axios.post('https://localhost:7069/api/Auth/Login', values)
            .then(res => {
                console.log(res);
            })
            .catch(err => console.log(err));
    }
    return (
        <div>
            <h1>Sign In</h1>
            <form onSubmit={handleLogin}>
                <div>
                    <label htmlFor="email">Email</label>
                    <input type="text" name="email"
                           onChange={e =>
                               setValues({...values, email: e.target.value})}/>
                </div>
                <div>
                    <label htmlFor="password">Password</label>
                    <input type="text" name="password"
                           onChange={e =>
                               setValues({...values, password: e.target.value})}/>
                </div>
                <div>
                    <label htmlFor="confirmPassword">Confirm password</label>
                    <input type="text" name="confirmPassword"
                           onChange={e =>
                               setValues({...values, confirmPassword: e.target.value})}/>
                </div>
                <button>Submit</button>
            </form>
        </div>
    )
}

export default Login