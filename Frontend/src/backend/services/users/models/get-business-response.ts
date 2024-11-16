import Pagination from "../../pagination";
import { ListBusinessInfo } from "./list-business-info";

export interface GetBusinessResponse {
    businesses: ListBusinessInfo[];
    pagination: Pagination;
}