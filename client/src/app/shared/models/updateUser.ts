export interface UpdateUser {
    firstName: string | null;
    lastName: string | null;
    email: string | null;
    contact: string | null;
    oldPassword: string | null;
    newPassword: string | null;
    // confirmPassword: string | null;
}