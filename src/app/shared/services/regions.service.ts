import { Injectable } from '@angular/core';
import { Observable, of } from 'rxjs';
import { HttpClient } from '@angular/common/http';
import { catchError, mergeMap } from 'rxjs/operators';
import {
  ADMIN_TOKEN_KEY,
  AuthorizedBaseService,
} from './authorized-base.service';
import { AuthorizedToken } from '../models/authorized-token.model';
import { BaseService } from './base.service';
import { Paginated } from '../models/paginated.model';
import { Region } from '../models/region.model';
import { AuthData } from '../models/auth-data.model';

@Injectable()
export class RegionsService extends AuthorizedBaseService {
  private CONTROLLER_URL = `${this.BACKEND_URL}/region`;

  constructor(http: HttpClient) {
    super(http);
  }

  public getList(
    countryId: number = 0,
    regionName: string = '',
    pageNum: number = 0,
    pageSize: number = 2
  ): Observable<Paginated<Region>> {
    let methodName = 'list';
    let url = `${this.CONTROLLER_URL}/${methodName}`;
    let params = {
      countryId: countryId,
      regionName: regionName,
      pageNum: pageNum,
      pageSize: pageSize,
    };
    return this.http
      .get<Paginated<Region>>(url, { params: params })
      .pipe(catchError(this.handleError));
  }

  public getConcrete(id: number): Observable<Region> {
    let url = `${this.CONTROLLER_URL}/${id}`;
    let headers = this.getTokenHeaders();
    return this.http
      .get<Region>(url, { headers: headers })
      .pipe(catchError(this.handleError));
  }

  public addEntry(
    entry: Region,
    authData: AuthData | null = null,
    authType: string = ''
  ): Observable<Region> {
    if (authData == null) authData = this.authorizedToken?.authData ?? null;
    if (authType == '') authType = this.authorizedToken?.authType ?? '';
    let url = `${this.CONTROLLER_URL}`;
    let headers = this.getTokenHeaders();
    let body = { authData: authData, fullRegion: entry };
    let params = {
      authType: authType,
    };
    return this.http
      .post<Region>(url, body, { headers: headers, params: params })
      .pipe(catchError(this.handleError));
  }

  public redactEntry(
    id: number,
    entry: Region,
    authData: AuthData | null = null,
    authType: string = ''
  ): Observable<Region> {
    if (authData == null) authData = this.authorizedToken?.authData ?? null;
    if (authType == '') authType = this.authorizedToken?.authType ?? '';
    let url = `${this.CONTROLLER_URL}/${id}`;
    let headers = this.getTokenHeaders();
    let body = { authData: authData, fullRegion: entry };
    let params = {
      authType: authType,
    };
    return this.http
      .put<Region>(url, body, { headers: headers, params: params })
      .pipe(catchError(this.handleError));
  }

  public deleteEntry(
    id: number,
    authData: AuthData | null = null,
    authType: string = ''
  ): Observable<Region> {
    if (authData == null) authData = this.authorizedToken?.authData ?? null;
    if (authType == '') authType = this.authorizedToken?.authType ?? '';
    let url = `${this.CONTROLLER_URL}/${id}`;
    let headers = this.getTokenHeaders().append(
      'Content-Type',
      'application/json'
    );
    let body = authData;
    let params = {
      authType: authType,
    };
    return this.http
      .request<Region>('delete', url, {
        body: body,
        headers: headers,
        params: params,
      })
      .pipe(catchError(this.handleError));
  }

  public getEntry(
    id: number,
    authData: AuthData | null = null,
    authType: string = ''
  ): Observable<Region> {
    if (authData == null) authData = this.authorizedToken?.authData ?? null;
    if (authType == '') authType = this.authorizedToken?.authType ?? '';
    let url = `${this.CONTROLLER_URL}/${id}`;
    let headers = this.getTokenHeaders();
    let body = authData;
    let params = {
      authType: authType,
    };
    return this.http
      .post<Region>(url, body, { headers: headers, params: params })
      .pipe(catchError(this.handleError));
  }
}
