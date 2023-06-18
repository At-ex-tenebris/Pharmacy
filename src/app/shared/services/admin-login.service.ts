import { Injectable } from "@angular/core";
import { BaseService } from "./base.service";
import { AuthorizedToken } from "../models/authorized-token.model";
import { HttpClient } from "@angular/common/http";
import { Observable } from "rxjs/internal/Observable";
import { of } from "rxjs/internal/observable/of";

@Injectable()
export class AdminLoginService extends BaseService {
  private LOGIN_FORM_URL = 'admin/auth';
  private authorizedToken: AuthorizedToken | null = null;

  constructor(http: HttpClient) {
    super(http);
  }

  // Недоделанная функция 
  public logout(): Observable<string> {
    return of(this.LOGIN_FORM_URL);
  }

}