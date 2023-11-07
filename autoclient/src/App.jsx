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
import { Route, Routes } from "react-router-dom"

function App() {
    return (
        <>
        <Navbar />
        <div className="container">
            <Routes>
                <Route path="/" element={<Home />}/>
                <Route path="/Buses" element={<Buses />}/>
                <Route path="/Personnels" element={<Personnels />}/>
                <Route path="/Account" element={<Account />}/>
                <Route path="/CreateBus" element={<CreateBus />}/>
                <Route path="/EditBus/:id" element={<EditBus />}/>
            </Routes>
        </div>
        </>
    )
}

export default App