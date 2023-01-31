import { Component, EventEmitter, OnInit } from '@angular/core';
import { ExplorerService } from './explorer.service';
import { DirEnty } from '../models/dirEntry.model';

@Component({
  selector: 'app-explorer',
  templateUrl: './explorer.component.html',
  styleUrls: ['./explorer.component.scss'],
})
export class ExplorerComponent implements OnInit {
  workspacePath: string | null = null;
  files: DirEnty[];
  projectName: string;
  
  constructor(private _explorerService: ExplorerService) {
   
  }

  ngOnInit(): void {
    this._explorerService.closeWorkspace.subscribe((res) => {
      if(res == true){
        console.log("test");
        this.workspacePath = null;
        this.files = undefined;
        this.projectName = undefined;
        this._explorerService.fileSelected.emit(null);
      } 
    });
  }

  async onCreateWorkspace() {
    let projectConfig = await this._explorerService.CreateWorkspace();

    if (projectConfig != null) {
      this.workspacePath = projectConfig.Path;
      this.files = this._explorerService.files;
      this.projectName = projectConfig.ProjectName;
    }
  }

  async onOpenWorkspace() {
    let projectConfig = await this._explorerService.OpenWorkspace();

    if (projectConfig != null) {
      this.workspacePath = projectConfig.Path;
      this.files = this._explorerService.files;
      this.projectName = projectConfig.ProjectName;
    }
  }

  async onReloadWorkspace() {
    try {
      await this._explorerService.ReloadWorkspace();
      this.files = this._explorerService.files;
    } catch (e) {
      console.error('Reload workspace failed. Please try again.');
      // TODO: Popup-Reload failed. Please try again!
    }
  }

  onSelectFile(file: DirEnty) {
    this._explorerService.fileSelected.emit(file);
  }
}
