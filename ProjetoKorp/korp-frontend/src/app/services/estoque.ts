import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

@Injectable({ providedIn: 'root' })
export class EstoqueService {
  private readonly API = 'https://localhost:7023/api/produtos';

  constructor(private http: HttpClient) { }

  getProdutos(): Observable<any[]> {
    return this.http.get<any[]>(this.API);
  }

  cadastrar(produto: any): Observable<any> {
    return this.http.post<any>(this.API, produto);
  }

  atualizar(id: number, produto: any): Observable<any> {
    return this.http.put<any>(`${this.API}/${id}`, produto);
  }
  
  excluir(id: number): Observable<any> {
    return this.http.delete<any>(`${this.API}/${id}`);
  }
}