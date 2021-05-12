using System;
using System.IO;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Logging.Console;
using Microsoft.Extensions.Options;

namespace DependenciesMockingDemoProject.Test
{
    internal static class TestLogger
    {
        internal static readonly ILoggerFactory Factory = new LoggerFactory(new[]
        {
            new ConsoleLoggerProvider(new OptionsMonitor<ConsoleLoggerOptions>(
                new OptionsFactory<ConsoleLoggerOptions>(new IConfigureOptions<ConsoleLoggerOptions>[]
                    {
                        new TestFormatterConfigureOptions()
                    }, new IPostConfigureOptions<ConsoleLoggerOptions>[0],
                    new IValidateOptions<ConsoleLoggerOptions>[0]),
                new IOptionsChangeTokenSource<ConsoleLoggerOptions>[0],
                new OptionsCache<ConsoleLoggerOptions>()), new[] {new TestConsoleFormatter()})
        }, new LoggerFilterOptions
        {
            MinLevel = LogLevel.Trace,
            Rules =
            {
                new LoggerFilterRule("Console", "Microsoft.EntityFrameworkCore", LogLevel.Warning, null),
                new LoggerFilterRule("Console", "Microsoft.EntityFrameworkCore.Database.Command", LogLevel.Information,
                    null),
            }
        });

        private class TestFormatterConfigureOptions : IConfigureOptions<ConsoleLoggerOptions>
        {
            public void Configure(ConsoleLoggerOptions options)
            {
                options.FormatterName = TestConsoleFormatter.LoggerName;
            }
        }
    }

    public class TestConsoleFormatter : ConsoleFormatter
    {
        public static readonly string LoggerName = "TestFormatter";

        public TestConsoleFormatter()
            : base(LoggerName)
        {
        }

        public override void Write<TState>(in LogEntry<TState> logEntry, IExternalScopeProvider scopeProvider,
            TextWriter textWriter)
        {
            textWriter.Write(DateTime.Now.ToString("T"));
            textWriter.Write("|");
            textWriter.Write(logEntry.LogLevel);
            textWriter.Write("|");

            textWriter.Write(logEntry.Category);
            textWriter.Write("|");
            textWriter.Write(logEntry.State);
            if (logEntry.Exception != null)
            {
                textWriter.Write("|");
                textWriter.Write(logEntry.Exception.ToString());
            }

            textWriter.Write(Environment.NewLine);
        }
    }
}