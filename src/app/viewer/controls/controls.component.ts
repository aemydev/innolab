import { HttpClient, HttpParams } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { ExplorerService } from 'src/app/sidebar/explorer/explorer.service';
import { DirEnty } from 'src/app/sidebar/models/dirEntry.model';

@Component({
  selector: 'app-controls',
  templateUrl: './controls.component.html',
  styleUrls: ['./controls.component.css'],
})
export class ControlsComponent implements OnInit {
  selectedFile: DirEnty;
  angle:number;
  fusionindex:number;
  
  constructor(
    private _explorerService: ExplorerService,
    private _httpClient: HttpClient
  ) {}

  ngOnInit(): void {
    this._explorerService.fileSelected.subscribe((file: DirEnty) => {
      this.selectedFile = file;
    });
  }

  async onCalculateFusionIndex() {
    if (this.selectedFile) {
      //call api
      try {
        let queryParams = new HttpParams();
        queryParams = queryParams.append(
          'filepath',
          this.selectedFile.path
        );
        queryParams = queryParams.append(
          'path',
          this._explorerService.projectConfig.Path
        );

        await this._httpClient
          .get('http://localhost:8080/cellanalyzer/analyze/fusionindex', { params: queryParams })
          .subscribe((res) => {
            console.log(res);
            let resultEntry:DirEnty = new DirEnty();
            resultEntry.filename="result";
            resultEntry.path="path";

            // emit file
            this._explorerService.fileSelected.emit(resultEntry);
          });
      } catch {}
    }
  }
  
  async onCalcAngle() {   
    if (this.selectedFile) {
      // call api
      try {
        let queryParams = new HttpParams();
        queryParams = queryParams.append(
          'filepath',
          this.selectedFile.path
        );
        queryParams = queryParams.append(
          'path',
          this._explorerService.projectConfig.Path
        );

        await this._httpClient
          .get('http://localhost:8080/cellanalyzer/analyze/angle', { params: queryParams })
          .subscribe((res) => {
            console.log(res);
            let resultEntry:DirEnty = new DirEnty();
            resultEntry.filename="result";
            resultEntry.path="";
            // emit file
            this.angle=0;
            this._explorerService.fileSelected.emit(resultEntry);
          });
      } catch {}
    }
  }
}
