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
