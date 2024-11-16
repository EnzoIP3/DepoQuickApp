export default class Business {
    name: string;
    ownerName: string;
    ownerSurname: string;
    ownerEmail: string;
    rut: string;
    logo: string;

    constructor(
        name: string,
        ownerName: string,
        ownerSurname: string,
        ownerEmail: string,
        rut: string,
        logo: string
    ) {
        this.name = name;
        this.ownerName = ownerName;
        this.ownerSurname = ownerSurname;
        this.ownerEmail = ownerEmail;
        this.rut = rut;
        this.logo = logo;
    }
}
