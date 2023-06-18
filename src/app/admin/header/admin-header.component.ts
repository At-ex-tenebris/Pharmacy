import { Component, Input } from "@angular/core";
import { Router } from "@angular/router";
import { Link } from "src/app/shared/models/link.model";
import { AdminLoginService } from "src/app/shared/services/admin-login.service";
import { ADMIN_TOKEN_KEY } from "src/app/shared/services/authorized-base.service";

@Component({
  selector: "admin-header",
  templateUrl: "./admin-header.component.html",
  styleUrls: ["./admin-header.component.scss"],
})
export class AdminHeaderComponent {
  constructor(private router: Router,
    private adminLoginService: AdminLoginService) {}

    static MAIN_HEADER_CLASS_NAME = "admin-header-main"
  
    @Input() links : Link[] = [];
    @Input() cssClassName : string = "";

  doesPanelVisible() : boolean{
    return !!sessionStorage.getItem(ADMIN_TOKEN_KEY);
  }

  logout(){
    this.adminLoginService.logout();
    this.router.navigate(['admin/auth']);
  }

  // Добавить содержимое функции (недоделанная)

  isShown(link : Link) : boolean {
    return true;
  }

  getStyle(link: Link) : string{
    let style =  location.href.includes(link.ref) ? 'text-decoration: underline;' : '';
    if(!this.isShown(link)) style += 'display: none;';
    return style+link.style;
  }

  moveTo(ref : string){
    this.router.navigate([ref]);
  }
}
