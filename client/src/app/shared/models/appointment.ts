import { Pet } from "./pet";
import { Service } from "./services";
import { User } from "./user";

export type Appointment = {
    id: number;
    serviceId: number;
    status: string;
    appointmentDate: string;
    petId: number;
    ownerId: string;
    clinicId: number;
    pet?: Pet;
    service?: Service;
    owner?: User;
}