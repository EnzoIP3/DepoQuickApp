import { Component } from '@angular/core';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-business-page',
  templateUrl: './business-page.component.html',
  styles: ``
})
export class BusinessPageComponent {
  title = "Unnamed business";
    id!: string;

    constructor(private route: ActivatedRoute) {}

    ngOnInit() {
        this.route.params.subscribe((params) => {
            this.id = params["id"];
        });
    }
}
