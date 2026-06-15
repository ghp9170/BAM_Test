import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { GetAstronautDutiesByNameResult } from '../../models/GetAstronautDutiesByNameResult';
import { environment } from '../../../environments/environment';


@Injectable({
  providedIn: 'root'
})
export class AstronautDutyService {
  private http = inject(HttpClient);
  private readonly apiUrl = `${environment.apiUrl}/AstronautDuty`;

  getAstronautDuty(name : string): Observable<GetAstronautDutiesByNameResult> {
    return this.http.get<GetAstronautDutiesByNameResult>(`${this.apiUrl}/${name}`);
  }
}
