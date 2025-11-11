export interface RegisterWithOtpRequest {
    firstName: string;
    lastName: string;
    email: string;
    contact: string;
    password: string;
    clinicId: number;
    otp: string;
}