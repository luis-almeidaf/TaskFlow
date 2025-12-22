# TaskFlow API 🚀

O **TaskFlow** é uma API de gerenciamento de tarefas e quadros (estilo Kanban) desenvolvida com foco em escalabilidade e manutenibilidade, utilizando os princípios de **Clean Architecture** e **DDD**.

## 🎯 Objetivo do Projeto

Este repositório foi desenvolvido para fins de estudo e demonstração técnica. O foco principal foi aplicar padrões de arquitetura modernos e práticas recomendadas de desenvolvimento com .NET 9, simulando um cenário real de gerenciamento de tarefas.


## 🛠️ Tecnologias Utilizadas

* **Linguagem:** C# (.NET 9)
* **Arquitetura:** Clean Architecture / DDD
* **Banco de Dados:** MySQL
* **Comunicação:** MediatR (CQRS)
* **Autenticação:** JWT (Access Token + Refresh Token)
* **Validações:** FluentValidation
* **Mapeamento:** Mapster
* **Testes:** xUnit, FluentAssertions e Bogus

---

## ⚙️ Como Executar o Projeto

### Pré-requisitos

* [.NET SDK 9.0](https://dotnet.microsoft.com/en-us/download/dotnet/9.0)
* [MySQL](https://www.mysql.com/)

### Passo a Passo

1. **Clonar o repositório:**
```bash
git clone https://github.com/luis-almeidaf/TaskFlow.git
cd taskflow
```

2. **Configurar o banco de dados:**
   No projeto `TaskFlow.API`, localize o arquivo `appsettings.Development.json` e atualize a sua `ConnectionString` do MySQL e as chaves do JWT:
```json
"ConnectionStrings": {
  "DefaultConnection": "Server=localhost;Database=TaskFlowDb;Uid=root;Pwd=suasenha;"
},
"Settings": {
  "Jwt": {
    "SigningKey": "sua_chave_secreta_com_pelo_menos_32_caracteres",
    "ExpiresMinutes": 15
  }
}
```

3. **Rodar as Migrations:**
```bash
cd src
dotnet ef database update --project TaskFlow.Infrastructure --startup-project TaskFlow.API
```


4. **Executar a API:**
```bash
cd src
dotnet run --project TaskFlow.API
```
---

## 📚 Documentação da API

Uma vez que a aplicação esteja rodando, você pode acessar a documentação interativa do **Swagger** em:

`http://localhost:5117/swagger/index.html`

Lá você encontrará todos os endpoints de:

* **Auth:** Login, Logout e Refresh Token.
* **User:** Cadastro, atualização e remoção.
* **Boards:** Criação e gerenciamento de quadros.
* **Cards:** Movimentação e edição de tarefas.

---

## 🧪 Testes

Para executar a suíte de testes (Unitários e de Integração):

```bash
cd Taskflow.Tests
dotnet test
```
