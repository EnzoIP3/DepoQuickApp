export default interface Route {
    label: string;
    icon?: string;
    routerLink?: string[];
    command?: () => void;
    permission?: string;
    items?: Route[];
}
