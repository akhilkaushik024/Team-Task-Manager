import { Component, OnInit } from '@angular/core';
import { ProjectService } from '../../services/project.service';
import { AuthService } from '../../services/auth.service';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';

@Component({
  selector: 'app-projects',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './projects.component.html'
})
export class ProjectsComponent implements OnInit {
  projects: any[] = [];
  isAdmin = false;
  showForm = false;
  projectForm: FormGroup;

  constructor(private projectService: ProjectService, private auth: AuthService, private fb: FormBuilder) {
    this.isAdmin = JSON.parse(sessionStorage.getItem('user') || '{}').role === 'Admin';
    this.projectForm = this.fb.group({
      name: ['', Validators.required],
      description: ['']
    });
  }

  ngOnInit() { this.loadProjects(); }

  loadProjects() {
    this.projectService.getProjects().subscribe(res => this.projects = res);
  }

  onSubmit() {
    if (this.projectForm.valid) {
      this.projectService.createProject(this.projectForm.value).subscribe(() => {
        this.loadProjects();
        this.showForm = false;
        this.projectForm.reset();
      });
    }
  }

  deleteProject(id: string) {
    if (confirm('Are you sure you want to delete this project and all its tasks?')) {
      this.projectService.deleteProject(id).subscribe(() => {
        this.loadProjects();
      });
    }
  }
}
