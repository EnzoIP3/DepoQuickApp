import {
    Component,
    Input,
    TemplateRef,
    ViewChild,
    OnInit,
    AfterViewInit,
    OnDestroy
} from "@angular/core";
import TableColumn from "../../components/table/models/table-column";
import Member from "../../backend/services/homes/models/member";
import { Subscription } from "rxjs";
import { HomesService } from "../../backend/services/homes/homes.service";
import GetMembersResponse from "../../backend/services/homes/models/get-members-response";
import { TableComponent } from "../../components/table/table.component";
import { CommonModule } from "@angular/common";
import { AvatarComponent } from "../../components/avatar/avatar.component";
import { SetNotificationsButtonComponent } from "../set-notifications-button/set-notifications-button.component";

@Component({
    selector: "app-members-table",
    standalone: true,
    imports: [
        CommonModule,
        TableComponent,
        AvatarComponent,
        SetNotificationsButtonComponent
    ],
    templateUrl: "./members-table.component.html"
})
export class MembersTableComponent implements OnInit, AfterViewInit, OnDestroy {
    @ViewChild("photoTemplate") photoTemplate: TemplateRef<any> | undefined;
    @ViewChild("nameTemplate") nameTemplate: TemplateRef<any> | undefined;
    @ViewChild("boolTemplate") boolTemplate: TemplateRef<any> | undefined;
    @ViewChild("notificationTemplate") notificationTemplate:
        | TemplateRef<any>
        | undefined;
    @Input() homeId!: string;

    columns: TableColumn[] = [
        {
            field: "photo",
            header: "Photo"
        },
        {
            field: "name",
            header: "Name"
        },
        {
            field: "canAddDevices",
            header: "Can Add Devices"
        },
        {
            field: "canListDevices",
            header: "Can List Devices"
        },
        {
            field: "canNameDevices",
            header: "Can Name Devices"
        },
        {
            field: "notifications",
            header: "Notifications"
        }
    ];

    members: any[] = [];
    private _membersSubscription: Subscription | null = null;
    private _getMembersSubscription: Subscription | null = null;
    error: string | null = null;
    loading = true;
    customTemplates: any;

    constructor(private readonly _homesService: HomesService) {}

    ngOnInit() {
        this.customTemplates = {};
        this._getMembersSubscription = this._homesService
            .getMembers(this.homeId)
            .subscribe({
                error: () => {
                    this.error =
                        "You do not have permission to view members in this home.";
                    this.loading = false;
                }
            });
        if (!this.error) {
            this._membersSubscription = this._homesService.members.subscribe({
                next: (response: GetMembersResponse | null) => {
                    if (response) {
                        this.members = response.members.map(
                            (member: Member) => {
                                return {
                                    ...member,
                                    canAddDevices: this.hasPermission(
                                        member,
                                        "add-devices"
                                    ),
                                    canListDevices: this.hasPermission(
                                        member,
                                        "get-devices"
                                    ),
                                    canNameDevices: this.hasPermission(
                                        member,
                                        "name-device"
                                    )
                                };
                            }
                        );
                    }
                    this.loading = false;
                }
            });
        }
    }

    ngAfterViewInit() {
        this.customTemplates = {
            photo: this.photoTemplate,
            name: this.nameTemplate,
            canAddDevices: this.boolTemplate,
            canListDevices: this.boolTemplate,
            shouldBeNotified: this.notificationTemplate
        };
    }

    ngOnDestroy() {
        this._getMembersSubscription?.unsubscribe();
        this._membersSubscription?.unsubscribe();
        this._homesService.clearState();
    }

    hasPermission(member: Member, permission: string): boolean {
        return member.permissions.includes(permission);
    }
}
