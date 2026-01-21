import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { AuditLog } from '../models/audit-log';

@Injectable({ providedIn: 'root' })
export class AuditService {
    private apiUrl = 'http://localhost:5101/api/audit'; // development environment

    constructor(private http: HttpClient) {}

    getHistory(plateId: string): Observable<AuditLog[]> {
        return this.http.get<AuditLog[]>(`${this.apiUrl}/${plateId}`);
    }

    // Trigger the browser download
    exportJson(plateId: string): void {
        window.open(`${this.apiUrl}/${plateId}/export`, '_blank');
    }
}
