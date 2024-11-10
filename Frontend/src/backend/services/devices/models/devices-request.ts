import Pagination from "../../pagination";

export default interface DevicesRequest extends Pagination {
    name?: string;
    type?: string;
    model?: number;
    businessName?: string;
}
