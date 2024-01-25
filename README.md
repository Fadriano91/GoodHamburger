# GoodHamburger
Este é um projeto com uso do .NET 8 e EntityFramework 8 para gerenciar pedidos de hambúrgueres.

## Pré-requisitos

Certifique-se de ter o SDK .NET 8 instalado em sua máquina. Você pode baixá-lo em [https://dotnet.microsoft.com/download/dotnet/8.0](https://dotnet.microsoft.com/download/dotnet/8.0).

## Configuração

1. Clone o repositório em sua máquina local:

    ```bash
    git clone https://github.com/seu-usuario/good-hamburger.git
    ```

2. Abra o projeto no Visual Studio.

3. Configure a string de conexão no arquivo `appsettings.json`.

4. No terminal do Visual Studio (Ctrl+`), execute o seguinte comando para aplicar as migrações:

    ```bash
    dotnet ef database update
    ```

5. Inicie o projeto clicando em "Depurar" (F5).

## Funcionalidades

- **Criar Pedido (Order):**
  - Utilize a rota `POST /api/Order/CreateOrder` fornecendo o ID do lanche (`SandwichId`) e os IDs dos extras (`ExtrasId`). Se mais de um extra, separe com vírgula.

- **Listar Pedidos:**
  - Utilize a rota `GET /api/Order/SearchAllOrders` para listar todos os pedidos.

- **Atualizar Pedido:**
  - Utilize a rota `PUT /api/Order/UpdateOrder/{orderId}` fornecendo os novos IDs do lanche e dos extras.

- **Excluir Pedido:**
  - Utilize a rota `DELETE /api/Order/DeleteOrder/{orderId}` para excluir um pedido pelo ID.

- **Pesquisar Pedido por ID:**
  - Utilize a rota `GET /api/Order/SearchOrderById/{orderId}` para obter detalhes de um pedido específico.

- **Adicionar Sandwich:**
  - Utilize as rotas `POST /api/Sandwich/CreateSandwich` para adicionar novos lanches

- **Listar todos os Sandwiches:**
  - Utilize as rotas `GET /api/Sandwich/SearchAllSandwiches` para listar todos os lanches

- **Adicionar Extra:**
  - Utilize as rotas `POST /api/Extra/CreateExtra` para adicionar novos extras.

- **Listar todos os Extras:**
  - Utilize as rotas `GET /api/Extra/SearchAllExtras` para listar todos os extras.
    
- **Listar todos os Lanches e Extras:**
  - Utilize as rotas `GET /api/Sandwich/SearchAllSandwichesAndExtras` para listar todos os lanches e extras.

Consulte a documentação completa da API usando a interface Swagger em [http://localhost:5140/swagger](http://localhost:5140/swagger).

