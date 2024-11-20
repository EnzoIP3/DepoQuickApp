import {
    Component,
    EventEmitter,
    Input,
    Output,
    OnInit,
    OnDestroy
} from "@angular/core";
import { HomesService } from "../../backend/services/homes/homes.service";
import { MessageService } from "primeng/api";
import { Subscription } from "rxjs";
import GetHomeResponse from "../../backend/services/homes/models/get-home-response";
import ListItem from "../../components/list/models/list-item";
import { ListComponent } from "../../components/list/list.component";
import { Router } from "@angular/router";

@Component({
    selector: "app-home-list",
    standalone: true,
    imports: [ListComponent],
    templateUrl: "./home-list.component.html"
})
export class HomeListComponent implements OnInit, OnDestroy {
    @Input() id!: string;
    @Output() onHomeNameChange = new EventEmitter<string>();

    loading = true;
    home: GetHomeResponse | null = null;
    listItems: ListItem[] = [];

    private _homeSubscription: Subscription | null = null;
    private _getHomeSubscription: Subscription | null = null;

    constructor(
        private readonly _homesService: HomesService,
        private readonly _messageService: MessageService,
        private readonly _router: Router
    ) {}

    ngOnInit() {
        this._getHomeSubscription = this._homesService
            .getHome(this.id)
            .subscribe();
        this._homeSubscription = this._homesService.home.subscribe({
            next: (response: GetHomeResponse | null) => {
                if (!response) {
                    return;
                }

                this.listItems = [
                    {
                        label: "Owner Name",
                        value: `${response.owner.name} ${response.owner.surname}`
                    },
                    {
                        label: "Address",
                        value: response.address
                    },
                    {
                        label: "Latitude",
                        value: response.latitude.toString()
                    },
                    {
                        label: "Longitude",
                        value: response.longitude.toString()
                    },
                    {
                        label: "Limit of Members",
                        value: response.maxMembers.toString()
                    }
                ];

                if (response.name) {
                    this.onHomeNameChange.emit(response.name);
                }

                this.loading = false;
            },
            error: (error) => {
                this.loading = false;
                this._messageService.add({
                    severity: "error",
                    summary: "Error",
                    detail: error.message
                });
                this._router.navigate(["/homes"]);
            }
        });
    }

    ngOnDestroy() {
        this._getHomeSubscription?.unsubscribe();
        this._homeSubscription?.unsubscribe();
    }
}
