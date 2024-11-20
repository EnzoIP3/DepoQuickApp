import { Component, Input } from "@angular/core";
import { ImageModule } from "primeng/image";

@Component({
    selector: "app-image",
    standalone: true,
    imports: [ImageModule],
    templateUrl: "./image.component.html"
})
export class ImageComponent {
    @Input() image: string = "";

    onImageError(event: Event): void {
    }
}
