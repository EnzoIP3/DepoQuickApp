export default interface UserLogged {
    userId: string;
    token: string;
    roles: Record<string, string[]>;
    currentRole: string;
}
