import axios from "axios";
import {useEffect, useState} from "react";
import {Link} from "react-router-dom";

function Trips(){
    const [trips, setTrip] = useState([]);
    const usingAxios = () => {
        axios.get("https://localhost:7089/api/Trips")
            .then((response) => {
            setTrip(response.data);
        });
    };

    useEffect(() => {
        usingAxios();
    }, []);

    return (
        <div>
            <div>
                <Link to="/CreateBus">Create</Link>
            </div>
            <table className="dataTable">
                <thead>
                <tr>
                    <th>Id</th>
                    <th>Seats</th>
                    <th>Model</th>
                    <th>Specs</th>
                    <th>Options</th>
                </tr>
                </thead>
                <tbody>
                {buses?.map((bus) => (
                    <tr key={bus.busId}>
                        <td>{bus.busId}</td>
                        <td>{bus.seatCapacity}</td>
                        <td>{bus.model}</td>
                        <td>{bus.specs}</td>
                        <td>
                            <Link to={`/EditBus/${bus.busId}`}>Edit</Link>
                            <button onClick={() => removeData(bus.busId)}>Delete</button>
                        </td>
                    </tr>
                ))}
                </tbody>
            </table>
        </div>
    );
}

export default Trips