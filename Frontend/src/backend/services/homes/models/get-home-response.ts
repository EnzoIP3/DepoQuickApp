import Home from "./home";

export default interface GetHomeResponse extends Home {
    owner: {
        id: string;
        name: string;
        surname: string;
    };
}
