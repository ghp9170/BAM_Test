import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { PersonResponse } from '../../models/PersonResponse';
import { environment } from '../../../environments/environment';


@Injectable({
  providedIn: 'root'
})
export class PersonService {
  private http = inject(HttpClient);
  private readonly apiUrl = `${environment.apiUrl}/Person`;

  getPeople(): Observable<PersonResponse> {
    return this.http.get<PersonResponse>(this.apiUrl);
  }
}
