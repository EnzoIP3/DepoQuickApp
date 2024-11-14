import { Component, Input, TemplateRef } from "@angular/core";
import { GalleriaModule } from "primeng/galleria";

@Component({
    selector: "app-image-gallery",
    standalone: true,
    imports: [GalleriaModule],
    templateUrl: "./image-gallery.component.html"
})
export class ImageGalleryComponent {
    @Input() images!: string[];
    @Input() imageTemplate: TemplateRef<any> | null = null;
}
