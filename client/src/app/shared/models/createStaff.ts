export interface CreateStaff {
    firstName: string;
    lastName: string;
    email: string;
    phoneNumber: string;
    staffRole: string;
    pictureUrl?: string;
    clinicId: number;
}