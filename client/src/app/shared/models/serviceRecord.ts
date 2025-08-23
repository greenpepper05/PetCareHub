export interface ServiceRecord {
    id: number;
    petId: number;
    petName: string;
    serviceName: string;
    clinicName: string;
    dateOfService: string;
    notes?: string;
    appointmentId?: number;
    visitType: string;
    status: string;
}