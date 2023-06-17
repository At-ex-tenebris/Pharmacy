import {throwError} from 'rxjs';
import {Injectable} from '@angular/core';
import {HttpClient} from '@angular/common/http';

@Injectable()
export class BaseService {
    
    protected BACKEND_URL = 'http://localhost:1011/api';

    constructor(protected http: HttpClient) {}

    protected handleError(error: any) {
    let errMsg = error.message ? 
        error.message : 
        (error.status ? `${error.status} - ${error.statusText}` : 'Server server-error');
    return throwError(() => errMsg);
    }

    protected handleErrorStatus(error: any) {
    return throwError(() => ({status: error.status, body: error.error}));
    }
}