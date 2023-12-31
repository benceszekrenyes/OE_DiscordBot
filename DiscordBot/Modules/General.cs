﻿using Discord;
using Discord.Commands;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscordBot.Modules
{
    public class General : ModuleBase<SocketCommandContext>
    {
        private readonly ILogger<General> _logger;
        //You can inject the host. This is useful if you want to shutdown the host via a command, but be careful with it.
        private readonly IHost _host;
        private readonly Random rnd;

        List<string> _cars;

        public void InitCars()
        {
            foreach (string s in Directory.GetFiles("Cars"))
            {
                _cars.Add(s);
            }
            
        }


        public General(IHost host, ILogger<General> logger)
        {
            _host = host;
            _logger = logger;
            rnd = new Random();
            _cars = new List<string>();
            InitCars();
        }

        [Command("ping")]
        [Alias("p")]
        public async Task PingAsync()
        {
            await Context.Channel.TriggerTypingAsync();
            await Context.Channel.SendMessageAsync("Pong!");
        }

        [Command("valami")]
        public async Task valami()
        {

            await Context.Channel.TriggerTypingAsync();
            await Context.Channel.SendMessageAsync("Te vagy a király!!!");
            await Context.Channel.SendFileAsync("Pictures/ad_132299305.jpg");
        }



        [Command("car")]
        public async Task Cars()
        {
            await Context.Channel.TriggerTypingAsync();
            await Context.Channel.SendFileAsync(_cars[rnd.Next(0, _cars.Count)]);
        }

        [Command("shutdown")]
        public Task Stop()
        {
            _ = _host.StopAsync();
            return Task.CompletedTask;
        }

        [Command("log")]
        public Task TestLogs()
        {
            _logger.LogTrace("This is a trace log");
            _logger.LogDebug("This is a debug log");
            _logger.LogInformation("This is an information log");
            _logger.LogWarning("This is a warning log");
            _logger.LogError(new InvalidOperationException("Invalid Operation"), "This is a error log with exception");
            _logger.LogCritical(new InvalidOperationException("Invalid Operation"), "This is a critical load with exception");

            _logger.Log(GetLogLevel(LogSeverity.Error), "Error logged from a Discord LogSeverity.Error");
            _logger.Log(GetLogLevel(LogSeverity.Info), "Information logged from Discord LogSeverity.Info ");

            return Task.CompletedTask;
        }

        private static LogLevel GetLogLevel(LogSeverity severity)
            => (LogLevel)Math.Abs((int)severity - 5);
    }
}
