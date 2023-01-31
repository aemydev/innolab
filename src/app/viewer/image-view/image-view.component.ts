import { Component, ElementRef, HostListener, OnInit } from '@angular/core';
import { ExplorerService } from 'src/app/sidebar/explorer/explorer.service';
import { DirEnty } from 'src/app/sidebar/models/dirEntry.model';

@Component({
  selector: 'app-image-view',
  templateUrl: './image-view.component.html',
  styleUrls: ['./image-view.component.css']
})
export class ImageViewComponent implements OnInit {
  public selectedFile : string | null = null;

  constructor(private _explorerService: ExplorerService) {

  }

  ngOnInit(): void {
    this._explorerService.fileSelected.subscribe(
      (file: DirEnty) => {

        if(file != null){
          console.log("filepath")

          this.selectedFile = "file://" + file.path;
          console.log(this.selectedFile)
        }else{
          this.selectedFile = null;
        }
      }
    );
  } 

}
