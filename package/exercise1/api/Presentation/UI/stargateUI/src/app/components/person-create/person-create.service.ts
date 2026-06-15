import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { PersonResponse } from '../../models/PersonResponse';
import { environment } from '../../../environments/environment';


@Injectable({
  providedIn: 'root'
})
export class PersonCreateService {
  private http = inject(HttpClient);
  private readonly apiUrl = `${environment.apiUrl}/Person`;

  create(name:string): Observable<PersonResponse> {
    return this.http.post<PersonResponse>(this.apiUrl, JSON.stringify(name), {
      headers: {
        'Content-Type': 'application/json'
      }
    });
  }
}
