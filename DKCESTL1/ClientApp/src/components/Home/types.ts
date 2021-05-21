export interface City {
    name: string;
}

export interface ParcelType {
    type: string;
}

export interface Parcel {
    startCity: City;
    endCity: City;
    parcelType: ParcelType;
    weight: number;
    route: boolean;
    signed: boolean;
}

export const parcelTypes: ParcelType[] = [
    { type: 'Weapons' },
    { type: 'Live animals' },
    { type: 'Cautios parcels' },
    { type: 'Refrigerated goods' },
    { type: 'Standard' },

]

export const getCities = () => {
    const cities: City[] = [
        { name: 'Tanger' },
        { name: 'Tunis' },
        { name: 'Tripoli' },
        { name: 'Marrakesh' },
        { name: 'Sahara' },
        { name: 'De kanariske øer' },
        { name: 'Dakar' },
        { name: 'Sierra Leone' },
        { name: 'Guldkysten' },
    ]
    return cities;
}