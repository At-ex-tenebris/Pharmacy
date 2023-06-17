import {HttpHeaders} from '@angular/common/http';
import { AuthorizedToken } from '../models/authorized-token.model';
import { BaseService } from './base.service';

export const ADMIN_TOKEN_KEY = 'adminToken';

export class AuthorizedBaseService extends BaseService {
  protected authorizedToken: AuthorizedToken | null = null;

  protected getTokenHeaders(): HttpHeaders {
    let headers = new HttpHeaders();
    let token = this.getToken;
    headers = headers.append('Authorization', `Bearer ${token}`);
    return headers;
  }

  protected get getToken(): string | null {
    if (!this.authorizedToken) {
      this.authorizedToken = JSON.parse(sessionStorage.getItem(ADMIN_TOKEN_KEY) ?? 'null');
    }
    return this.authorizedToken ? this.authorizedToken.self ?? null : null;
  }
}