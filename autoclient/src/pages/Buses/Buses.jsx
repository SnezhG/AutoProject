import axios from "axios";
import { useEffect, useState } from "react";
import {Link} from "react-router-dom";
import {Card, CardImg, Col, Row} from "react-bootstrap";

function Buses() {
    
    const removeData = (id) => {
        let url = `https://localhost:7089/api/Buses/${id}`
        axios.delete(url).then(res =>{
            console.log("deleted^ ", res.data)
        })
    }
    
    const [buses, setBuses] = useState([]);
    const usingAxios = () => {
        axios.get("https://localhost:7089/api/Buses")
            .then((response) => {
            setBuses(response.data);
        });
    };

    useEffect(() => {
        usingAxios();
    }, []);

    return (
        <div className="container" style={{marginTop: '2rem'}}>
            <div className="mb-3">
                <Link to="/CreateBus" className="btn btn-primary">
                    Создать
                </Link>
            </div>
            <Row className="row-cols-4 gy-3">
                    {buses.map((bus) => (
                        <Col key={bus.busId}>
                           {/* <CardImg className="card-img-top" src="bus.png"></CardImg>*/}
                            <Card>
                                <Card.Body>
                                    <Card.Text>
                                        <p><strong>Модель:</strong> {bus.model}</p>
                                        <p><strong>Кол-во мест:</strong> {bus.seatCapacity}</p>
                                        <p><strong>Харакетристики:</strong> {bus.specs}</p>
                                    </Card.Text>
                                    <Link to={`/EditBus/${bus.busId}`} className="btn btn-primary">
                                        Изменить
                                    </Link>
                                    <button onClick={() =>
                                    removeData(bus.busId)} className="btn btn-danger">
                                        Удалить
                                    </button>
                                </Card.Body>
                            </Card>
                        </Col>
                    ))}
            </Row>
        </div>
    );
}
export default Buses;