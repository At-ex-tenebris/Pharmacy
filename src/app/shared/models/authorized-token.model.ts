import { AuthData } from './auth-data.model';

export class AuthorizedToken {
  self: string;
  authData: AuthData;
  authType: string;
  constructor(self: string, authData: AuthData, authType: string) {
    this.self = self;
    this.authData = authData;
    this.authType = authType;
  }
}
