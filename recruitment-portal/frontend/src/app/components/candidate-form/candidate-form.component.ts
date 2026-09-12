import { Component, OnInit, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, ReactiveFormsModule } from '@angular/forms';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';
import { Observable } from 'rxjs';
import { CandidateService } from '../../services/candidate.service';
import { Candidate } from '../../models/candidate.model';
import {
  Validators,
  phoneValidator,
  urlValidator,
  notFutureDate,
  minAge,
  expectedCtcNotBelowCurrent
} from '../../shared/validators';

@Component({
  selector: 'app-candidate-form',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, RouterLink],
  templateUrl: './candidate-form.component.html'
})
export class CandidateFormComponent implements OnInit {
  private fb = inject(FormBuilder);
  private candidateService = inject(CandidateService);
  private route = inject(ActivatedRoute);
  private router = inject(Router);

  submitting = false;
  errorMessage = '';
  successMessage = '';
  candidateId: number | null = null;

  // Dropdown option lists
  genders = ['Male', 'Female', 'Other', 'Prefer not to say'];
  maritalStatuses = ['Single', 'Married', 'Divorced', 'Widowed', 'Prefer not to say'];
  employmentTypes = ['Full-time', 'Part-time', 'Contract', 'Freelance', 'Internship'];
  sources = ['Job Board', 'LinkedIn', 'Referral', 'Company Website', 'Social Media', 'Other'];

  form = this.fb.nonNullable.group(
    {
      // Personal details
      firstName: ['', [Validators.required, Validators.minLength(2), Validators.maxLength(100)]],
      middleName: ['', [Validators.maxLength(100)]],
      lastName: ['', [Validators.required, Validators.minLength(2), Validators.maxLength(100)]],
      email: ['', [Validators.required, Validators.email, Validators.maxLength(256)]],
      phone: ['', [phoneValidator]],
      alternatePhone: ['', [phoneValidator]],
      dateOfBirth: ['', [notFutureDate, minAge(16)]],
      gender: [''],
      maritalStatus: [''],
      nationality: ['', [Validators.maxLength(100)]],

      // Address
      address: ['', [Validators.maxLength(500)]],
      city: ['', [Validators.maxLength(100)]],
      state: ['', [Validators.maxLength(100)]],
      country: ['', [Validators.maxLength(100)]],
      postalCode: ['', [Validators.maxLength(20), Validators.pattern(/^[A-Za-z0-9\s-]{3,20}$/)]],

      // Professional details
      positionApplied: ['', [Validators.maxLength(150)]],
      employmentType: [''],
      totalExperience: [null as number | null, [Validators.min(0), Validators.max(60)]],
      currentCompany: ['', [Validators.maxLength(150)]],
      currentCtc: [null as number | null, [Validators.min(0)]],
      expectedCtc: [null as number | null, [Validators.min(0)]],
      noticePeriodDays: [null as number | null, [Validators.min(0), Validators.max(365)]],
      preferredLocation: ['', [Validators.maxLength(150)]],
      willingToRelocate: [false],
      availableFrom: [''],
      highestQualification: ['', [Validators.maxLength(150)]],
      skills: ['', [Validators.maxLength(1000)]],

      // Links & documents
      linkedInUrl: ['', [urlValidator, Validators.maxLength(300)]],
      portfolioUrl: ['', [urlValidator, Validators.maxLength(300)]],
      gitHubUrl: ['', [urlValidator, Validators.maxLength(300)]],
      resumeUrl: ['', [urlValidator, Validators.maxLength(300)]],
      coverLetter: ['', [Validators.maxLength(4000)]],

      // References & meta
      referenceName: ['', [Validators.maxLength(150)]],
      referenceContact: ['', [Validators.maxLength(150)]],
      source: ['']
    },
    { validators: expectedCtcNotBelowCurrent('currentCtc', 'expectedCtc') }
  );

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
          firstName: c.firstName ?? '',
          middleName: c.middleName ?? '',
          lastName: c.lastName ?? '',
          email: c.email ?? '',
          phone: c.phone ?? '',
          alternatePhone: c.alternatePhone ?? '',
          dateOfBirth: c.dateOfBirth ? c.dateOfBirth.substring(0, 10) : '',
          gender: c.gender ?? '',
          maritalStatus: c.maritalStatus ?? '',
          nationality: c.nationality ?? '',
          address: c.address ?? '',
          city: c.city ?? '',
          state: c.state ?? '',
          country: c.country ?? '',
          postalCode: c.postalCode ?? '',
          positionApplied: c.positionApplied ?? '',
          employmentType: c.employmentType ?? '',
          totalExperience: c.totalExperience ?? null,
          currentCompany: c.currentCompany ?? '',
          currentCtc: c.currentCtc ?? null,
          expectedCtc: c.expectedCtc ?? null,
          noticePeriodDays: c.noticePeriodDays ?? null,
          preferredLocation: c.preferredLocation ?? '',
          willingToRelocate: c.willingToRelocate ?? false,
          availableFrom: c.availableFrom ? c.availableFrom.substring(0, 10) : '',
          highestQualification: c.highestQualification ?? '',
          skills: c.skills ?? '',
          linkedInUrl: c.linkedInUrl ?? '',
          portfolioUrl: c.portfolioUrl ?? '',
          gitHubUrl: c.gitHubUrl ?? '',
          resumeUrl: c.resumeUrl ?? '',
          coverLetter: c.coverLetter ?? '',
          referenceName: c.referenceName ?? '',
          referenceContact: c.referenceContact ?? '',
          source: c.source ?? ''
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
      this.errorMessage = 'Please fix the highlighted fields before submitting.';
      return;
    }

    this.submitting = true;
    const payload = this.form.getRawValue() as Candidate;

    const request$: Observable<Candidate | void> = this.isEdit
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
