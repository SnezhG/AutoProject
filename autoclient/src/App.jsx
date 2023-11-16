import React from 'react'
import FetchData from './FetchData'
import Navbar from "@/Navbar.jsx";
import "./styles.css"
import Home from "./pages/Home"
import Buses from "./pages/Buses"
import Personnels from "./pages/Personnels"
import Account from "./pages/Account"
import CreateBus from "./pages/Buses/CreateBus"
import EditBus from "./pages/Buses/EditBus"
import IssueTripTicket from "./pages/IssueTripTicket"
import Login from "./pages/Auth/Login"
import TicketPay from "./pages/TicketPay"
import Users from "./pages/Users.jsx"
import CreateUser from "./pages/Users/CreateUser"
import ChangeUserRole from "./pages/Users/ChangeUserRole"
import { Route, Routes } from "react-router-dom"

function App() {
    return (
        <>
        <Navbar />
        <div className="container">
            <Routes>
                <Route path="/" element={<Home />}/>
                <Route path="/Buses" element={<Buses />}/>
                <Route path="/Users" element={<Users />}/>
                <Route path="/Personnels" element={<Personnels />}/>
                <Route path="/Account" element={<Account />}/>
                <Route path="/CreateBus" element={<CreateBus />}/>
                <Route path="/CreateUser" element={<CreateUser />}/>
                <Route path="/Auth/Login" element={<Login />}/>
                <Route path="/EditBus/:id" element={<EditBus />}/>
                <Route path="/TicketPay/:id" element={<TicketPay />}/>
                <Route path="/IssueTripTicket/:id" element={<IssueTripTicket />}/>
                <Route path="/ChangeUserRole/:id" element={<ChangeUserRole />}/>
            </Routes>
        </div>
        </>
    )
}

export default App