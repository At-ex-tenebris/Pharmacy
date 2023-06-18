import {Injectable} from '@angular/core';
import {Observable, of} from 'rxjs';
import {HttpClient} from '@angular/common/http';
import {catchError, mergeMap} from 'rxjs/operators';
import { ADMIN_TOKEN_KEY, AuthorizedBaseService } from './authorized-base.service';
import { AuthorizedToken } from '../models/authorized-token.model';
import { BaseService } from './base.service';
import { City } from '../models/city.model';
import { Paginated } from '../models/paginated.model';
import { AuthData } from '../models/auth-data.model';

const STD_PASSWORD = 'auth_example';

@Injectable()
export class TemplateService extends AuthorizedBaseService {
    private CONTROLLER_URL = `${this.BACKEND_URL}/template`;

    constructor(http: HttpClient) {
        super(http);
    }

    public getList(regionId: number = 0, cityName: string = '', 
        pageNum : number = 0, pageSize : number = 2) : Observable<Paginated<City>>{
        let methodName = 'list';
        let url = `${this.CONTROLLER_URL}/${methodName}`;
        let params = {regionId: regionId, cityName : cityName, 
            pageNum : pageNum, pageSize : pageSize};
        return this.http.get<Paginated<City>>(url, {params: params})
            .pipe(catchError(this.handleError));
    }

    public getConcrete(id : number) : Observable<City>{
        let url = `${this.CONTROLLER_URL}/${id}`;
        let headers = this.getTokenHeaders();
        return this.http.get<City>(url,{headers: headers})
            .pipe(catchError(this.handleError));
    }
    
    public addEntry(entry : City, authData: AuthData | null = null, authType : string = "") : Observable<City>{
        if (authData == null) authData = this.authorizedToken?.authData ?? null;
        if (authType == '') authType = this.authorizedToken?.authType ?? '';
        let url = `${this.CONTROLLER_URL}`;
        let headers = this.getTokenHeaders();
        let body = {authData : authData, fullCity : entry};
        let params = {authType : authType};
        return this.http.post<City>(url, body, { headers: headers, params : params })
            .pipe(catchError(this.handleError));
    }

    public redactEntry(id : number, entry : City, authData: AuthData | null = null, authType : string = "") : Observable<City>{
        if (authData == null) authData = this.authorizedToken?.authData ?? null;
        if (authType == '') authType = this.authorizedToken?.authType ?? '';
        let url = `${this.CONTROLLER_URL}/${id}`;
        let headers = this.getTokenHeaders();
        let body = {authData : authData, fullCity : entry};
        let params = {authType : authType};
        return this.http.put<City>(url, body, { headers: headers, params : params })
            .pipe(catchError(this.handleError));
    }

    public deleteEntry(id : number, authData: AuthData | null = null, authType : string = "") : Observable<City>{
        if (authData == null) authData = this.authorizedToken?.authData ?? null;
        if (authType == '') authType = this.authorizedToken?.authType ?? '';
        let url = `${this.CONTROLLER_URL}/${id}`;
        let headers = this.getTokenHeaders().append('Content-Type', 'application/json');
        let body = authData;
        let params = {authType : authType};
        return this.http.request<City>('delete', url, { body : body, headers: headers, params : params})
            .pipe(catchError(this.handleError));
    }
    
    public getInfo(id : number, authData: AuthData | null = null, authType : string = "") : Observable<City>{
        if (authData == null) authData = this.authorizedToken?.authData ?? null;
        if (authType == '') authType = this.authorizedToken?.authType ?? '';
        let url = `${this.CONTROLLER_URL}/${id}`;
        let headers = this.getTokenHeaders();
        let body = authData;
        let params = {authType : authType};
        return this.http.post<City>(url, body, {headers: headers, params : params})
            .pipe(catchError(this.handleError));
    }
}
