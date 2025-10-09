using Microsoft.Extensions.Logging;
using System;
using System.IO;

namespace APICatalogo.Logging
{
    public class CustomerLogger : ILogger
    {
        private readonly string _loggerName;
        private readonly CustomLoggerProviderConfiguration _loggerConfig;

        public CustomerLogger(string name, CustomLoggerProviderConfiguration config)
        {
            _loggerName = name;
            _loggerConfig = config;
        }

        public IDisposable? BeginScope<TState>(TState state) where TState : notnull
        {
            // Não há escopo sendo usado aqui, então retornamos null
            return null;
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            return logLevel >= _loggerConfig.LogLevel;
        }

        public void Log<TState>(
            LogLevel logLevel,
            EventId eventId,
            TState state,
            Exception? exception, // <-- aqui é nullable
            Func<TState, Exception?, string> formatter // <-- aqui também
        )
        {
            if (!IsEnabled(logLevel))
                return;

            if (formatter == null)
                throw new ArgumentNullException(nameof(formatter));

            string mensagem =
                $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} [{logLevel}] {eventId.Id} - {formatter(state, exception)}";

            EscreverTextoNoArquivo(mensagem);
        }

        private void EscreverTextoNoArquivo(string mensagem, string? configuredPath = null)
        {
            string caminhoArquivoLog = configuredPath ??
                                       Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Log", "log.txt");

            string? dir = Path.GetDirectoryName(caminhoArquivoLog);
            if (!string.IsNullOrEmpty(dir))
            {
                Directory.CreateDirectory(dir);
            }

            try
            {
                using var streamWriter = new StreamWriter(caminhoArquivoLog, append: true);
                streamWriter.WriteLine(mensagem);
            }
            catch (Exception ex)
            {
                // Log interno — apenas para depuração
                Console.WriteLine($"Logger internal error: {ex.Message}");
            }
        }
    }
}
