﻿import axios from "axios";
import { useEffect, useState } from "react";

function Personnels() {
    const [personnels, setPersonnels] = useState([]);
    const usingAxios = () => {
        axios.get("https://localhost:7089/api/Personnels").then((response) => {
            setBuses(response.data);
        });
    };

    useEffect(() => {
        usingAxios();
    }, []);

    return (
        <div className="FetchData">
            <table style={{ width: "100%" }}>
                <thead>
                <tr>
                    <th>Id</th>
                    <th>Surname</th>
                    <th>Name</th>
                    <th>Patr</th>
                    <th>Post</th>
                    <th>Experience</th>
                </tr>
                </thead>
                <tbody>
                {personnels?.map((pers) => (
                    <tr key={pers.personnelId}>
                        <td>{pers.personnelId}</td>
                        <td>{pers.surname}</td>
                        <td>{pers.name}</td>
                        <td>{pers.patronimyc}</td>
                        <td>{pers.experience}</td>
                    </tr>
                ))}
                </tbody>
            </table>
        </div>
    );
}

export default Personnels;