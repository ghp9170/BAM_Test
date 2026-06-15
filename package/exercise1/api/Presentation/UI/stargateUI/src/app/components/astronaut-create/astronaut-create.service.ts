import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { CreateAstronautDetail } from '../../models/CreateAstronautDetail';
import { environment } from '../../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class AstronautCreateService {
  private http = inject(HttpClient);
  private readonly apiUrl = `${environment.apiUrl}/AstronautDetail`;

  create(details: CreateAstronautDetail): Observable<any> {
    return this.http.post(this.apiUrl, details);
  }
}
