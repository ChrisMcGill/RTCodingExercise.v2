export interface AuditLog {
    auditId: string;
    plateId: string;
    action: string;
    details: string;
    timestamp: Date;
}
