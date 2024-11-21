export interface CreateCameraRequest {
    description?: string;
    mainPhoto?: string;
    modelNumber?: string;
    name: string;
    secondaryPhotos?: string[];
    motionDetection?: boolean;
    personDetection?: boolean;
    exterior?: boolean;
    interior?: boolean;
}
