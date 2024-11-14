import Pagination from "../../pagination";
import Device from "./device";

export default interface GetDevicesResponse {
    devices: Device[];
    pagination: Pagination;
}
