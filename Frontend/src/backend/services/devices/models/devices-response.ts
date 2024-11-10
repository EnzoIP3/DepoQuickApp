import Pagination from "../../pagination";
import Device from "./device";

export default interface DevicesResponse {
    devices: Device[];
    pagination: Pagination;
}
