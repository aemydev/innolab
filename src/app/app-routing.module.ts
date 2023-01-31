import { NgModule } from "@angular/core";
import { RouterModule, Routes } from "@angular/router";
import { HelppageComponent } from "./helppage/helppage.component";
import { SettingsComponent } from "./settings/settings.component";
import { ViewerComponent } from "./viewer/viewer.component";

const appRoutes: Routes = [
    {path: '', component: ViewerComponent},
    {path: 'settings', component: SettingsComponent},
    {path: 'help', component: HelppageComponent}
]

@NgModule({
    imports: [
        RouterModule.forRoot(appRoutes)
    ],
    exports:[ RouterModule ]
})
export class AppRoutingModule {}