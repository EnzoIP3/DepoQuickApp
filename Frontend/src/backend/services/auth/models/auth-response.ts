export default interface AuthResponse {
    userId: string;
    token: string;
    permissions: string[];
}
