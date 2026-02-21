# Clean Architecture

```
DapperCrudClean/
├── DapperCrudClean.Api
├── DapperCrudClean.Application
├── DapperCrudClean.Domain
├── DapperCrudClean.Infrastructure
└── DapperCrudClean.Tests
```


## Regra de Dependência (Inward Dependency) - 1

Na Clean Architecture, as dependências sempre apontam para dentro.

A regra é:

Camadas externas podem depender das internas,
mas camadas internas nunca dependem das externas.

Estrutura típica:

```
API → Infrastructure → Application → Domain
```

Domain não depende de ninguém.
Application depende apenas de Domain.
Infrastructure depende de Application e Domain.
API depende de Application.

Isso garante:

Isolamento de regra de negócio
Independência de framework
Alta testabilidade
Facilidade de substituição de tecnologia

Isso é o que o Robert C. Martin define na Clean Architecture.


## Por que Application NÃO conhece Dapper? - 2

Porque Dapper é detalhe de infraestrutura.

Dapper é um micro-ORM. Ele resolve acesso a dados.

Mas a camada Application representa casos de uso.

Se Application conhecer Dapper:
Você acopla regra de negócio ao banco
Fica impossível trocar tecnologia
Seus testes passam a depender de infraestrutura
Você viola o princípio de inversão de dependência (DIP)
Por isso a Application define apenas:

public interface IProductRepository

Quem implementa isso é a Infrastructure.
Application depende de abstração, não de implementação.

Isso é SOLID na prática.


## Por que Infrastructure depende de Application? - 3

Porque Infrastructure implementa as interfaces definidas na Application.

Exemplo:
```
public class ProductRepository : IProductRepository

```

IProductRepository está na Application.
Então Infrastructure precisa conhecê-la para implementar.
Mas Application NÃO conhece Infrastructure.

Isso cria inversão de controle:

Application define o contrato
Infrastructure fornece a implementação
API injeta via DI

Isso é o Dependency Inversion Principle funcionando corretamente.


## Como trocar SQL Server por PostgreSQL sem mexer no domínio? - 4

Hoje usamos:

SQL Server
Microsoft.Data.SqlClient

Se eu quiser trocar para:

PostgreSQL
Npgsql

Eu só preciso:

Criar nova implementação:
```
public class ProductRepositoryPostgres : IProductRepository 
```
Trocar a connection string

Registrar no DI:
```
builder.Services.AddScoped<IProductRepository, ProductRepositoryPostgres>();
```

E pronto.

✔ Domain não muda
✔ Application não muda
✔ Controllers não mudam

Só a Infrastructure muda.

Isso é desacoplamento real.



Na Clean Architecture, aplicamos a regra de dependência onde as camadas externas dependem das internas. 
O domínio é isolado e não conhece frameworks ou banco de dados. 
A camada Application define contratos e regras de negócio, enquanto a Infrastructure implementa esses contratos usando tecnologias como Dapper. 
Isso permite trocar SQL Server por PostgreSQL apenas alterando a implementação do repositório, sem impactar domínio ou casos de uso.
Esse design segue o DIP do SOLID e garante alta testabilidade e baixo acoplamento.

