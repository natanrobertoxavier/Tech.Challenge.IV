using Tech.Challenge.Persistence.Functional.Tests.Shared.RabbitMq;
using TechTalk.SpecFlow;

namespace Tech.Challenge.Persistence.Functional.Tests.Steps;
[Binding]
public class RabbitMQCommunicationSteps
{
    private Publisher _publisher;
    private Consumer _consumer;
    private bool _mensagemProcessada;
    private string _queueName = string.Empty;

    public RabbitMQCommunicationSteps()
    {
        _publisher = new Publisher();
        _consumer = new Consumer();
    }

    [Given(@"que existe uma fila de mensagens chamada ""(.*)""")]
    public void DadoQueExisteUmaFilaDeMensagensChamada(string queueName)
    {
        _queueName = $"{queueName}_{Guid.NewGuid()}";
        _publisher.ConfigureQueue(_queueName);
        Console.WriteLine($"Fila {_queueName} está configurada.");
    }

    [When(@"o produtor publica a mensagem ""(.*)""")]
    public void QuandoOProdutorPublicaAMensagem(string message)
    {
        _publisher.PublishMessage(message);
    }

    [Then(@"o consumidor deve processar a mensagem ""(.*)"" corretamente")]
    public void EntaoOConsumidorDeveProcessarAMensagemCorretamente(string expectedMessage)
    {
        _consumer.OnMessageReceived += (message) =>
        {
            Console.WriteLine($"Mensagem Processada: {message}");
            _mensagemProcessada = message == expectedMessage;
        };

        _consumer.StartConsuming(_queueName);

        int retries = 50;
        while (!_mensagemProcessada && retries > 0)
        {
            Thread.Sleep(100);
            retries--;
        }

        Assert.True(_mensagemProcessada, "A mensagem não foi processada corretamente.");
    }
}
