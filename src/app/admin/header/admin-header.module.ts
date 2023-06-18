import { CommonModule } from "@angular/common";
import { NgModule } from "@angular/core";
import { FormsModule } from "@angular/forms";
import { AdminAuthComponent } from "../auth/admin-auth.component";
import { AdminHeaderComponent } from "./admin-header.component";
import { AdminLoginService } from "src/app/shared/services/admin-login.service";


@NgModule({
    imports: [CommonModule],
    declarations: [AdminHeaderComponent],
    exports: [],
    providers: [AdminLoginService],
  })
  export class AdminHeaderModule {}