export interface ServiceRecordStep {
    id : number;
    procedureId: number;
    name: string;
    description: string;
    order: number;
    isCompleted: string;
    isSkipped: string;
    completedAt: string;
}