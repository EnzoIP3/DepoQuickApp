import { Component, OnInit } from "@angular/core";
import { ActivatedRoute } from "@angular/router";

@Component({
    selector: "app-home-page",
    templateUrl: "./home-page.component.html"
})
export class HomePageComponent implements OnInit {
    title = "Unnamed home";
    id!: string;

    constructor(private route: ActivatedRoute) {}

    ngOnInit() {
        this.route.params.subscribe((params) => {
            this.id = params["id"];
        });
    }
}
