export interface Staff {
    id: number;
    firstName: string;
    lastName: string;
    email: string;
    phoneNumber: string;
    staffRole: string;
    pictureUrl?: string;
    clinicId: number;
    clinicName: string;
}