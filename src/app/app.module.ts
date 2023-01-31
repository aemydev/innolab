import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppComponent } from './app.component';
import { SidebarComponent } from './sidebar/sidebar.component';
import { HeaderComponent } from './header/header.component';
import { DropdownDirective } from './shared/directives/dropdown.directive';
import { ViewerComponent } from './viewer/viewer.component';
import { SettingsComponent } from './settings/settings.component';
import { HelppageComponent } from './helppage/helppage.component';
import { AppRoutingModule } from './app-routing.module';
import { ToolbarComponent } from './sidebar/toolbar/toolbar.component';

import { MatIconModule } from '@angular/material/icon';
import { ExplorerComponent } from './sidebar/explorer/explorer.component';
import { ExpandableDirective } from './sidebar/explorer/expandable.directive';
import { ImageViewComponent } from './viewer/image-view/image-view.component';
import { ControlsComponent } from './viewer/controls/controls.component';
import { ZoomDirDirective } from './viewer/image-view/zoom-dir.directive';
import { FormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { Routes } from '@angular/router';

const appRputes : Routes = [
  {path:'', component: ViewerComponent},
  {path:'settings', component:SettingsComponent}
]

@NgModule({
  declarations: [
    AppComponent,
    SidebarComponent,
    HeaderComponent,
    DropdownDirective,
    ViewerComponent,
    SettingsComponent,
    HelppageComponent,
    ToolbarComponent,
    ExplorerComponent,
    ExpandableDirective,
    ViewerComponent,
    ImageViewComponent,
    ControlsComponent,
    ZoomDirDirective 
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    MatIconModule,
    FormsModule,
    HttpClientModule
    
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
