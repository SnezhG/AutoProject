import { Link, useMatch, useResolvedPath } from "react-router-dom"
export default function Navbar(){
    return (
        <nav className="navbar navbar-expand navbar-dark bg-dark">
            <div className="container">
                <Link to="/" className="navbar-brand">
                    Five Tickets at Freddy
                </Link>
                <ul className="navbar-nav">
                    <li className="nav-item">
                        <Link to="/Buses" className="nav-link">Автобусы</Link>
                    </li>
                    <li className="nav-item">
                        <Link to="/Personnels" className="nav-link">Персонал</Link>
                    </li>
                    <li className="nav-item">
                        <Link to="/Account" className="nav-link">Аккаунт</Link>
                    </li>
                </ul>
            </div>
    </nav>
)
}

function CustomLink({to, children, ...props}){
    const resolvedPath = useResolvedPath(to)
    const isActive = useMatch({path: resolvedPath.pathname, end: true})
    return(
        <li className={isActive ? "active" : ""}>
            <Link to={to} {...props}>
                {children}
            </Link>
        </li>
    )
}