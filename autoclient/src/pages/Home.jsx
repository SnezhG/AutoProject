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
            <h1>Search for a trip</h1>
            <form onSubmit={handleSubmit}>
                <div>
                    <label htmlFor="depCity">Departure city:</label>
                    <input
                        type="text"
                        name="depCity"
                        onChange={(e) => 
                            setValues({ ...values, depCity: e.target.value })}
                    />
                </div>
                <div>
                    <label htmlFor="arrCity">Arrival city:</label>
                    <input
                        type="text"
                        name="arrCity"
                        onChange={(e) => 
                            setValues({ ...values, arrCity: e.target.value })}
                    />
                </div>
                <div>
                    <label htmlFor="depDate">Departure time:</label>
                    <input
                        type="text"
                        name="depDate"
                        onChange={(e) => 
                            setValues({ ...values, depDate: e.target.value })}
                    />
                </div>
                <button>Find</button>
            </form>

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