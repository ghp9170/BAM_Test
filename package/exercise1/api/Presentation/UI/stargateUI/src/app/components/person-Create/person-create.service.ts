import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { PersonResponse } from '../../models/PersonResponse';


@Injectable({
  providedIn: 'root'
})
export class PersonCreateService {
  private http = inject(HttpClient);
  private readonly apiUrl = 'https://localhost:7204/Person';

  create(name:string): Observable<PersonResponse> {
    return this.http.post<PersonResponse>(this.apiUrl, JSON.stringify(name), {
      headers: {
        'Content-Type': 'application/json'
      }
    });
  }
}
