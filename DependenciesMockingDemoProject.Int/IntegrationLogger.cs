using System;
using System.Collections.Concurrent;
using System.IO;
using DependenciesMockingDemoProject.Test;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Logging.Console;
using NUnit.Framework;

namespace DependenciesMockingDemoProject.Int
{
    internal static class IntegrationLogger
    {
        internal static TextWriter Output = TestContext.Out;

        internal static readonly ILoggerFactory Factory = new LoggerFactory(new[]
        {
            new IntegrationLoggerProvider()
        }, new LoggerFilterOptions
        {
            MinLevel = LogLevel.Warning,
            Rules =
            {
                new LoggerFilterRule("NUnit", "DependenciesMockingDemoProject.Web", LogLevel.Trace, null),
                new LoggerFilterRule("NUnit", "DependenciesMockingDemoProject.Client", LogLevel.Trace, null),
            }
        });

        [ProviderAlias("NUnit")]
        private class IntegrationLoggerProvider : ILoggerProvider
        {
            private readonly ConcurrentDictionary<string, ILogger> _loggers = new();

            public void Dispose()
            {
            }

            public ILogger CreateLogger(string categoryName) =>
                _loggers.GetOrAdd(categoryName, s => new NUnitLogger(s));
        }

        private class NUnitLogger : ILogger
        {
            private class NullScope : IDisposable
            {
                internal static readonly NullScope Instance = new();

                public void Dispose()
                {
                }
            }

            [ThreadStatic] private static StringWriter _stringWriter;
            private static readonly ConsoleFormatter Formatter = new TestConsoleFormatter();
            private readonly string _name;

            public NUnitLogger(string name)
            {
                _name = name;
            }

            public IDisposable BeginScope<TState>(TState state)
            {
                return NullScope.Instance;
            }

            public bool IsEnabled(LogLevel logLevel)
            {
                return logLevel != LogLevel.None;
            }

            public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception,
                Func<TState, Exception, string> formatter)
            {
                _stringWriter ??= new StringWriter();
                var logEntry = new LogEntry<TState>(logLevel, _name, eventId, state, exception, formatter);
                Formatter.Write(in logEntry, null, _stringWriter);

                var sb = _stringWriter.GetStringBuilder();
                if (sb.Length == 0)
                {
                    return;
                }

                string computedAnsiString = sb.ToString();
                sb.Clear();

                Output.Write(computedAnsiString);
            }
        }
    }
}