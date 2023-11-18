import React from 'react'
import 'bootstrap/dist/css/bootstrap.min.css';
import FetchData from './FetchData'
import Navbar from "@/Navbar.jsx";
import "./styles.css"
import Home from "./pages/Home"
import Buses from "./pages/Buses/Buses.jsx"
import CreateBus from "./pages/Buses/CreateBus"
import EditBus from "./pages/Buses/EditBus"
import Busroutes from "./pages/Busroutes/Busroutes.jsx"
import CreateBusroute from "./pages/Busroutes/CreateBusroute"
import EditBusroute from "./pages/Busroutes/EditBusroute"
import Personnels from "./pages/Personnels/Personnels.jsx"
import CreatePersonnel from "./pages/Personnels/CreatePersonnel"
import EditPersonnel from "./pages/Personnels/EditPersonnel"
import Trips from "./pages/Trip/Trips"
import CreateTrip from "./pages/Trip/CreateTrip"
import EditTrip from "./pages/Trip/EditTrip"
import IssueTripTicket from "./pages/Ticket/IssueTripTicket"
import TicketPay from "./pages/Ticket/TicketPay"
import Ticket from "./pages/Ticket/Ticket.jsx"
import ClientTickets from "./pages/Ticket/ClientTickets"
import Users from "./pages/Users/Users.jsx"
import CreateUser from "./pages/Users/CreateUser"
import ChangeUserRole from "./pages/Users/ChangeUserRole"
import Login from "./pages/Auth/Login"
import Registration from "./pages/Auth/Registration"
import Account from "./pages/Account"
import { Route, Routes } from "react-router-dom"

function App() {
    return (
        <>
        <Navbar />
        <div className="container">
            <Routes>
                <Route path="/" element={<Home />}/>
                <Route path="/Buses" element={<Buses />}/>
                <Route path="/CreateBus" element={<CreateBus />}/>
                <Route path="/EditBus/:id" element={<EditBus />}/>
                <Route path="/Personnels" element={<Personnels />}/>
                <Route path="/CreatePersonnel" element={<CreatePersonnel />}/>
                <Route path="/EditPersonnel/:id" element={<EditPersonnel />}/>
                <Route path="/Busroutes" element={<Busroutes />}/>
                <Route path="/CreateBusroute" element={<CreateBusroute />}/>
                <Route path="/EditBusroute/:id" element={<EditBusroute />}/>
                <Route path="/Trips" element={<Trips />}/>
                <Route path="/CreateTrip" element={<CreateTrip />}/>
                <Route path="/EditTrip/:id" element={<EditTrip />}/>
                <Route path="/Users" element={<Users />}/>
                <Route path="/ChangeUserRole/:id" element={<ChangeUserRole />}/>
                <Route path="/CreateUser" element={<CreateUser />}/>
                <Route path="/Auth/Login" element={<Login />}/>
                <Route path="/Auth/Registration" element={<Registration />}/>
                <Route path="/TicketPay/:id" element={<TicketPay />}/>
                <Route path="/Ticket/:id" element={<Ticket />}/>
                <Route path="/IssueTripTicket/:id" element={<IssueTripTicket />}/>
                <Route path="/Account" element={<Account />}/>
            </Routes>
        </div>
        </>
    )
}

export default App