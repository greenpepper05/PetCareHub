export interface createPetServiceHistory {
    ownerId: string;
    petId: number;
    serviceId: number;
    clinicId: number;
    dateOfService: string;
    notes?: string;
    visitType: number;
    appointmentId?: number;
}