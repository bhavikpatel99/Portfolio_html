import { Component, OnInit, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router, RouterLink } from '@angular/router';
import { AuthService } from '../../services/auth.service';
import { CandidateService } from '../../services/candidate.service';
import { Candidate } from '../../models/candidate.model';

@Component({
  selector: 'app-dashboard',
  standalone: true,
  imports: [CommonModule, RouterLink],
  templateUrl: './dashboard.component.html'
})
export class DashboardComponent implements OnInit {
  candidates = signal<Candidate[]>([]);
  loading = signal(true);
  errorMessage = signal('');

  constructor(
    public auth: AuthService,
    private candidateService: CandidateService,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.load();
  }

  load(): void {
    this.loading.set(true);
    this.candidateService.getAll().subscribe({
      next: (data) => {
        this.candidates.set(data);
        this.loading.set(false);
      },
      error: () => {
        this.errorMessage.set('Could not load candidates.');
        this.loading.set(false);
      }
    });
  }

  edit(id?: number): void {
    if (id) this.router.navigate(['/candidate', id]);
  }

  remove(candidate: Candidate): void {
    if (!candidate.id) return;
    if (!confirm(`Delete application for ${candidate.firstName} ${candidate.lastName}?`)) return;

    this.candidateService.delete(candidate.id).subscribe({
      next: () => this.candidates.update((list) => list.filter((c) => c.id !== candidate.id)),
      error: () => this.errorMessage.set('Could not delete candidate.')
    });
  }

  logout(): void {
    this.auth.logout();
    this.router.navigate(['/login']);
  }
}
