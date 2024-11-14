export default class Business {
    id: string;
    name: string;
    ownerName: string;
    ownerSurname: string;
    ownerEmail: string;
    rut: string;

    constructor(
        id: string,
        name: string,
        ownerName: string,
        ownerSurname: string,
        ownerEmail: string,
        rut: string
    ) {
        this.id = id;
        this.name = name;
        this.ownerName = ownerName;
        this.ownerSurname = ownerSurname;
        this.ownerEmail = ownerEmail;
        this.rut = rut;
    }
}
