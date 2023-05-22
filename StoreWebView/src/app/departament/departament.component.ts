import { Component, OnInit } from '@angular/core';
import { environment } from 'src/environments/environment'; 
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-departament',
  templateUrl: './departament.component.html',
  styleUrls: ['./departament.component.css']
})
export class DepartamentComponent implements OnInit {

  constructor(private http:HttpClient) {}
    
  departaments:any=[];
  
    modalTitle="";
    DepartamentId=0;
    DepartamentName="";
   
  ngOnInit(): void {
    this.refreshList();
  }
refreshList(){
  this.http.get<any>(environment.API_URL + 'departament')
  .subscribe(data=>{
    this.departaments=data;
  });
}
addClick(){
  this.modalTitle="Add Departament";
  this.DepartamentId=0;
  this.DepartamentName="";
}
editClick(dep:any){
  this.modalTitle="Edit Departament";
  this.DepartamentId=dep.DepartamentId;
  this.DepartamentName=dep.DepartamentName;
}
createClick(){
  var val={
    DepartamentName:this.DepartamentName
  };

  this.http.post(environment.API_URL+'departament/',val)
  .subscribe(res=>{
    alert(res.toString());
    this.refreshList();
  })
}
updateClick(){
  var val={
    DepartamentId:this.DepartamentId,
    DepartamentName:this.DepartamentName
  };

  this.http.put(environment.API_URL+'departament/',val)
  .subscribe(res=>{
    alert(res.toString());
    this.refreshList();
  })
}
deleteClick(id:any){
 if (confirm('Are you sure?')){
  this.http.delete(environment.API_URL+'departament/' + id)
  .subscribe(res=>{
    alert(res.toString());
    this.refreshList();
  })
 }

  
}

}
