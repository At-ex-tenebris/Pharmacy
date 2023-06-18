import { Component, OnInit, ViewChild } from "@angular/core";
import { ActivatedRoute, Params, Router } from "@angular/router";
import { AdminLoginService } from "src/app/shared/services/admin-login.service";

@Component({
  selector: "admin-auth",
  templateUrl: "./admin-auth.component.html",
  styleUrls: ["./admin-auth.component.scss"],
})
export class AdminAuthComponent {

  constructor(
    public adminLoginService: AdminLoginService,
    private router: Router,
  ) {}

  password : string = "";

  login(){
    // this.adminLoginService.login(this.password).subscribe({
    //   next: (resp)=>{
    //     alert('Авторизация прошла успешно.');
    //     this.router.navigate(['admin']);
    //   },
    //   error : (err) => {
    //     console.log(err);
    //   },
    //   complete: ()=>{
    //     //...
    //   }
    // })
  }

  ngOnInit(): void {
    // this.adminLoginService.isAuthorized().subscribe((res) => {
    //   if(res) this.router.navigate(['admin']);
    // });
  }
}
