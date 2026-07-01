export interface Usuario {
  id: string;
  nomeUsuario: string;
  nome: string;
  email: string;
  telefone: string;
  perfil: 'Cliente' | 'Atendente' | 'Administrador';
  criadoEm: string;
}