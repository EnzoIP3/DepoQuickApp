import { Component, Input, TemplateRef, OnInit } from "@angular/core";
import { GalleriaModule } from "primeng/galleria";

@Component({
  selector: "app-image-gallery",
  standalone: true,
  imports: [GalleriaModule],
  templateUrl: "./image-gallery.component.html"
})
export class ImageGalleryComponent implements OnInit {
  @Input() images!: string[];
  @Input() imageTemplate: TemplateRef<any> | null = null;
  @Input() numVisible: number = 3;
  imagesToDisplay: string[] = [];

  ngOnInit() {
    this.imagesToDisplay = this.images.slice(0, this.numVisible);
  }
}
