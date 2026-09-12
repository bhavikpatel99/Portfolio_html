import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';
import { Country, StateItem } from '../models/lookup.model';

@Injectable({ providedIn: 'root' })
export class LookupService {
  private readonly baseUrl = `${environment.apiUrl}/lookups`;

  constructor(private http: HttpClient) {}

  getCountries(): Observable<Country[]> {
    return this.http.get<Country[]>(`${this.baseUrl}/countries`);
  }

  getStates(countryId: number): Observable<StateItem[]> {
    return this.http.get<StateItem[]>(`${this.baseUrl}/countries/${countryId}/states`);
  }

  /** Generic lookup categories: Gender, MaritalStatus, EmploymentType, Source. */
  getCategory(category: string): Observable<string[]> {
    return this.http.get<string[]>(`${this.baseUrl}/categories/${category}`);
  }
}
