import { Component, Input } from "@angular/core";
import { HomesService } from "../../backend/services/homes/homes.service";
import { MessageService } from "primeng/api";
import { Subscription } from "rxjs";
import GetHomeResponse from "../../backend/services/homes/models/get-home-response";
import ListItem from "../../components/list/models/list-item";
import { ListComponent } from "../../components/list/list.component";
import { PanelComponent } from "../../components/panel/panel.component";

@Component({
    selector: "app-home-list",
    standalone: true,
    imports: [ListComponent, PanelComponent],
    templateUrl: "./home-list.component.html"
})
export class HomeListComponent {
    @Input() id!: string;

    title: string = "Unnamed home";
    loading: boolean = true;
    home: GetHomeResponse | null = null;
    listItems: ListItem[] = [];

    private _homesServiceSubscription: Subscription | null = null;

    constructor(
        private readonly _homesService: HomesService,
        private readonly _messageService: MessageService
    ) {}

    ngOnInit() {
        this._homesServiceSubscription = this._homesService
            .getHome(this.id)
            .subscribe({
                next: (response: GetHomeResponse) => {
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
                        this.title = response.name;
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
                }
            });
    }

    ngOnDestroy() {
        this._homesServiceSubscription?.unsubscribe();
    }
}
