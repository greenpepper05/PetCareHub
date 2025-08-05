export interface PetServiceHistory {
    id: number;
    petId: number;
    petName: string;
    serviceName: string;
    clinicName: string;
    dateofService: string;
    notes?: string;
    appointmentId?: number;
}