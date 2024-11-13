import { Component, Input, TemplateRef, ViewChild } from "@angular/core";
import TableColumn from "../../components/table/models/table-column";
import Member from "../../backend/services/homes/models/member";
import { Subscription } from "rxjs";
import { HomesService } from "../../backend/services/homes/homes.service";
import { MessageService } from "primeng/api";
import GetMembersResponse from "../../backend/services/homes/models/get-members-response";
import { TableComponent } from "../../components/table/table.component";
import { CommonModule } from "@angular/common";
import { AvatarComponent } from "../../components/avatar/avatar.component";
import { ButtonComponent } from "../../components/button/button.component";
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
export class MembersTableComponent {
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
            field: "shouldBeNotified",
            header: "Notifications"
        }
    ];

    members: Member[] = [];
    private _homesServiceSubscription: Subscription | null = null;
    loading: boolean = true;
    customTemplates: any;

    constructor(
        private readonly _homesService: HomesService,
        private readonly _messageService: MessageService
    ) {}

    ngOnInit() {
        this.customTemplates = {};
        this._homesServiceSubscription = this._homesService
            .getMembers(this.homeId)
            .subscribe({
                next: (response: GetMembersResponse) => {
                    this.members = response.members;
                    this.loading = false;
                },
                error: (error) => {
                    this.loading = false;
                    this._messageService.add({
                        severity: "error",
                        summary: "Error",
                        detail: error.message
                    });
                }
            });
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
        this._homesServiceSubscription?.unsubscribe();
    }
}
