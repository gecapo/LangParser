using Sprache;
using System;
using System.Collections.Generic;

class Program
    {
        static void Main(string[] args)
        {
            //string test = "when Activity.Subject contains snippet 'Fund.Name'";
           // string test1 = "when Activity with Subject contains snippet 'Quarter / Year'";
           // string test2 = "for Activity with Subject contains ‘Emerging’ OR Subject contains ‘Smith’ do set body = 'test'";
           // string test3 = "Do set Activity body = 'sekunda'";
           // string test4 = "For Activity with Subject contains all words 'tedsastsds'";
           // string test5 = "For Activity with Subject is 'tedsastsds' and Body contains all words 'SEs'";
            string test6 = "For Activity with Subject is 'tedsastsds' and Body contains all words 'SEs' and Date contains exact phrase 'MART' OR BODY = 'GEORGI' and GOGO EQUALS 'EBE MAIKI'";

            string testH0i = @"when Investor Account.Subject starts   
                                                                with snippet 'Fund.Name'";

           //var qutedtext = Grammar.CommandDispacher.Parse(test);
           //var qutedtext1 = Grammar.CommandDispacher.Parse(test1);
           //var qutedtext2 = Grammar.CommandDispacher.Parse(test2);
           //var qutedtext3 = Grammar.CommandDispacher.Parse(test3);
           //var qutedtext4 = Grammar.CommandDispacher.Parse(test4);
           //var qutedtext5 = Grammar.CommandDispacher.Parse(test5);
            var qutedtext6 = Grammar.CommandDispacher.Parse(test6);
            ;

        }
    }
    public static class Grammar
    {
            public static readonly Parser<string> StartsWith =
                from startsLiteral in Parse.IgnoreCase("STARTS").Token()
                from withLiteral in Parse.IgnoreCase("WITH").Token()
                select "Starts With"; //TODO MAKE IT RETURN CLASS OPERATOR OR SMTH

            public static readonly Parser<string> EndsWith =
                from endsLiteral in Parse.IgnoreCase("ENDS").Token()
                from withLiteral in Parse.IgnoreCase("WITH").Token()
                select "Ends With"; //TODO MAKE IT RETURN CLASS OPERATOR OR SMTH

            public static readonly Parser<string> Equal =
                from equalsLiteral in Parse.IgnoreCase("EQUALS").Token()
                select "Equals"; //TODO MAKE IT RETURN CLASS OPERATOR OR SMTH

            public static readonly Parser<string> EqualsSign =
                from equalsLiteral in Parse.IgnoreCase("=").Token()
                select "="; //TODO MAKE IT RETURN CLASS OPERATOR OR SMTH

           public static readonly Parser<string> IsNotSign =
                from isNotLiteral in Parse.IgnoreCase("!=").Token()
                select "!="; //TODO MAKE IT RETURN CLASS OPERATOR OR SMTH

            public static readonly Parser<string> EqualOrMore =
                from literal in Parse.IgnoreCase(">=").Token()
                select ">="; //TODO MAKE IT RETURN CLASS OPERATOR OR SMTH

            public static readonly Parser<string> More =
                from literal in Parse.IgnoreCase(">").Token()
                select ">"; //TODO MAKE IT RETURN CLASS OPERATOR OR SMTH

            public static readonly Parser<string> EqualOrLess =
                from literal in Parse.IgnoreCase("<=").Token()
                select "<="; //TODO MAKE IT RETURN CLASS OPERATOR OR SMTH

            public static readonly Parser<string> Less =
                from literal in Parse.IgnoreCase("<").Token()
                select "<"; //TODO MAKE IT RETURN CLASS OPERATOR OR SMTH

            public static readonly Parser<string> Plus =
                from literal in Parse.IgnoreCase("+").Token()
                select "+"; //TODO MAKE IT RETURN CLASS OPERATOR OR SMTH
            
            public static readonly Parser<string> Minus =
                from literal in Parse.IgnoreCase("-").Token()
                select "-"; //TODO MAKE IT RETURN CLASS OPERATOR OR SMTH

            public static readonly Parser<string> Contains =
                from containsLiteral in Parse.IgnoreCase("CONTAINS").Token()
                select "Contains"; //TODO MAKE IT RETURN CLASS OPERATOR OR SMTH

            public static readonly Parser<string> Is =
                from literal in Parse.IgnoreCase("IS").Token()
                select "Is"; //TODO MAKE IT RETURN CLASS OPERATOR OR SMTH

            public static readonly Parser<string> Max =
                from literal in Parse.IgnoreCase("MAX").Token()
                select "Max"; //TODO MAKE IT RETURN CLASS OPERATOR OR SMTH

            public static readonly Parser<string> Sin =
                from literal in Parse.IgnoreCase("SIN").Token()
                select "Sin"; //TODO MAKE IT RETURN CLASS OPERATOR OR SMTH

            public static readonly Parser<string> ContainsSnippet =
                from containsLiteral in Parse.IgnoreCase("CONTAINS").Token()
                from snippetLiteral in Parse.IgnoreCase("SNIPPET").Token()
                select "Contains Snippet"; //TODO MAKE IT RETURN CLASS OPERATOR OR SMTH

            public static readonly Parser<string> ContainsExactPhrase =
                from containsLiteral in Parse.IgnoreCase("CONTAINS").Token()
                from exactLiteral in Parse.IgnoreCase("EXACT").Token()
                from phraseLiteral in Parse.IgnoreCase("PHRASE").Token()
                select "Contains exact phrase"; //TODO MAKE IT RETURN CLASS OPERATOR OR SMTH

            public static readonly Parser<string> ContainsAllWords =
                from containsLiteral in Parse.IgnoreCase("CONTAINS").Token()
                from allLiteral in Parse.IgnoreCase("ALL").Token()
                from wordsLiteral in Parse.IgnoreCase("WORDS").Token()
                select "Contains all words"; //TODO MAKE IT RETURN CLASS OPERATOR OR SMTH

            public static readonly Parser<string> ContainsAlias =
                from containsLiteral in Parse.IgnoreCase("CONTAINS").Token()
                from aliasLiteral in Parse.IgnoreCase("ALIAS").Token()
                select "Contains alias"; //TODO MAKE IT RETURN CLASS OPERATOR OR SMTH

            //REGISTER OPERATORS
            public static readonly Parser<string> Operator =
                     StartsWith
                    .Or(EndsWith)
                    .Or(Equal)
                    .Or(EqualsSign)
                    .Or(IsNotSign)
                    .Or(EqualOrMore)
                    .Or(More)
                    .Or(EqualOrLess)
                    .Or(Less)
                    .Or(Plus)
                    .Or(Minus)
                    .Or(Max)
                    .Or(Sin)
                    .Or(Is)
                    .Or(Contains)
                    .Or(ContainsSnippet)
                    .Or(ContainsExactPhrase)
                    .Or(ContainsAllWords)
                    .Or(ContainsAlias);

        //REGISTER SEPARATORS
        private static readonly Parser<IOption<IEnumerable<char>>> Separator = Parse.IgnoreCase(GrammarDictionary.Separator.SEPARATOR_OR)
            .Or(Parse.IgnoreCase(GrammarDictionary.Separator.SEPARATOR_AND)).Optional();

        private static readonly Parser<char> SeparatorChar = Parse.Chars("\\\"= .\'");
        private static readonly Parser<char> ControlChar = Parse.Char(Char.IsControl, "Control character");

        //REGISTER MODIFIERS
        private static readonly Parser<string> Modifier = Parse.IgnoreCase(GrammarDictionary.MOD_WITH)
            .Or(Parse.String(GrammarDictionary.MOD_DOT)).Text();

        //COMMAND DISPACHER
        public static Parser<GrammarDictionary.Command> CommandChoserParser =
            Parse.IgnoreCase(GrammarDictionary.Initializers.FOR_COMMAND).Token()
                .Return(GrammarDictionary.Command.Trigger)
            .Or(Parse.IgnoreCase(GrammarDictionary.Initializers.WHEN_COMMAND).Token()
                        .Return(GrammarDictionary.Command.Trigger))
            .Or(Parse.IgnoreCase(GrammarDictionary.Initializers.DO_COMMAND).Token()
                    .Return(GrammarDictionary.Command.Subject));


        public static Parser<ICommand> CommandDispacher =
            from cmd in CommandChoserParser
            from value in Parse.AnyChar.AtLeastOnce().Text()
            select CommandDispacherParser(cmd, value);

        public static ICommand CommandDispacherParser(GrammarDictionary.Command cmd, string value)
        {
            switch (cmd)
            {
                case GrammarDictionary.Command.Trigger:
                    return new TriggerCommand(Grammar.Entity.Parse(value));
                case GrammarDictionary.Command.Subject:
                    return new SubjectCommand(value);
                default:
                    throw new Exception("Invalid Command");
            };
        }

        //OTHER
        private static readonly Parser<char> TokenChar = Parse.AnyChar.Except(SeparatorChar).Except(ControlChar);
        private static readonly Parser<string> Token = TokenChar.AtLeastOnce().Text();

        private static readonly Parser<char> DoubleQuote = Parse.Char((char)39);
        private static readonly Parser<char> QdText = Parse.AnyChar.Except(DoubleQuote);


        private static readonly Parser<string> QuotedString =
            from open in DoubleQuote
            from text in QdText.Many().Text()
            from close in DoubleQuote
            select text;

        public static readonly Parser<Property> Property =
            from name in Token
            from fws in Parse.WhiteSpace
            from _ in Operator
            from sws in Parse.WhiteSpace
            from value in QuotedString
            select new Property(name, _, value);

        public static readonly Parser<Property> ListProperty =
            from and in Separator
            from a in Property
            select a;

        public static readonly Parser<Entity> Entity =
            from name in Token
            from fws in Parse.WhiteSpace.Optional()
            from mood in Modifier
            from sws in Parse.WhiteSpace.Optional()
            from value in ListProperty.Many()
            select new Entity(name, value);

        public static readonly Parser<string> SubjectCommand = Parse.IgnoreCase(GrammarDictionary.SubjectCommand.SET)
            .Or(Parse.IgnoreCase(GrammarDictionary.SubjectCommand.DELETE))
            .Or(Parse.IgnoreCase(GrammarDictionary.SubjectCommand.RELATE)).Text();
    }

    public static class GrammarDictionary
    {

        public static string MOD_WITH = "WITH";
        public static string MOD_DOT = ".";
        public static string MOD_NOT = "NOT"; //TODO

        public static class Initializers
        {
            public static string FOR_COMMAND = "FOR";
            public static string DO_COMMAND = "DO";
            public static string WHEN_COMMAND = "WHEN";
        }

        public enum Command
        {
            Trigger,
            Subject
        }


        public static class SubjectCommand
        {
            public static string SET = "SET";
            public static string DELETE = "DELETE";
            public static string RELATE = "RELATE TO";
        }


        public static class Separator
        {
            public static string SEPARATOR_OR = " OR ";
            public static string SEPARATOR_AND = " AND ";
        }

    }


    public class Entity : Item
    {
        public Entity(string name, IEnumerable<Property> value)
        {
            Name = name;
            Properties = value;
        }

        public string Name { get; set; }
        public IEnumerable<Property> Properties { get; set; }
    }


    public class Property : Item
    {
        public Property(string name, string @operator, string value)
        {
            Name = name;
            Value = value;
            @Operator = @operator;
        }

        public string Name { get; }
        public string Operator { get; }
        public string Value { get; }
    }

    public interface Item
    {

    }

    public class TriggerCommand : ICommand
    {
        public TriggerCommand()
        {

        }
        public TriggerCommand(Entity en)
        {
            TriggerEntity = en;
        }
        public Entity TriggerEntity { get; set; }


        public string Execute()
        {
            return "SITE.SELECT {ENTITY}";
        }
    }

    public class SubjectCommand : ICommand
    {
        //MAJOR TODO
        public SubjectCommand()
        {

        }
        public SubjectCommand(string en)
        {
            TriggerEntity = en;
        }
        public string TriggerEntity { get; set; }


        public string Execute()
        {
            return "SITE.SELECT {ENTITY}";
        }
    }


    public interface ICommand
    {
        string Execute();
    }
