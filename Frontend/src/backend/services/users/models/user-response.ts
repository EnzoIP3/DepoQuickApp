import Pagination from "../../pagination";
import User from "./user";

export default interface GetUsersResponse{
    users : User[];
    pagination : Pagination;
}