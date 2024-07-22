Este projeto utiliza as seguintes tecnologias e padrões arquiteturais:

CQRS (Command Query Responsibility Segregation)
Kafka
Unit of Work
Repositório Genérico
Swagger

1. **Exportar a Collection do Postman**
   - Exporte a coleção do Postman e salve-a dentro da pasta `delivery-dispatch.postman_collection.json`.

2. **Subir a Aplicação com Docker Compose**
   - Abra o CMD dentro da pasta do projeto e execute o seguinte comando:
     docker-compose up

3. **Diagrama da Arquitetura**
   - Um diagrama da arquitetura está disponível na raiz do projeto. Ele foi criado no Draw.IO e pode ser usado para entender a estrutura do sistema.

## Estrutura do Projeto

- `delivery-dispatch.postman_collection.json`: Coleção do Postman contendo as APIs utilizadas no projeto.
- `docker-compose.yml`: Arquivo de configuração do Docker Compose para subir a aplicação.
- `architecture-diagram.drawio`: Diagrama da arquitetura do projeto, localizado na raiz do repositório.

## Pré-requisitos

- Docker e Docker Compose instalados.
- Postman para importação da coleção de APIs.

## Passos para Execução

1. **Importar a Coleção do Postman**
   - Abra o Postman e importe a coleção localizada em `delivery-dispatch.postman_collection.json`.

2. **Subir a Aplicação**
   - Navegue até a pasta do projeto no CMD.
   - Execute o comando:
     docker-compose up

3. **Visualizar o Diagrama**
   - Utilize o Draw.IO para abrir o arquivo `architecture-diagram.drawio` e visualizar o diagrama da arquitetura.
