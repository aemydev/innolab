import { Component } from '@angular/core';
import { ExplorerService } from '../sidebar/explorer/explorer.service';

@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.scss']
})
export class HeaderComponent {

  constructor(private _explorerService:ExplorerService){

  }
  
  onCloseWorkspace(){
    this._explorerService.closeWorkspace.emit(true);
  }


}
