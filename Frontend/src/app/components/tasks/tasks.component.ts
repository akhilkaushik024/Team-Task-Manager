import { Component, OnInit } from '@angular/core';
import { TaskService } from '../../services/task.service';
import { ProjectService } from '../../services/project.service';
import { UserService } from '../../services/user.service';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';

@Component({
  selector: 'app-tasks',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './tasks.component.html'
})
export class TasksComponent implements OnInit {
  tasks: any[] = [];
  projects: any[] = [];
  users: any[] = [];
  isAdmin = false;
  showForm = false;
  taskForm: FormGroup;

  constructor(
    private taskService: TaskService, 
    private projectService: ProjectService, 
    private userService: UserService,
    private fb: FormBuilder
  ) {
    const user = JSON.parse(sessionStorage.getItem('user') || '{}');
    this.isAdmin = user.role === 'Admin';
    this.taskForm = this.fb.group({
      title: ['', Validators.required],
      description: [''],
      projectId: ['', Validators.required],
      assignedToId: ['', Validators.required],
      dueDate: [new Date().toISOString().split('T')[0], Validators.required]
    });
  }

  isOverdue(task: any): boolean {
    if (task.status === 'DONE' || !task.dueDate) return false;
    return new Date(task.dueDate) < new Date();
  }

  ngOnInit() {
    this.userService.getUsers().subscribe(res => {
      this.users = res;
      this.loadTasks(); // Load tasks after users so we can map names
    });
    if (this.isAdmin) {
      this.projectService.getProjects().subscribe(res => this.projects = res);
    }
  }

  loadTasks() {
    this.taskService.getTasks().subscribe(res => {
      this.tasks = res.map((t: any) => {
        const assignedUser = this.users.find(u => u.id === t.assignedToId);
        t.assignedToName = assignedUser ? assignedUser.name : 'Unknown User';
        return t;
      });
    });
  }

  onSubmit() {
    if (this.taskForm.valid) {
      this.taskService.createTask(this.taskForm.value).subscribe(() => {
        this.loadTasks();
        this.showForm = false;
        this.taskForm.reset();
      });
    }
  }

  updateStatus(task: any, status: string) {
    this.taskService.updateStatus(task.id, status).subscribe(() => this.loadTasks());
  }

  deleteTask(id: string) {
    if (confirm('Are you sure?')) {
      this.taskService.deleteTask(id).subscribe(() => this.loadTasks());
    }
  }
}

