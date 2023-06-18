import { CommonModule } from "@angular/common";
import { NgModule } from "@angular/core";
import { FormsModule } from "@angular/forms";
import { AdminHeaderModule } from "../header/admin-header.module";
import { AdminRegionsComponent } from "./admin-regions.component";
import { AdminRegionsRoutingModule } from "./admin-regions.routing";


@NgModule({
    imports: [AdminRegionsRoutingModule, CommonModule, FormsModule, AdminHeaderModule, ],
    declarations: [AdminRegionsComponent],
    exports: [],
    providers: [],
  })
  export class AdminRegionsModule {}