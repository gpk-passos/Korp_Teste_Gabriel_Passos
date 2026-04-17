import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class FaturamentoService {

  private readonly API = 'http://localhost:5017/api/NotaFiscal';

  constructor(private http: HttpClient) { }

  listar(): Observable<any[]> {
    return this.http.get<any[]>(this.API);
  }

  cadastrar(nota: any): Observable<any> {
    return this.http.post(this.API, nota);
  }

  
 adicionarItem(notaId: number, item: any): Observable<any> {
    return this.http.put(`${this.API}/adicionar-item/${notaId}`, item, { responseType: 'text' });
  }

  imprimir(id: number): Observable<any> {
    return this.http.post(`${this.API}/imprimir/${id}`, {});
  }

  excluir(id: number): Observable<any> {
    return this.http.delete(`${this.API}/${id}`, { responseType: 'text' });
  }

  removerItemDaNota(notaId: number, produtoId: number): Observable<any> {
    return this.http.delete(`${this.API}/remover-item/${notaId}/${produtoId}`, { responseType: 'text' });
  }
  
}