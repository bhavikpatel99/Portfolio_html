import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';
import { CandidateService } from '../../services/candidate.service';
import { Candidate } from '../../models/candidate.model';

@Component({
  selector: 'app-candidate-form',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, RouterLink],
  templateUrl: './candidate-form.component.html'
})
export class CandidateFormComponent implements OnInit {
  submitting = false;
  errorMessage = '';
  successMessage = '';
  candidateId: number | null = null;

  genders = ['Male', 'Female', 'Other', 'Prefer not to say'];

  form = this.fb.nonNullable.group({
    firstName: ['', [Validators.required, Validators.maxLength(100)]],
    lastName: ['', [Validators.required, Validators.maxLength(100)]],
    email: ['', [Validators.required, Validators.email]],
    phone: [''],
    dateOfBirth: [''],
    gender: [''],
    address: [''],
    city: [''],
    state: [''],
    country: [''],
    postalCode: [''],
    positionApplied: [''],
    totalExperience: [null as number | null],
    currentCompany: [''],
    currentCtc: [null as number | null],
    expectedCtc: [null as number | null],
    noticePeriodDays: [null as number | null],
    highestQualification: [''],
    skills: [''],
    linkedInUrl: [''],
    resumeUrl: [''],
    coverLetter: ['']
  });

  constructor(
    private fb: FormBuilder,
    private candidateService: CandidateService,
    private route: ActivatedRoute,
    private router: Router
  ) {}

  get f() {
    return this.form.controls;
  }

  get isEdit(): boolean {
    return this.candidateId !== null;
  }

  ngOnInit(): void {
    const idParam = this.route.snapshot.paramMap.get('id');
    if (idParam) {
      this.candidateId = Number(idParam);
      this.loadCandidate(this.candidateId);
    }
  }

  private loadCandidate(id: number): void {
    this.candidateService.getById(id).subscribe({
      next: (c) => {
        this.form.patchValue({
          ...c,
          dateOfBirth: c.dateOfBirth ? c.dateOfBirth.substring(0, 10) : ''
        });
      },
      error: () => (this.errorMessage = 'Could not load candidate details.')
    });
  }

  submit(): void {
    this.errorMessage = '';
    this.successMessage = '';
    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }

    this.submitting = true;
    const payload = this.form.getRawValue() as Candidate;

    const request$ = this.isEdit
      ? this.candidateService.update(this.candidateId!, payload)
      : this.candidateService.create(payload);

    request$.subscribe({
      next: () => {
        this.successMessage = this.isEdit
          ? 'Application updated successfully.'
          : 'Application submitted successfully.';
        this.submitting = false;
        setTimeout(() => this.router.navigate(['/dashboard']), 800);
      },
      error: (err) => {
        this.errorMessage = err?.error?.message ?? 'Could not save the application.';
        this.submitting = false;
      }
    });
  }
}
