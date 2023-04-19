import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { WorkItem } from '../work-item';
import { Observable, throwError } from 'rxjs';
import { retry, catchError } from 'rxjs/operators';
import { AnyCatcher } from 'rxjs/internal/AnyCatcher';

@Injectable({
  providedIn: 'root',
})
export class ApiService {

    apiURL = 'http://localhost:34910/api';

  constructor(private http: HttpClient) {}

  httpOptions = {
    headers: new HttpHeaders({
      'Content-Type': 'application/json',
    }),
  };

  getWorkItems(): Observable<WorkItem> {
    return this.http
      .get<WorkItem>(this.apiURL + '/workitem')
      .pipe(retry(1), catchError(this.handleError));
  }

  getWorkItemsAny(): any
  {
    this.http.get<any>(this.apiURL + '/workitem')
    .subscribe(resp => {
      // display its headers
      console.log(resp);
      return resp;
    });
  }
  

  getWorkItemsByTag(tag: any): Observable<WorkItem> {
    return this.http
      .get<WorkItem>(this.apiURL + '/workitem?tag=' + tag)
      .pipe(retry(1), catchError(this.handleError));
  }

  updateWorkItem(id: any, workItem: any): Observable<WorkItem> {
    return this.http
      .put<WorkItem>(
        this.apiURL + '/workitem/' + id,
        JSON.stringify(workItem),
        this.httpOptions
      )
      .pipe(retry(1), catchError(this.handleError));
  }

  handleError(error: any) {
    let errorMessage = '';
    if (error.error instanceof ErrorEvent) {
      errorMessage = error.error.message;
    } else {
      errorMessage = `Error Code: ${error.status}\nMessage: ${error.message}`;
    }
    console.log(errorMessage);
    return throwError(() => {
      return errorMessage;
    });
  }
}