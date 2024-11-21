import { Component, Input, OnInit } from "@angular/core";
import { AvatarModule } from "primeng/avatar";

@Component({
    selector: "app-avatar",
    standalone: true,
    imports: [AvatarModule],
    templateUrl: "./avatar.component.html"
})
export class AvatarComponent implements OnInit {
    @Input() image: string | undefined;
    @Input() defaultIcon = "pi pi-user";
    @Input() size: "normal" | "large" | "xlarge" | undefined = "large";
    @Input() shape: "square" | "circle" | undefined = "circle";

    icon: string | undefined;

    ngOnInit() {
        if (!this.image) {
            this.icon = this.defaultIcon;
        }
    }

    handleImageError() {
        this.icon = this.defaultIcon;
        this.image = undefined;
    }
}
