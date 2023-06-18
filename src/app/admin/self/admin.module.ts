import { CommonModule } from "@angular/common";
import { NgModule } from "@angular/core";
import { FormsModule } from "@angular/forms";
import { AdminComponent } from "./admin.component";
import { AdminRoutingModule } from "./admin.routing";
import { AdminAuthComponent } from "../auth/admin-auth.component";
import { AdminHeaderComponent } from "../header/admin-header.component";
import { AdminHeaderModule } from "../header/admin-header.module";
import { AdminLoginService } from "src/app/shared/services/admin-login.service";


@NgModule({
    imports: [AdminRoutingModule, CommonModule, FormsModule, AdminHeaderModule],
    declarations: [AdminComponent, AdminAuthComponent],
    exports: [],
    providers: [AdminLoginService],
  })
  export class AdminModule {}