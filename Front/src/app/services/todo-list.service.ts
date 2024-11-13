import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from '../../environment/environment.development';
import { Constant } from '../constant/constant';
import { Todo } from '../model/class';

@Injectable({
  providedIn: 'root'
})
export class TodoListService {

  constructor(private http: HttpClient) {  }

  getAllTodo(): Observable<any>{
    return this.http.get<any>(environment.API_URL_TODO + Constant.API_METHOD.GET_ALL_TODO_ITEM);
  }

  updateTodo(obj: Todo): Observable<any>{
    return this.http.post<any>(environment.API_URL_TODO + Constant.API_METHOD.UPDATE_TODO_ITEM, obj);
  }

  deleteTodo(Id: string){
    return this.http.delete<any>(environment.API_URL_TODO + Constant.API_METHOD.DELETE_TODO_ITEM+Id);
  }

  addTodo(obj: Todo){
    return this.http.post<any>(environment.API_URL_TODO + Constant.API_METHOD.INSERT_TODO_ITEM, obj);
  }
/*
getByUserId()
onUpdate()
onDelete()
addTodo()
*/
}
