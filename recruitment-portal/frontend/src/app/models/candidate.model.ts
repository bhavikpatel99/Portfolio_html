export interface Candidate {
  id?: number;
  userId?: number | null;
  firstName: string;
  lastName: string;
  email: string;
  phone?: string | null;
  dateOfBirth?: string | null;
  gender?: string | null;
  address?: string | null;
  city?: string | null;
  state?: string | null;
  country?: string | null;
  postalCode?: string | null;
  positionApplied?: string | null;
  totalExperience?: number | null;
  currentCompany?: string | null;
  currentCtc?: number | null;
  expectedCtc?: number | null;
  noticePeriodDays?: number | null;
  highestQualification?: string | null;
  skills?: string | null;
  linkedInUrl?: string | null;
  resumeUrl?: string | null;
  coverLetter?: string | null;
  status?: string;
  createdAt?: string;
  updatedAt?: string | null;
}
