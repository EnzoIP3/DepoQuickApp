import Route from "../../../../components/sidebar/models/route";
import { Permissions } from "./permissions";

export const AuthenticatedRoutes: Route[] = [
    {
        label: "Devices",
        items: [
            {
                label: "All devices",
                icon: "pi pi-lightbulb",
                routerLink: ["/devices"]
            },
            {
                label: "Notifications",
                icon: "pi pi-bell",
                routerLink: ["/notifications"],
                permission: Permissions.GET_NOTIFICATIONS
            }
        ]
    },
    {
        label: "Homes",
        permission: Permissions.CREATE_HOME,
        items: [
            {
                label: "My homes",
                icon: "pi pi-home",
                routerLink: ["/homes"]
            }
        ]
    },
    {
        label: "Admin",
        permission: Permissions.CREATE_ADMINISTATOR,
        items: [
            {
                label: "Administration",
                icon: "pi pi-shield",
                routerLink: ["/admins"]
            }
        ]
    },
    {
        label: "Businesses",
        permission: Permissions.CREATE_BUSINESS,
        items: [
            {
                label: "My businesses",
                icon: "pi pi-building",
                routerLink: ["/businesses"]
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
