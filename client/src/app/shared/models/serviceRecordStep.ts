export interface ServiceRecordStep {
    id : number;
    procedureId: number;
    name: string;
    description: string;
    order: number;
    isCompleted: boolean;
    isSkipped: boolean;
    completedAt: string;
}