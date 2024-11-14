import Pagination from "../../pagination";

export default interface GetDevicesRequest extends Pagination {
    name?: string;
    type?: string;
    model?: number;
    businessName?: string;
}
