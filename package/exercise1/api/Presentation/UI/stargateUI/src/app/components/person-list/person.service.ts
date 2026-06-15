import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { PersonResponse } from '../../models/PersonResponse';


@Injectable({
  providedIn: 'root'
})
export class PersonService {
  private http = inject(HttpClient);
  private readonly apiUrl = 'https://localhost:7204/Person';

  getPeople(): Observable<PersonResponse> {
    return this.http.get<PersonResponse>(this.apiUrl);
  }
}
