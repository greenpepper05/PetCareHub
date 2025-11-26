export interface ClinicSchedule {
  clinicId: number;
  dayOfWeek: number;
  openingTime: string;
  closingTime: string;
  isOpen: boolean;
}