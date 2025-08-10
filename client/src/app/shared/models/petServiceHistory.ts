export interface PetServiceHistory {
    id: number;
    petId: number;
    petName: string;
    serviceName: string;
    clinicName: string;
    dateOfService: string;
    notes?: string;
    appointmentId?: number;
    visitType: string;
}