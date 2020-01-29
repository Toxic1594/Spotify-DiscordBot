using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord.Commands;
using System.IO;
namespace DiscordBot_1.Modules
{
   
    class Commands : ModuleBase<SocketCommandContext>
    {
        Program prg = new Program();
        [Command("account")]
        public async Task Account()
        {
            
            var user = Context.User;
            if (File.Exists("/stuff/users/" + user + ".txt"))
            {

            }
            else
            {
                var myFile = File.Create("/stuff/users/" + user + ".txt");
                myFile.Close();
            }


            //read time
            StreamReader strmread = new StreamReader("/stuff/users/" + user + ".txt");
            string test = strmread.ReadToEnd();
            strmread.Close();
            int timeleft = 0;
            if (test != "")
            {
                TimeSpan timespan = DateTime.Parse(test) - DateTime.Now;
                timeleft = (int)timespan.TotalMinutes;
            }
            if (test == "" || timeleft <= 0)
            {
                
                string account = readAccountList("");
                if (account == "")
                {
                    await ReplyAsync("Accounts out of Stock");
                }
                else
                {
                    StreamWriter strmwriter = new StreamWriter("/stuff/users/" + user + ".txt");
                    strmwriter.WriteLine(DateTime.Now.AddDays(1));
                    strmwriter.Close();
                    await ReplyAsync("Check your PM´s");
                    await Discord.UserExtensions.SendMessageAsync(user, "Thanks for using Singleplayer´s Bot, Here is your Account-> " +account);
                }


            }
            else
            {
                try
                {
                    await ReplyAsync("Time until u can request again: " + timeleft + "Minutes");
                }
                catch (Exception ex)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Error: " + ex);
                    await ReplyAsync("Error , check console");
                    Console.ForegroundColor = ConsoleColor.White;
                }
            }





        }


        public string readAccountList(string returnaccount)
        {
            if (Program.TESTLOL.Count <= 0) return "";
                returnaccount = Program.TESTLOL[0];
                Program.TESTLOL.RemoveAt(0);

                return returnaccount;
            

            
        }

    }
}
