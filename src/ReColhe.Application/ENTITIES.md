# Entidades do Sistema - ReColhe

## 📋 Tabelas do Banco de Dados

### 1. Usuarios
**Descrição:** Armazena os usuários do sistema

| Campo | Tipo | Obrigatório | Descrição |
|-------|------|-------------|-----------|
| UsuarioId | int | ✅ | Chave primária, auto-increment |
| Nome | string | ✅ | Nome completo do usuário |
| Email | string | ✅ | E-mail único do usuário |
| SenhaHash | string | ✅ | Senha criptografada |
| TipoUsuarioId | int | ✅ | Chave estrangeira para TipoUsuario |
| EmpresaId | int | ❌ | Chave estrangeira para Empresa (opcional) |

**Relacionamentos:**
- Pertence a **1 TipoUsuario** (obrigatório)
- Pertence a **0 ou 1 Empresa** (opcional)

---

### 2. Empresas
**Descrição:** Armazena as empresas cadastradas

| Campo | Tipo | Obrigatório | Descrição |
|-------|------|-------------|-----------|
| EmpresaId | int | ✅ | Chave primária, auto-increment |
| NomeFantasia | string | ✅ | Nome fantasia da empresa |
| CNPJ | string | ✅ | CNPJ único da empresa |
| EmailContato | string | ❌ | E-mail para contato |
| TelefoneContato | string | ❌ | Telefone para contato |

**Relacionamentos:**
- Tem **1 ou N Usuarios** (opcional)

---

### 3. TiposUsuario
**Descrição:** Define os tipos/perfis de usuário

| Campo | Tipo | Obrigatório | Descrição |
|-------|------|-------------|-----------|
| TipoUsuarioId | int | ✅ | Chave primária, auto-increment |
| Nome | string | ✅ | Nome do tipo (ex: Admin, Comum, Colaborador) |

**Relacionamentos:**
- Tem **1 ou N Usuarios** (obrigatório)

## 🔗 Relacionamentos

### Usuario → TipoUsuario
- **Tipo:** Muitos-para-Um (M:1)
- **Obrigatório:** ✅ Sim
- **Delete:** Cascade
- **Descrição:** Todo usuário deve ter um tipo de usuário

### Usuario → Empresa  
- **Tipo:** Muitos-para-Um (M:1)
- **Obrigatório:** ❌ Não
- **Delete:** Restrict
- **Descrição:** Usuário pode ou não pertencer a uma empresa

## 🗂️ Exemplos de Dados

### TiposUsuario:
```sql
INSERT INTO TiposUsuario (Nome) VALUES 
('Administrador'),
('Colaborador'), 
('Comum');