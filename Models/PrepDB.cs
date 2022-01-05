using Commander.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;

namespace Commander.Models
{
    public static class PrepDB
    {
        public static void PrepPopulation(IApplicationBuilder app)
        {
            using (var serviceScope = app.ApplicationServices.CreateScope())
            {
                SeedData(serviceScope.ServiceProvider.GetService<CommanderContext>());
            }
        }

        public static void SeedData(CommanderContext context)
        {
            System.Console.WriteLine("Applying Migrations...");

            context.Database.Migrate();

            if (!context.Commands.Any())
            {
                System.Console.WriteLine("Adding data - seeding...");
            
                context.Commands.AddRange(
                    new Command() {HowTo="Display path of current working directory", Line="pwd", Platform="Linux"},
                    new Command() {HowTo="Change directory", Line="cd <directory>", Platform="Linux"},
                    new Command() {HowTo="List directory contents", Line="ls", Platform="Linux"},
                    new Command() {HowTo="Output the contents of file", Line="cat <file>", Platform="Linux"},
                    new Command() {HowTo="Delete file", Line="rm <file>", Platform="Linux"},
                    new Command() {HowTo="Copy file", Line="cp <file> <new file>", Platform="Linux"},
                    new Command() {HowTo="Create new directory", Line="mkdir <directory>", Platform="Linux"},
                    new Command() {HowTo="Delete directory", Line="rmdir <directory>", Platform="Linux"},
                    new Command() {HowTo="Read the first 10 lines of a file", Line="head <file>", Platform="Linux"},
                    new Command() {HowTo="Read the last 10 lines of a file", Line="tail <file>", Platform="Linux"},
                    new Command() {HowTo="Run a command as root", Line="sudo", Platform="Linux"},
                    new Command() {HowTo="Search for a string in a file", Line="grep 'pattern/string' <file>", Platform="Linux"},
                    new Command() {HowTo="Open a file or directory in GUI", Line="open <file/directory>", Platform="Linux"},
                    new Command() {HowTo="Print the current date", Line="date", Platform="Linux"},
                    new Command() {HowTo="Show the last 500 commands used", Line="history", Platform="Linux"},
                    new Command() {HowTo="Compare the contents of two files", Line="diff <file1> <file2>", Platform="Linux"},
                    new Command() {HowTo="Define temporary aliases", Line="alias <command>=<new command>", Platform="Linux"},
                    new Command() {HowTo="Remove an alias", Line="unlias <alias>", Platform="Linux"},
                    new Command() {HowTo="Display manual page of a command", Line="man <command>", Platform="Linux"},
                    new Command() {HowTo="Update the access and modification times of a file", Line="touch <file>", Platform="Linux"}
                );

                context.SaveChanges();
            }
            else {
                System.Console.WriteLine("Already have data - not seeding");
            }
        }
    }
}