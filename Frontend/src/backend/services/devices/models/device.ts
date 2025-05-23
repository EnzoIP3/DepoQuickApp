export default interface Device {
    id: string;
    name: string;
    description: string;
    businessName: string;
    type: string;
    modelNumber: string;
    mainPhoto: string;
    secondaryPhotos: string[];
    roomId: string;
    hardwareId: string;
    state: string;
    isOpen: boolean;
}
