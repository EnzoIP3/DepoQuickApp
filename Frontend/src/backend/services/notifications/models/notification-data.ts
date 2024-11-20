import Device from "../../devices/models/device";

export default interface NotificationData {
    event: string;
    device: Device;
    read: boolean;
    dateCreated: string;
}
