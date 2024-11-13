import { Component, Input } from "@angular/core";
import TableColumn from "../../components/table/models/table-column";
import Member from "../../backend/services/homes/models/member";
import { Subscription } from "rxjs";
import { HomesService } from "../../backend/services/homes/homes.service";
import { MessageService } from "primeng/api";
import GetMembersResponse from "../../backend/services/homes/models/get-members-response";
import { TableComponent } from "../../components/table/table.component";

@Component({
    selector: "app-members-table",
    standalone: true,
    imports: [TableComponent],
    templateUrl: "./members-table.component.html"
})
export class MembersTableComponent {
    @Input() homeId!: string;

    columns: TableColumn[] = [
        {
            field: "name",
            header: "Name"
        },
        {
            field: "surname",
            header: "Surname"
        },
        {
            field: "photo",
            header: "Photo"
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
            header: "Should Be Notified"
        }
    ];

    members: Member[] = [];
    private _homesServiceSubscription: Subscription | null = null;
    loading: boolean = true;

    constructor(
        private readonly _homesService: HomesService,
        private readonly _messageService: MessageService
    ) {}

    ngOnInit() {
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

    ngOnDestroy() {
        this._homesServiceSubscription?.unsubscribe();
    }
}
