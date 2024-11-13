import PaginationResponse from "../../pagination-response";
import Device from "./device";

export default interface DevicesResponse {
    devices: Device[];
    pagination: PaginationResponse;
}
