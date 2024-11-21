export default interface AuthResponse {
    userId: string;
    token: string;
    roles: Record<string, string[]>;
}
