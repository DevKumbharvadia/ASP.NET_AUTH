import { HttpClient } from '@angular/common/http';
import { Component, inject, OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { Router, RouterLink, RouterLinkActive } from '@angular/router';
import { IRole } from '../../model/interface';
import { AuthService } from '../../services/auth.service';

@Component({
  selector: 'app-register',
  standalone: true,
  imports: [FormsModule, RouterLink, RouterLinkActive],
  templateUrl: './register.component.html',
  styleUrl: './register.component.css'
})
export class RegisterComponent implements OnInit {
  registerObj: any = {
      username: '',
      password: '',
      email: '',
      roleId: ''
  };

  roles: any[] = [];
  router = inject(Router);
  http = inject(HttpClient);

  authService = inject(AuthService);

  ngOnInit(): void {
    this.authService.getAllRoles().subscribe((res:any)=>{
      this.roles = res;
      console.log(res)
    });
  }

  onRegister(){
    debugger;
    console.log(this.registerObj)
    this.http.post("https://localhost:7250/api/User/register",this.registerObj).subscribe((res:any)=>{
      console.log(res);
    })
  }

  

}
