export interface ClinicSchedule {
  clinicId?: number;
  dayOfWeek: string;
  openingTime: string;
  closingTime: string;
  isOpen: boolean;
}