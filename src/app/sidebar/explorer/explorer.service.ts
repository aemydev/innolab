import { EventEmitter, Injectable } from '@angular/core';
import { ProjectConfig } from '../models/projectConfig.model';
import { ElectronService } from 'src/app/electron/electron.service';
import { DirEnty } from '../models/dirEntry.model';

@Injectable({
  providedIn: 'root',
})
export class ExplorerService {
  projectConfig: ProjectConfig | null;
  files: DirEnty[];
  public fileSelected: EventEmitter<DirEnty> = new EventEmitter<DirEnty>();
  public closeWorkspace:EventEmitter<boolean> = new EventEmitter<boolean>();

  constructor(private _electronService: ElectronService) {
    this.projectConfig = null;
  }

  // TODO: Add projectname
  public async CreateWorkspace(): Promise<ProjectConfig> {
    const projectName = 'Workspace';

    try {
      let folderPath: string = await this.GetFolderPath();
      // TODO: Add Validation for folderPath?
      await this.GetAllFilesInDir(folderPath);
      await this.CreateConfig(projectName, folderPath);
    } catch (e) {
      // TODO: Catch for folder
      return null;
    }

    return this.projectConfig;
  }

  // Reload all Files from workspace
  public async ReloadWorkspace() {
    try {
      this.files = await this._electronService.getAllFilesInDir(
        this.projectConfig.Path
      );
    } catch (e) {
      throw new Error('Reload failed'); // -> Catched by component
    }
  }

  public async OpenWorkspace() {
    try {
      let path = await this.GetFilePath();
      let config = await this._electronService.readFile(path);
      if (config != undefined) {
        this.projectConfig = JSON.parse(config);
        this.files = await this._electronService.getAllFilesInDir(
          this.projectConfig.Path
        );
      }
    } catch {
      return null; // no config
    }

    return this.projectConfig;
  }

  /* Private */
  // returns null or path
  private async GetFolderPath(): Promise<any> {
    console.info('[ExplorerService]: GetFolderPath called');
    try {
      let folderPath = await this._electronService.openFolderPathDialog();
      if (folderPath) {
        return folderPath;
      } else {
        throw new Error('No folder selected');
      }
    } catch (e) {
      throw new Error('Error. Could not get folderPath', e);
    }
  }

  private async GetAllFilesInDir(folderPath: string, imagesOnly = true) {
    try {
      let folderContent = await this._electronService.getAllFilesInDir(
        folderPath
      );

      if (imagesOnly) {
        this.files = this.filterImagesOnly(folderContent);
      } else {
        this.files = folderContent;
      }
    } catch (e) {
      console.error('Could not get all files in directory' + folderPath, e);
    }
  }

  private async CreateConfig(name: string, folderPath: string) {
    this.projectConfig = new ProjectConfig(name, folderPath, 'hash');

    // Save config as json-file to fs
    await this._electronService.saveFile(
      folderPath,
      `project.json`,
      JSON.stringify(this.projectConfig)
    );
  }

  private async GetFilePath() {
    console.info('[ExplorerService]: GetFilePath called');
    try {
      let filePath = await this._electronService.openFile();
      if (filePath) {
        // TODO: Validate File Path -> last part = "workspace.json"

        return filePath;
      } else {
        throw new Error('No file selected');
      }
    } catch (e) {
      throw new Error('Error. Could not get filePath', e);
    }
  }

  private filterImagesOnly(files: DirEnty[]) {
    let images: DirEnty[];

    for (let item of files) {
      if (item.type == 'File') {
      }
    }
    return files;
  }
}
