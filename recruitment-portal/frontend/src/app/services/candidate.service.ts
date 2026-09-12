import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';
import { Candidate } from '../models/candidate.model';

@Injectable({ providedIn: 'root' })
export class CandidateService {
  private readonly baseUrl = `${environment.apiUrl}/candidates`;

  constructor(private http: HttpClient) {}

  getAll(): Observable<Candidate[]> {
    return this.http.get<Candidate[]>(this.baseUrl);
  }

  getById(id: number): Observable<Candidate> {
    return this.http.get<Candidate>(`${this.baseUrl}/${id}`);
  }

  create(candidate: Candidate): Observable<Candidate> {
    return this.http.post<Candidate>(this.baseUrl, candidate);
  }

  update(id: number, candidate: Candidate): Observable<void> {
    return this.http.put<void>(`${this.baseUrl}/${id}`, candidate);
  }

  delete(id: number): Observable<void> {
    return this.http.delete<void>(`${this.baseUrl}/${id}`);
  }
}
