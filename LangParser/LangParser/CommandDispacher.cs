using System;
using System.Collections.Generic;
using System.Text;
using LangParser.Grammar;

namespace LangParser
{
    class CommandDispacher
    {
        public static Queue<Command> DefineCommands(string wholeText)
        {
            //JUST TESTING STUFF
            var correct = wholeText.StartsWith(GrammarDictionary.ForCommand, StringComparison.OrdinalIgnoreCase);
            if (!correct)
            {
                throw new Exception("Incorect Command");
            }

            correct = wholeText.Contains(GrammarDictionary.DoCommand);
            if (!correct)
            {
                throw new Exception("Incorect Command");
            }


            var triggerCommand = new Command()
            {
                type = CommandType.TriggerCommand,
                subcomands = wholeText.Split(GrammarDictionary.DoCommand, StringSplitOptions.RemoveEmptyEntries)[0]
            };


            var subjectCommand = new Command()
            {
                type = CommandType.SubjectCommand,
                subcomands = GrammarDictionary.DoCommand + wholeText.Split(GrammarDictionary.DoCommand, StringSplitOptions.RemoveEmptyEntries)[1]
            };

            Queue<Command> que = new Queue<Command>();
            que.Enqueue(triggerCommand);
            que.Enqueue(subjectCommand);
            return que;
        }


    }


    class Command
    {
        public CommandType type { get; set; }

        public string subcomands { get; set; }
    }

    enum CommandType
    {
        TriggerCommand,
        SubjectCommand
    }

    class SubCommand
    {
        string commnad { get; set; }
    }
}
