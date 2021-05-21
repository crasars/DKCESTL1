import React, { useState, useEffect } from 'react';
import { connect, useDispatch } from 'react-redux';
import { Form, Card, Row, InputGroup, Col, Button } from 'react-bootstrap';
import { Typeahead } from 'react-bootstrap-typeahead';
import { City, getCities, Parcel, parcelTypes } from './types';
import * as Actions from "./actions";

export default function Home() {

    const dispatch = useDispatch();
    const [routeSelector, setRouteSelector] = useState<boolean>(true);
    const [recommendedParcel, setRecommendedParcel] = useState<boolean>(false);
    const [toCity, setToCity] = useState<string>('');
    const [cityDestination, setCityDestination] = useState<string>('');
    const [parcelType, setParcelType] = useState<string>('');
    const [weight, setWeight] = useState<number>(0);
    const [result, setResult] = useState<string>('');

    useEffect(() => {
        console.log('toCity', toCity, 'city destination', cityDestination, 'parcelType', parcelType, 'weight', weight, 'cheapest route', routeSelector, 'recommended parcel', recommendedParcel);
    }, [routeSelector, recommendedParcel, toCity, cityDestination, parcelType, weight]);

    useEffect(() => {
        console.log("resulting path", result);
    }, [result]);

    const findPath = async () => {
        if (toCity != null && cityDestination != null) {
            let path = await Actions.findPath(toCity, cityDestination, parcelType, weight, recommendedParcel);
            console.log("path found", path);
            setResult(path);
        }
    }

    const getRouteText = (routeResult: any[]) => {
        const pathText = routeResult.splice(0, -2);
        console.log(pathText);
        return pathText;
    }

    console.log('all cities', getCities());

    return (
        <Form.Group as={Row}>
            <Card style={{ width: "30rem" }}>
                <Card.Title>Hej</Card.Title>
                <Form.Group as={Row}>
                    <Form.Label column sm="2">From</Form.Label>
                    <Typeahead
                        id="sender-country-dropdown"
                        labelKey={(city) => city.name}
                        placeholder="Choose a sender country"
                        options={getCities()}
                        onChange={e => {
                            setToCity(e[0].name);
                        }}
                    />
                </Form.Group>
                <Form.Group as={Row}>
                    <Form.Label column sm="2">Destination</Form.Label>
                    <Typeahead
                        id="destination-country-dropdown"
                        labelKey={(city) => city.name}
                        placeholder="Choose destination"
                        options={getCities()}
                        onChange={e => {
                            setCityDestination(e[0].name);
                        }}
                    />
                </Form.Group>
                <Form.Group as={Row} size="lg">
                    <Form.Label column sm="2">Type</Form.Label>
                    <Form.Control as="select" style={{ width: "200px" }} onChange={e => setParcelType(e.target.value)}>
                        {
                            parcelTypes.map((pType, index) => {
                                return <option key={index} value={pType.type}>{pType.type}</option>
                            })
                        }
                    </Form.Control>
                </Form.Group>
                <Form.Group as={Row} size="lg">
                    <Form.Label column sm="2">Weight</Form.Label>
                    <Form.Control as="input" style={{ width: "200px" }} type="number" min={0} onChange={e => setWeight(Number(e.target.value))}/>
                </Form.Group>
                <fieldset>
                    <Form.Group as={Row}>
                        <Form.Label as="legend" column sm="2">Route</Form.Label>
                        <Col sm={10}>
                            <Form.Check
                                type="radio"
                                label="Cheapest route"
                                name="selectRoute"
                                id="formCheckCheapestRoute"
                                onChange={() => setRouteSelector(true)}
                            />
                            <Form.Check
                                type="radio"
                                label="Fastest route"
                                name="selectRoute"
                                id="formCheckFastestRoute"
                                onChange={() => setRouteSelector(false)}
                            />
                        </Col>
                    </Form.Group>
                </fieldset>
                <Form.Group as={Row} controlId="formCheckRecommended">
                    <Col sm={{ span: 10, offset: 2 }}>
                        <Form.Check type="checkbox" label="Recommended shipping" onChange={() => {setRecommendedParcel(!recommendedParcel)}}/>
                    </Col>
                </Form.Group>
                <Form.Group as={Row}>
                    <Col sm={{ span: 10, offset: 2 }}>
                        <Button type="submit" onClick={() => Actions.sendParcel(toCity, cityDestination, parcelType, weight, routeSelector, recommendedParcel)}>Find route</Button>
                    </Col>
                </Form.Group>
                <Form.Group as={Row}>
                    <Col sm={{ span: 10, offset: 2 }}>
                        <Button type="submit" onClick={() => findPath()}>Find path</Button>
                    </Col>
                </Form.Group>
            </Card>
            <Card>{result}</Card>
        </Form.Group>
    );
}
