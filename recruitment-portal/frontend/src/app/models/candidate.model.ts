export interface Candidate {
  id?: number;
  userId?: number | null;

  // Personal details
  firstName: string;
  middleName?: string | null;
  lastName: string;
  email: string;
  phone?: string | null;
  alternatePhone?: string | null;
  dateOfBirth?: string | null;
  gender?: string | null;
  maritalStatus?: string | null;
  nationality?: string | null;

  // Address
  address?: string | null;
  city?: string | null;
  state?: string | null;
  country?: string | null;
  postalCode?: string | null;

  // Professional details
  positionApplied?: string | null;
  employmentType?: string | null;
  totalExperience?: number | null;
  currentCompany?: string | null;
  currentCtc?: number | null;
  expectedCtc?: number | null;
  noticePeriodDays?: number | null;
  preferredLocation?: string | null;
  willingToRelocate?: boolean | null;
  availableFrom?: string | null;
  highestQualification?: string | null;
  skills?: string | null;

  // Links & documents
  linkedInUrl?: string | null;
  portfolioUrl?: string | null;
  gitHubUrl?: string | null;
  resumeUrl?: string | null;
  coverLetter?: string | null;

  // References & meta
  referenceName?: string | null;
  referenceContact?: string | null;
  source?: string | null;
  status?: string;
  createdAt?: string;
  updatedAt?: string | null;
}
