import axios from "axios";
import { useEffect, useState } from "react";
/*import "./App.css";*/

function FetchData() {
    const [busroutes, setBusroutes] = useState([]);
/*    const usingAxios = () => {
        axios.get("https://localhost:7089/api/Busroutes").then((response) => {
            setBusroutes(response.data);
            console.log(response.data);
        });
    };*/

    useEffect(() => {
        usingAxios();
    }, []);

    return (
        <div className="FetchData">
            <table style={{ width: "100%" }}>
                <thead>
                <tr>
                    <th>Id</th>
                    <th>DepCity</th>
                    <th>ArrCity</th>
                </tr>
                </thead>
                <tbody>
                {busroutes?.map((busroute) => (
                    <tr key={busroute.routeId}>
                        <td>{busroute.routeId}</td>
                        <td>{busroute.depCity}</td>
                        <td>{busroute.arrCity}</td>
                    </tr>
                ))}
                </tbody>
            </table>
        </div>
    );
}

export default FetchData;