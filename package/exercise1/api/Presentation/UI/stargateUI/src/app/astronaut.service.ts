import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

export interface Person {
  personId: number;
  name: string;
  currentRank: string;
  currentDutyTitle: string;
  careerStartDate: string | null;
  careerEndDate: string | null;
}

export interface PersonResponse {
  people: Person[];
  success: boolean;
  message: string;
  responseCode: number;
}

@Injectable({
  providedIn: 'root'
})
export class AstronautService {
  private http = inject(HttpClient);
  private readonly apiUrl = 'https://localhost:7204/Person';

  getPeople(): Observable<PersonResponse> {
    return this.http.get<PersonResponse>(this.apiUrl);
  }
}