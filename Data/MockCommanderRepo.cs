using Commander.Models;
using System.Collections.Generic;

/* This mock repository is solely used for testing purposes. */

namespace Commander.Data
{
    public class MockCommanderRepo : ICommanderRepo
    {
        public void CreateCommand(Command cmd)
        {
            throw new System.NotImplementedException();
        }

        public void DeleteCommand(Command cmd)
        {
            throw new System.NotImplementedException();
        }

        public IEnumerable<Command> GetAllCommands()
        {
            var commands = new List<Command>
            {
                new Command {Id = 0, HowTo = "Boil egg", Line = "Boil Water133", Platform = "Kettle1" },
                new Command {Id = 1, HowTo = "Boil tofu", Line = "Boil Water2", Platform = "Kettle2" },
                new Command {Id = 2, HowTo = "Boil vegetables", Line = "Boil Water3", Platform = "Kettle3" }
            };

            return commands;
        }

        public Command GetCommandById(int id)
        {
            return new Command {Id = 0, HowTo = "Boil egg", Line = "Boil Water", Platform = "Kettle" };
        }

        public bool SaveChanges()
        {
            throw new System.NotImplementedException();
        }

        public void UpdateCommand(Command cmd)
        {
            throw new System.NotImplementedException();
        }
    }
}