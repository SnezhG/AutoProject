import { Link, useMatch, useResolvedPath } from "react-router-dom"
import {AuthProvider, useAuth} from "./pages/Auth/AuthContext.jsx"
export default function Navbar(){
    const { isUserLoggedIn } = useAuth(AuthProvider)
    const userRole = localStorage.getItem('role')
    return (
        <nav className="navbar navbar-expand" style={{backgroundColor: '#deb893'}}>
            <div className="container">
                <Link to="/" className="navbar-brand">
                    <strong>Five Tickets by Freddie</strong>
                </Link>
                <ul className="navbar-nav">
                    <li className="nav-item">
                        <Link to="/Buses" className="nav-link">Автобусы</Link>
                    </li>
                    <li className="nav-item">
                        <Link to="/Personnels" className="nav-link">Персонал</Link>
                    </li>
                    <li className="nav-item">
                        <Link to="/Busroutes" className="nav-link">Маршруты</Link>
                    </li>
                    {userRole === 'dispatcher' && (
                        <li className="nav-item">
                            <Link to="/CreateTrip" className="nav-link">Создать рейс</Link>
                        </li>
                    )}
                    {userRole === 'admin' && (
                        <li className="nav-item">
                            <Link to="/Users" className="nav-link">Пользователи</Link>
                        </li>
                    )}
                    {userRole === 'client' && (
                        <li className="nav-item">
                            <Link to="/ClientTickets" className="nav-link">Билеты</Link>
                        </li>
                    )}
                    <li className="nav-item">
                        {isUserLoggedIn ? (
                            <Link to="/Logout" className="nav-link">
                                Выйти
                            </Link>
                        ) : (
                            <Link to="/Auth/Login" className="nav-link">
                                Войти
                            </Link>
                        )}
                    </li>
                </ul>
            </div>
    </nav>
)
}