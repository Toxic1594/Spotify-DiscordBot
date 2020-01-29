using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord.WebSocket;
using Discord;
using Discord.Commands;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using DiscordBot_1.Modules;
using System.IO;
namespace DiscordBot_1
{
    class Program
    {
        public static List<string> TESTLOL;
        public static void Main(string[] args)
        {
            Program ppp = new Program();
            ppp.additemstolist();
           
            new Program().MainAsync().GetAwaiter().GetResult();
        }
        private Task Log(LogMessage msg)
        {
            Console.WriteLine(msg.ToString());
            return Task.CompletedTask;
        }
        private Task BotLoggedIn()
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(DateTime.Now.ToString("HH:mm:ss") + " Successfully logged in!");
            Console.ForegroundColor = ConsoleColor.White;
            return Task.CompletedTask;
        }


        private DiscordSocketClient _client;
        private CommandService _commands;
        private IServiceProvider _services;
        public async Task MainAsync()
        {

            _client = new DiscordSocketClient();
            _commands = new CommandService();
            _services = new ServiceCollection()
                    .AddSingleton(_client)
                    .AddSingleton(_commands)
                    .BuildServiceProvider();
            _client.Log += Log;
            _client.LoggedIn += BotLoggedIn;
            //  You can assign your bot token to a string, and pass that in to connect.
            //  This is, however, insecure, particularly if you plan to have your code hosted in a public repository.
            var token = "NjAwNjk1MTUxNTc2MjE5NjU4.XibqkA.3I-68BOViZWOL_SKKTexzBlMEuM";

            // Some alternative options would be to keep your token in an Environment Variable or a standalone file.
            // var token = Environment.GetEnvironmentVariable("NameOfYourEnvironmentVariable");
            // var token = File.ReadAllText("token.txt");
            // var token = JsonConvert.DeserializeObject<AConfigurationClass>(File.ReadAllText("config.json")).Token;
            await RegisterCommandsAsync();
            await _client.LoginAsync(TokenType.Bot, token);
            await _client.StartAsync();
            await _client.SetStatusAsync(UserStatus.Online);
            await _client.SetGameAsync("Dropping Spotify Accs for you <3");                       
                // Block this task until the program is closed.
            await Task.Delay(-1);
        }
        public async Task RegisterCommandsAsync()
        {
            _client.MessageReceived += HandleCommandAsync;
            await _commands.AddModuleAsync(typeof(Modules.Commands), _services);
        }
        private async Task HandleCommandAsync(SocketMessage arg)
        {
            var message = arg as SocketUserMessage;
            if (message == null) return;
            var context = new SocketCommandContext(_client, message);
            if (message.Author.IsBot) return;
            if (context.Channel.Id != 663538221539655680) return;
            int argPos = 0;
            //if (message.Author.Username != "Singleplayer#1594") return;
            if (message.HasStringPrefix("!", ref argPos))
            {
                var result = await _commands.ExecuteAsync(context, argPos, _services);
                if (!result.IsSuccess) Console.WriteLine(result.ErrorReason);
            }
        }

        public void additemstolist()
        {
            TESTLOL = File.ReadLines("/stuff/Spotify/Premium.txt").ToList();
            for (int i =0; i<TESTLOL.Count; i++)
            {
                Console.WriteLine(TESTLOL[i]);
            }
            
        }
    }
}
