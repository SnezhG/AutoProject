import {useState} from "react";
import axios from "axios";

function Home(){
    const [values, setValues] = useState({
        DepCity: '',
        ArrCity: '',
        DepDate: ''
    })

    const [searchResults, setSearchResults] = useState([])
    const handleSubmit = (event) =>{
        event.preventDefault();
        axios.get('https://localhost:7089/api/Trips/FindTrips', values)
            .then(res => {
                console.log(res);
                setSearchResults(res.data);
            })
            .catch(err => console.log(err));
    }


    return (
        <div>
            <h1>Search for a trip</h1>
            <form onSubmit={handleSubmit}>
                <div>
                    <label htmlFor="DepCity">Departure city:</label>
                    <input
                        type="text"
                        name="depCity"
                        onChange={(e) => setValues({ ...values, DepCity: e.target.value })}
                    />
                </div>
                <div>
                    <label htmlFor="ArrCity">Arrival city:</label>
                    <input
                        type="text"
                        name="arrCity"
                        onChange={(e) => setValues({ ...values, ArrCity: e.target.value })}
                    />
                </div>
                <div>
                    <label htmlFor="DepDate">Departure time:</label>
                    <input
                        type="text"
                        name="depDate"
                        onChange={(e) => setValues({ ...values, DepCity: e.target.value })}
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