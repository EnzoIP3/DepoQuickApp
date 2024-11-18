export default interface Device {
    id: string;
    name: string;
    businessName: string;
    type: string;
    modelNumber: number;
    mainPhoto: string;
    secondaryPhotos: string[];
    roomId: string;
    hardwareId: string;
    state: string;
    isOpen: boolean;
}