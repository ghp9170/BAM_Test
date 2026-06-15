import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { GetAstronautDutiesByNameResult } from '../../models/GetAstronautDutiesByNameResult';


@Injectable({
  providedIn: 'root'
})
export class AstronautDutyService {
  private http = inject(HttpClient);
  private readonly apiUrl = 'https://localhost:7204/AstronautDuty';

  getAstronautDuty(name : string): Observable<GetAstronautDutiesByNameResult> {
    return this.http.get<GetAstronautDutiesByNameResult>(`${this.apiUrl}/${name}`);
  }
}