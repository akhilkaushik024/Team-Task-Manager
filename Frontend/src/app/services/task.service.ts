import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../environments/environment';
import { map } from 'rxjs';

@Injectable({ providedIn: 'root' })
export class TaskService {
  private apiUrl = `${environment.apiUrl}/tasks`;
  constructor(private http: HttpClient) {}

  getTasks(projectId?: string, assignedToId?: string) {
    let params: any = {};
    if (projectId) params.projectId = projectId;
    if (assignedToId) params.assignedToId = assignedToId;
    return this.http.get<any[]>(this.apiUrl, { params }).pipe(
      map(tasks => tasks.map(t => ({ ...t, id: t._id || t.id })))
    );
  }
  createTask(task: any) { return this.http.post<any>(this.apiUrl, task); }
  updateStatus(id: string, status: string) { return this.http.put<any>(`${this.apiUrl}/${id}/status`, { status }); }
  deleteTask(id: string) { return this.http.delete<any>(`${this.apiUrl}/${id}`); }
}
