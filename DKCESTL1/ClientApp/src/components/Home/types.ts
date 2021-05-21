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
        { name: 'Cairo' },
        { name: 'Marrakesh' },
        { name: 'Sahara' },
        { name: 'Omdurman' },
        { name: 'Suaken' },
        { name: 'Dakar' },
        { name: 'Timbuktu' },
        { name: 'Wadai' },
        { name: 'Addis Abeba' },
        { name: 'Kap Guardafui' },
        { name: 'Sierra Leone' },
        { name: 'Guld Kysten' },
        { name: 'Slave-Kysten' },
        { name: 'Darfur' },
        { name: 'Bahr El Ghazal' },
        { name: 'Victoria Søen' },
        { name: 'Congo' },
        { name: 'Kabalo' },
        { name: 'Zanzibar' },
        { name: 'Luanda' },
        { name: 'Mocambique' },
        { name: 'Victoria Faldene' },
        { name: 'Hvalbugten' },
        { name: 'Dragebjerget' },
        { name: 'Kapstaden' },
        { name: 'St Helena' },
        { name: 'Kap St Marie' },
        { name: 'Amatave' },
        { name: 'De Kanariske Øer' }
    ];
    return cities;
}