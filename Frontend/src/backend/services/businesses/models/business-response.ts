import Pagination from "../../pagination";
import Business from "./Business";

export default interface GetBusinessesResponse{
    businesses : Business[];
    pagination : Pagination;
}