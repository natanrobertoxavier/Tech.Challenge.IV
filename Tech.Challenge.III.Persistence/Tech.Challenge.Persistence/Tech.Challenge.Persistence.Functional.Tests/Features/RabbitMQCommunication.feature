#language:pt
Funcionalidade: Consumir mensagem da fila do RabbitMQ

  @aceite @sucesso @regressivo @smoke
  Cenário: Consumir e processar uma mensagem da fila
    Dado que existe uma fila de mensagens chamada "FunctionalTestQueue"
    Quando o produtor publica a mensagem "Olá, RabbitMQ!"
    Então o consumidor deve processar a mensagem "Olá, RabbitMQ!" corretamente
