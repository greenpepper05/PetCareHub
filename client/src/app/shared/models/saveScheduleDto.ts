export interface SaveScheduleDto {
    clinicId: number;
    dayOfWeek: number;
    openingTime: string;
    closingTime: string;
    isOpen: boolean;
}