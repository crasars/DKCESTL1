import { City, Parcel, ParcelType } from "./types";
import axios from "axios";

export function sendParcel(
    fromCity: string,
    toCity: string,
    parcelType: string,
    weight: number,
    routeSelector: boolean,
    recommendedParcel: boolean
) {
    const sCity: City = { name: fromCity };
    const eCity: City = { name: toCity };
    const pType: ParcelType = { type: parcelType };
    const parcel: Parcel = { startCity: sCity, endCity: eCity, parcelType: pType, weight: weight, route: routeSelector, signed: recommendedParcel };
    const data = new FormData();
    data.append("parcel", JSON.stringify(parcel));
    console.log("formData", data.getAll("parcel"));

    axios.post("SendParcel/sendParcel", data);

    /*
    axios({
        method: "POST",
        url: "SendParcel/sendParcel",
        data: JSON.stringify(parcel)
    });

    
    fetch("SendParcel/sendParcel",
        {
            method: "POST",
            body: data
        }).then(response => response.text()).then(data => this.setState({ text: data, loading: false }));
    console.log("sending parcel", parcel);
    const data = new FormData();
    data.append("City", fromCity);
    data.append("City", toCity);
    data.append("City", parcelType);
    data.append("City", weight.toString());
    data.append("City", routeSelector.toString());
    data.append("City", recommendedParcel.toString());
    const xhr = new XMLHttpRequest();
    xhr.open("post", "/sendParcel", true);
    xhr.send(data);
    */
}

export const findPath = async(
    fromCity: string,
    toCity: string,
    parceltype: string,
    weight: number,
    recommended: boolean) => {
    console.log('finding path');
    const { status, data } =
        await axios.get("modelutils/calculate/" + parceltype + "/" + weight + "/" + fromCity + "/" + toCity + "/" + recommended);
    console.log("found path", data, status);

    return data;
    // let data = await fetch("modelutils/calculate/" + parceltype + "/" + weight + "/" + fromCity + "/" + toCity);
    // let json = await data.json();
}