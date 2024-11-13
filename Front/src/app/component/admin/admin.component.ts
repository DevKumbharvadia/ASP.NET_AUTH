import { Component, inject, OnInit } from '@angular/core';
import { AdminService } from '../../services/admin.service';
import { JsonPipe } from '@angular/common';
import { HttpClient } from '@angular/common/http';
import { AuthService } from '../../services/auth.service';

@Component({
  selector: 'app-admin',
  standalone: true,
  imports: [],
  templateUrl: './admin.component.html',
  styleUrl: './admin.component.css'
})
export class AdminComponent implements OnInit{
  audits:any;
  adminServices = inject(AdminService);
  authServices = inject(AuthService);
  http = inject(HttpClient);

  ngOnInit(): void {
    this.getAllAudits();
    this.authServices.getUserRoles();
    console.log();
  }

  getAllAudits(){
    this.http.get("https://localhost:7250/api/Audit/GetAllAudits").subscribe((res:any)=>{
      this.audits = res;
    })
  }



}
