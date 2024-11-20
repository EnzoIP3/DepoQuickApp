import Pagination from "../../pagination";
import Business from "./business";

export default interface GetBusinessesResponse {
    businesses: Business[];
    pagination: Pagination;
}
