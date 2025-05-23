import { Component, OnInit } from "@angular/core";
import { ActivatedRoute } from "@angular/router";

@Component({
    selector: "app-business-devices-page",
    templateUrl: "./business-devices-page.component.html"
})
export class BusinessDevicesPageComponent implements OnInit {
    constructor(private readonly _route: ActivatedRoute) {}

    businessId!: string;

    ngOnInit() {
        this._route.params.subscribe((params) => {
            this.businessId = params["id"];
        });
    }
}
