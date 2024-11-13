import { HttpClient } from '@angular/common/http';
import { Injectable, NgZoneOptions, OnInit } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class AdminService implements OnInit {

  AUDIT_DATA: any;

  constructor(private http : HttpClient) { }

  ngOnInit(): void {
      this.getAllAudits();
  }
    
  getAllAudits(){
    this.http.get("https://localhost:7250/api/Audits").subscribe((res:any)=>{
      this.AUDIT_DATA = res;
      console.log(res)
    })
  }

  

}
