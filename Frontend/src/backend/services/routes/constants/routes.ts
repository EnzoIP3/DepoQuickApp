import Route from "../../../../components/sidebar/models/route";
import { Permissions } from "./permissions";

export const AuthenticatedRoutes: Route[] = [
    {
        label: "Dashboard",
        items: [
            {
                label: "Home",
                icon: "pi pi-compass",
                routerLink: ["/home"]
            }
        ]
    },
    {
        label: "Home Owner",
        permission: Permissions.CREATE_HOME,
        items: [
            {
                label: "Homes",
                icon: "pi pi-home",
                routerLink: ["/homes"]
            }
        ]
    }
];

export const UnauthenticatedRoutes: Route[] = [
    {
        label: "Login",
        icon: "pi pi-sign-in",
        routerLink: ["/login"]
    },
    {
        label: "Register",
        icon: "pi pi-user-plus",
        routerLink: ["/register"]
    }
];
