import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../environments/environment';
import { map } from 'rxjs';

@Injectable({ providedIn: 'root' })
export class ProjectService {
  private apiUrl = `${environment.apiUrl}/projects`;
  constructor(private http: HttpClient) {}

  getProjects() { 
    return this.http.get<any[]>(this.apiUrl).pipe(
      map(projects => projects.map(p => ({ ...p, id: p._id || p.id })))
    ); 
  }
  createProject(project: any) { return this.http.post<any>(this.apiUrl, project); }
  deleteProject(id: string) { return this.http.delete<any>(`${this.apiUrl}/${id}`); }
}
