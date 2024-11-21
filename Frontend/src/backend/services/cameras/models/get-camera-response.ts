export interface GetCameraResponse {
    id: string;
    name: string;
    description: string;
    modelNumber: string;
    mainPhoto: string;
    secondaryPhotos: string[];
    motionDetection: boolean;
    personDetection: boolean;
    exterior: boolean;
    interior: boolean;
}
