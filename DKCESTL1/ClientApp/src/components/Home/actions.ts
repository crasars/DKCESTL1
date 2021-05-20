import { City, Parcel, ParcelType } from "./types";

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
    const parcel: Parcel = { startCity: sCity, destination: eCity, type: pType, weight: weight, route: routeSelector, signed: recommendedParcel };
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
}
