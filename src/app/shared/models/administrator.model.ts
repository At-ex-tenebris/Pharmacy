export class Administrator{
    id : number;
    login : string;
    password : string;

    constructor(Id: number, login: string, password: string) {
        this.id = Id;
        this.login = login;
        this.password = password;
      }
    
}