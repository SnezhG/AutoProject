import { Link, useMatch, useResolvedPath } from "react-router-dom"
export default function Navbar(){
    return (
        <nav className="nav">
        <Link to="/" className="site-title">
            Bus Tickets
        </Link>
        <ul>
            <li>
                <CustomLink to="/Buses">Buses</CustomLink>
            </li>
            <li>
                <CustomLink to="/Personnels">Personnels</CustomLink>
            </li>
            <li>
                <CustomLink to="/Account">Account</CustomLink>
            </li>
        </ul>
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