export default interface Member {
    id: string;
    name: string;
    surname: string;
    photo: string;
    canAddDevices: boolean;
    canListDevices: boolean;
    shouldBeNotified: boolean;
}
