export default class Business {
    name: string;
    ownerName: string;
    ownerSurname: string;
    ownerEmail: string;
    rut: string;

    constructor(
        name: string,
        ownerName: string,
        ownerSurname: string,
        ownerEmail: string,
        rut: string
    ) {
        this.name = name;
        this.ownerName = ownerName;
        this.ownerSurname = ownerSurname;
        this.ownerEmail = ownerEmail;
        this.rut = rut;
    }
}
