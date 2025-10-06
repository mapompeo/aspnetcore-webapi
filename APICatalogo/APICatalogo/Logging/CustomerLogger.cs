namespace APICatalogo.Logging;

// Logger customizado que escreve logs em arquivo de texto
public class CustomerLogger : ILogger
{
    readonly string loggerName;
    readonly CustomLoggerProviderConfiguration loggerConfig;

    public CustomerLogger(string name, CustomLoggerProviderConfiguration config)
    {
        loggerName = name;
        loggerConfig = config;
    }

    // Não implementa escopos de log
    public IDisposable BeginScope<TState>(TState state)
    {
        return null;
    }

    // Verifica se o nível de log está habilitado
    public bool IsEnabled(LogLevel logLevel)
    {
        return logLevel >= loggerConfig.LogLevel;
    }


    // Registra a mensagem de log no arquivo
    public void Log<TState>(LogLevel logLevel, EventId eventId, TState state,
            Exception exception, Func<TState, Exception, string> formatter)
    {
        string mensagem = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} [{logLevel}] {eventId.Id} - {formatter(state, exception)}";
        EscreverTextoNoArquivo(mensagem);
    }

    // Escreve o log no arquivo
    private void EscreverTextoNoArquivo(string mensagem)
    {
        string caminhoArquivoLog = @"C:\repositories\aspnetcore-webapi\APICatalogo\APICatalogo\log\log.txt";

        using (StreamWriter streamWriter = new StreamWriter(caminhoArquivoLog, true))
        {
            try
            {
                streamWriter.WriteLine(mensagem);
                streamWriter.Close();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}