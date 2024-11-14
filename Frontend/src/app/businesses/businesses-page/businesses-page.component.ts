import { Component } from '@angular/core';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-businesses-page',
  templateUrl: './businesses-page.component.html',
  styles: ``
})
export class BusinessesPageComponent {
  title = "Unnamed business";
    id!: string;

    constructor(private route: ActivatedRoute) {}

    ngOnInit() {
        this.route.params.subscribe((params) => {
            this.id = params["id"];
        });
    }
}
