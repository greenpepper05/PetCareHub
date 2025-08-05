export type Appointment = {
  id: number;
  appointmentDate: string;
  status: string;
  notes: string | null;
  clinicId: number;
  clinicName: string | null;
  ownerId: string;
  ownerEmail: string;

  petId: number;
  petName: string;
  petBreed: string;
  petSpecies: string;
  petGender: string;
  petBirthdate: string;

  serviceId: number;
  serviceName: string;
}