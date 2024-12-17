# Stock Tracker App

## Descrição  

Esta aplicação foi desenvolvida para permitir que os usuários se cadastrem em diversas ações e sejam notificados sempre que elas atingirem um valor de compra ou venda definido por eles. Dessa forma, os usuários podem tomar decisões informadas e rápidas sobre suas transações no mercado financeiro.  

## Arquitetura  

A solução é composta por três serviços principais, que trabalham de forma integrada para oferecer uma experiência completa e eficiente:  

### 1. **StocksApi**  
Este serviço é responsável pela interação inicial com os usuários, oferecendo funcionalidades como:  
- Inscrição em ações de interesse;  
- Cancelamento de inscrições;  
- Consulta das ações nas quais o usuário está inscrito.  

Sempre que um usuário se inscreve ou cancela a inscrição em uma ação, o serviço emite um evento para o **StocksMonitor**, informando a alteração.  

### 2. **StocksMonitor**  
Este serviço realiza o monitoramento contínuo dos preços das ações. Suas responsabilidades incluem:  
- Consumir api externa de cotação em tempo real de ações e verificar se os preços das ações monitoradas estão dentro dos intervalos definidos pelos usuários;  
- Emitir um evento **StockAlertTriggered** para o **StocksNotification** quando uma ação atingir o valor configurado pelo usuário.  

### 3. **StocksNotification**  
Encaminha notificações personalizadas para os usuários. Este serviço consome os eventos gerados pelo **StocksMonitor** e executa as seguintes tarefas:  
- Envio de e-mails alertando os usuários sobre a oportunidade de compra ou venda da ação monitorada;  

## Fluxo de Funcionamento  

1. O usuário utiliza o **StocksApi** para se inscrever ou cancelar inscrições em ações.  
2. O **StocksApi** emite eventos para o **StocksMonitor**, atualizando a lista de ações e critérios associados aos usuários.  
3. O **StocksMonitor** monitora os preços e, ao detectar que uma ação atingiu o intervalo definido, gera um evento **StockAlertTriggered**.  
4. O **StocksNotification** consome o evento e envia um e-mail para o usuário, recomendando a compra ou venda da ação correspondente.  

<p align="center">
  <img src="https://github.com/user-attachments/assets/5566b1a4-5894-4e8c-8ea7-1a48abba5a06" alt="Descrição da imagem">
</p>

## Configuração Local  

### StockApi  

Para executar o serviço **StockApi**, é necessário que os serviços do **PostgreSQL** e **RabbitMQ** estejam em execução.  

Uma alternativa simples é utilizar contêineres para rodar ambos os serviços. Basta executar o comando abaixo na raiz do projeto, onde está localizado o arquivo `docker-compose.yml`:  

```bash
docker-compose up
```

Após isso, será necessário criar o arquivo `appsettings.json` com as credenciais de acesso ao banco de dados e ao RabbitMQ.

Como referência, você pode usar o arquivo de exemplo abaixo:

Arquivo de exemplo `appsettings_example.json`:

```
{
  "ConnectionStrings": {
    "StocksDatabase": "Host=localhost;Port=5432;Database=StocksDb;Username=postgres;Password=postgres"
  },
  "MessageBroker": {
    "Host": "amqp://localhost:5672",
    "UserName": "guest",
    "Password": "guest",
    "SubscribedTopic": "subscribed-topic",
    "UnsubscribedTopic": "unsubscribed-topic",
    "SubscribedEventQueue": "subscribed-queue"
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*"
}
```
Após configurar os serviços e credenciais, é necessário executar as migrations para criar as tabelas no banco de dados.  

O projeto utiliza o **Entity Framework** como ORM para gerenciar as migrations e o esquema do banco. Siga os comandos abaixo para aplicar as migrations:  

1. Adicionar a migration inicial:  
 ```bash
 dotnet ef migrations add InitialMigration
   ```
2. Atualizar o banco de dados com a migration criada:
```bash
 dotnet ef database update
 ```
Esses comandos irão gerar as tabelas necessárias no banco configurado no appsettings.json. Certifique-se de que o PostgreSQL está em execução antes de rodar os comandos.

#### Endpoints
Todos os endpoints estão documentados utilizando Swagger e podem ser acessados no seguinte endereço:

http://localhost:5129/swagger/index.html

### StockMonitor  

Para configurar o serviço **StockMonitor**, será necessário criar o arquivo `appsettings.json` com as credenciais de acesso ao **RabbitMQ** e ao **Redis**. O serviço **Redis** também estará disponível após executar o comando `docker-compose up`.  

Além disso, o **StockMonitor** utiliza a API do [Alpha Vantage](https://www.alphavantage.co/documentation/) para coletar informações sobre ações. Para habilitar essa funcionalidade, é necessário:  
1. Gerar uma **API Key** no site do Alpha Vantage ([ApiKey Gratuita](https://www.alphavantage.co/support/#api-key).  
2. Inserir a API Key nas configurações do arquivo `appsettings.json`.  

Arquivo de exemplo `appsettings_example.json`:

```
{
  "ConnectionStrings": {
    "Redis": "localhost:6379"
  },
  "AlphaVantage": {
    "ApiKey": "your_alphavantage_api_key"
  },
  "MessageBroker": {
    "Host": "amqp://localhost:5672",
    "UserName": "guest",
    "Password": "guest",
    "SubscribedTopic": "subscribed-topic",
    "SubscribedEventQueue": "subscribed-queue",
    "UnsubscribedTopic": "unsubscribed-topic",
    "UnsubscribedEventQueue": "unsubscribed-queue",
    "PriceAlertTriggeredTopic": "price-alert-triggered-topic"
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.Hosting.Lifetime": "Information"
    }
  }
}
```
### StocksNotification

Para configurar o serviço **StocksNotification**, será necessário criar o arquivo `appsettings.json` com as credenciais de acesso ao **RabbitMQ**.  

Arquivo de exemplo `appsettings_example.json`:

```
{
  "MessageBroker": {
    "Host": "amqp://localhost:5672",
    "UserName": "guest",
    "Password": "guest",
    "PriceAlertTopic": "price-alert-triggered-topic",
    "PriceAlertQueue": "price-alert-triggered-queue"
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.Hosting.Lifetime": "Information"
    }
  }
}
```

Além disso, é necessário configurar as credenciais de e-mail para o envio de notificações. Para isso, crie um arquivo chamado emailsettings.txt na raiz do projeto com o seguinte conteúdo:

Arquivo de exemplo `emailsettings.txt`:
```
{
    "SmtpServer": "smtp.gmail.com",
    "SmtpPort": 587,
    "SenderEmail": "your-email",
    "Username": "your-email",
    "Password": "your-app-password"
}
```

Com todos esses passos concluídos, você estará pronto para rodar toda a aplicação localmente.
