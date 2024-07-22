# Projeto Delivery Dispatch

Este projeto utiliza as seguintes tecnologias e padrões arquiteturais:

- **CQRS (Command Query Responsibility Segregation)**
- **Kafka**
- **Unit of Work**
- **Repositório Genérico**
- **Swagger**
- **Arquitetura Onion**
- **FluentValidation**

## Estrutura do Projeto

- `delivery-dispatch.postman_collection.json`: Coleção do Postman contendo as APIs utilizadas no projeto.
- `docker-compose.yml`: Arquivo de configuração do Docker Compose para subir a aplicação.
- `architecture-diagram.drawio`: Diagrama da arquitetura do projeto, localizado na raiz do repositório.

## Pré-requisitos

Antes de iniciar, certifique-se de ter os seguintes itens instalados:

- Docker e Docker Compose
- Postman para importação da coleção de APIs

## Passos para Execução

### 1. Importar a Coleção do Postman

- Abra o Postman.
- Importe a coleção localizada em `delivery-dispatch.postman_collection.json`.

### 2. Subir a Aplicação com Docker Compose

- Navegue até a pasta do projeto no terminal.
- Execute o comando:
  ```sh
  docker-compose up
