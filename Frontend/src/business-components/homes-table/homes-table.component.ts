import { Component, OnInit, OnDestroy } from "@angular/core";
import TableColumn from "../../components/table/models/table-column";
import { Subscription } from "rxjs";
import { HomesService } from "../../backend/services/homes/homes.service";
import Home from "../../backend/services/homes/models/home";
import { TableComponent } from "../../components/table/table.component";
import GetHomesResponse from "../../backend/services/homes/models/get-homes-response";
import { Router } from "@angular/router";
import { CommonModule } from "@angular/common";
import { MessagesService } from "../../backend/services/messages/messages.service";

@Component({
    selector: "app-homes-table",
    standalone: true,
    imports: [CommonModule, TableComponent],
    templateUrl: "./homes-table.component.html"
})
export class HomesTableComponent implements OnInit, OnDestroy {
    columns: TableColumn[] = [
        {
            field: "address",
            header: "Address"
        },
        {
            field: "latitude",
            header: "Lat."
        },
        {
            field: "longitude",
            header: "Long."
        },
        {
            field: "maxMembers",
            header: "Limit of Members"
        },
        {
            field: "name",
            header: "Name"
        }
    ];

    homes: Home[] = [];
    private _homesServiceSubscription: Subscription | null = null;
    loading = true;

    constructor(
        private readonly _homesService: HomesService,
        private readonly _messagesService: MessagesService,
        private readonly _router: Router
    ) {}

    ngOnInit() {
        this._homesServiceSubscription = this._homesService
            .getHomes()
            .subscribe({
                next: (response: GetHomesResponse) => {
                    this.homes = response.homes;
                    this.loading = false;
                },
                error: (error) => {
                    this.loading = false;
                    this._messagesService.add({
                        severity: "error",
                        summary: "Error",
                        detail: error.message
                    });
                }
            });
    }

    onRowClick(home: Home) {
        this._router.navigate(["/homes", home.id]);
    }

    ngOnDestroy() {
        this._homesServiceSubscription?.unsubscribe();
    }
}
