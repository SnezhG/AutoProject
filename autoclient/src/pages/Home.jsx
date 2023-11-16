import {useState} from "react";
import axios from "axios";
import {useNavigate} from "react-router-dom";

function Home(){
    const [values, setValues] = useState({
        arrCity: '',
        depCity: '',
        depDate: ''
    })

    const [searchResults, setSearchResults] = useState([])
    const handleSubmit = (event) =>{
        event.preventDefault();
        axios.post('https://localhost:7089/api/Trips/FindTrips', values)
            .then(res => {
                console.log(res);
                setSearchResults(res.data);
            })
            .catch(err => console.log(err));
    }

    const navigator = useNavigate()
    const buyTicket = (id) => {
        navigator(`/IssueTripTicket/${id}`)
    }

    return (
        <div>
            <div className="findTickets">
            <h1>Билеты на автобус</h1>
            <form onSubmit={handleSubmit}>
                <div>
                    <input
                        type="text"
                        name="depCity"
                        placeholder="Откуда"
                        onChange={(e) =>
                            setValues({ ...values, depCity: e.target.value })}
                    />
                    <input
                        type="text"
                        name="arrCity"
                        placeholder="Куда"
                        onChange={(e) =>
                            setValues({ ...values, arrCity: e.target.value })}
                    />
                    <input
                        type="text"
                        name="depDate"
                        placeholder="Когда"
                        onChange={(e) =>
                            setValues({ ...values, depDate: e.target.value })}
                    />
                </div>
                <button>Найти</button>
            </form>
            </div>

            <div>
                <h1>Found trips</h1>
                {searchResults.length > 0 ? (
                    <ul>
                        {searchResults.map((trip) => (
                            <li key={trip.tripId}>
                                <p>TripId: {trip.tripId}</p>
                                <button onClick={() => buyTicket(trip.tripId)}>Buy Ticket</button>
                            </li>
                        ))}
                    </ul>
                ) : (
                    <h1>No results were found</h1>
                )}
            </div>
        </div>
    );
}

export default Home