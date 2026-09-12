import { AbstractControl, ValidationErrors, ValidatorFn, Validators } from '@angular/forms';

/** Phone numbers: digits, spaces, +, -, () — 7 to 20 chars. */
export const PHONE_PATTERN = /^[+]?[0-9\s\-()]{7,20}$/;

/** Basic http(s) URL. */
export const URL_PATTERN = /^https?:\/\/[^\s]+\.[^\s]+$/i;

/** Optional-field pattern validator: skips empty values, applies the pattern otherwise. */
export function optionalPattern(pattern: RegExp): ValidatorFn {
  return (control: AbstractControl): ValidationErrors | null => {
    const value = control.value;
    if (value === null || value === undefined || value === '') {
      return null;
    }
    return pattern.test(value) ? null : { pattern: true };
  };
}

/** Convenience validators for optional phone / URL controls. */
export const phoneValidator: ValidatorFn = optionalPattern(PHONE_PATTERN);
export const urlValidator: ValidatorFn = optionalPattern(URL_PATTERN);

/** The date (if provided) must not be in the future — used for Date of Birth. */
export const notFutureDate: ValidatorFn = (control: AbstractControl): ValidationErrors | null => {
  const value = control.value;
  if (!value) return null;
  const date = new Date(value);
  const today = new Date();
  today.setHours(23, 59, 59, 999);
  return date > today ? { futureDate: true } : null;
};

/** The candidate must be at least `min` years old on the given date of birth. */
export function minAge(min: number): ValidatorFn {
  return (control: AbstractControl): ValidationErrors | null => {
    const value = control.value;
    if (!value) return null;
    const dob = new Date(value);
    const cutoff = new Date();
    cutoff.setFullYear(cutoff.getFullYear() - min);
    return dob > cutoff ? { minAge: { requiredAge: min } } : null;
  };
}

/** Expected CTC (when both provided) should be >= current CTC. Applied at the group level. */
export function expectedCtcNotBelowCurrent(currentKey: string, expectedKey: string): ValidatorFn {
  return (group: AbstractControl): ValidationErrors | null => {
    const current = group.get(currentKey)?.value;
    const expected = group.get(expectedKey)?.value;
    if (current == null || expected == null || current === '' || expected === '') {
      return null;
    }
    return Number(expected) < Number(current) ? { expectedBelowCurrent: true } : null;
  };
}

/** Re-export Validators for a single import site in components. */
export { Validators };
