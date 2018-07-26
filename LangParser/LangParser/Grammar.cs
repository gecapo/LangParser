using System.Collections.Generic;
using System.Linq;
using D6S.Tool.RulesV2.LanguageParser.Models;
using Sprache;

namespace D6S.Tool.RulesV2.LanguageParser
{
    public static class Grammar
    {
        #region Operators
        //DEFINE OPERATORS
        private static readonly Parser<Operators> StartsWith =
            from startsLiteral in Parse.IgnoreCase("STARTS").Token()
            from withLiteral in Parse.IgnoreCase("WITH").Token()
            select Operators.starts_with;

        private static readonly Parser<Operators> EndsWith =
            from endsLiteral in Parse.IgnoreCase("ENDS").Token()
            from withLiteral in Parse.IgnoreCase("WITH").Token()
            select Operators.ends_with;

        private static readonly Parser<Operators> Equal =
            from equalsLiteral in Parse.IgnoreCase("EQUALS").Token()
            select Operators.@is;

        private static readonly Parser<Operators> EqualsSign =
            from equalsLiteral in Parse.IgnoreCase("=").Token()
            select Operators.@is;

        private static readonly Parser<Operators> IsNotSign =
            from isNotLiteral in Parse.IgnoreCase("!=").Token()
            select Operators.not_is;

        private static readonly Parser<Operators> EqualOrMore =
            from literal in Parse.IgnoreCase(">=").Token()
            select Operators.greater_than_or_equal;

        private static readonly Parser<Operators> More =
            from literal in Parse.IgnoreCase(">").Token()
            select Operators.greater_than;

        private static readonly Parser<Operators> EqualOrLess =
            from literal in Parse.IgnoreCase("<=").Token()
            select Operators.less_than_or_equal;

        private static readonly Parser<Operators> Less =
            from literal in Parse.IgnoreCase("<").Token()
            select Operators.less_than;

        private static readonly Parser<Operators> DoubleEqual =
            from literal in Parse.IgnoreCase("==").Token()
            select Operators.@is;

        private static readonly Parser<Operators> IsEmpty =
            from literal in Parse.IgnoreCase("IS").Token()
            from snippetLiteral in Parse.IgnoreCase("EMPTY").Token()
            select Operators.@is;

        private static readonly Parser<Operators> Contains =
            from containsLiteral in Parse.IgnoreCase("CONTAINS").Token()
            select Operators.contains;

        private static readonly Parser<Operators> Is =
            from literal in Parse.IgnoreCase("IS").Token()
            select Operators.@is;

        private static readonly Parser<Operators> DoesNotContain =
            from containsLiteral in Parse.IgnoreCase("DOES").Token()
            from aliasLiteral in Parse.IgnoreCase("NOT").Token()
            from phraseLiteral in Parse.IgnoreCase("CONTAIN").Token()
            select Operators.not_contains;

        private static readonly Parser<Operators> ContainsSnippet =
            from containsLiteral in Parse.IgnoreCase("CONTAINS").Token()
            from snippetLiteral in Parse.IgnoreCase("SNIPPET").Token()
            select Operators.contains_snippet;

        private static readonly Parser<Operators> ContainsExactPhrase =
            from containsLiteral in Parse.IgnoreCase("CONTAINS").Token()
            from exactLiteral in Parse.IgnoreCase("EXACT").Token()
            from phraseLiteral in Parse.IgnoreCase("PHRASE").Token()
            select Operators.contains_only;

        private static readonly Parser<Operators> ContainsAllWords =
            from containsLiteral in Parse.IgnoreCase("CONTAINS").Token()
            from allLiteral in Parse.IgnoreCase("ALL").Token()
            from wordsLiteral in Parse.IgnoreCase("WORDS").Token()
            select Operators.all;

        private static readonly Parser<Operators> ContainsAlias =
            from containsLiteral in Parse.IgnoreCase("CONTAINS").Token()
            from aliasLiteral in Parse.IgnoreCase("ALIAS").Token()
            select Operators.contains_alias;

        //REGISTER OPERATORS
        private static readonly Parser<Operators> Operator =
                 StartsWith
                .Or(EndsWith)
                .Or(Equal)
                .Or(EqualsSign)
                .Or(IsNotSign)
                .Or(EqualOrMore)
                .Or(More)
                .Or(EqualOrLess)
                .Or(Less)
                .Or(DoubleEqual)
                .Or(IsEmpty)
                .Or(Is)
                .Or(DoesNotContain)
                .Or(ContainsAlias)
                .Or(ContainsSnippet)
                .Or(ContainsExactPhrase)
                .Or(ContainsAllWords)
                .Or(Contains);
        #endregion

        #region  LogicOperators
        //DEFINE BinaryOperators
        private static readonly Parser<Operators> OrOperator =
            from equalsLiteral in Parse.IgnoreCase("OR").Token()
            select Operators.or;

        private static readonly Parser<Operators> AndOperator =
            from equalsLiteral in Parse.IgnoreCase("AND").Token()
            select Operators.and;

        //REGISTER BinaryOperators
        private static readonly Parser<Operators> LogicalOperators = OrOperator.Or(AndOperator);
        #endregion

        #region StarterKeywords
        private static readonly Parser<string> With =
            from withLiteral in Parse.IgnoreCase("WITH").Token()
            select "with";

        private static readonly Parser<string> When =
            from withLiteral in Parse.IgnoreCase("WHEN").Token()
            select "when";

        private static readonly Parser<string> For =
            from withLiteral in Parse.IgnoreCase("For").Token()
            select "For";

        private static readonly Parser<string> Starters = With.Or(When).Or(For);

        #endregion

        #region ActionStartKeywords
        private static readonly Parser<string> Do =
            from start in Parse.IgnoreCase("DO").Token()
            select "DO";

        private static readonly Parser<string> AlwaysDo =
            from start in Parse.IgnoreCase("ALWAYS").Token()
            from doKeyword in Parse.IgnoreCase("DO").Token()
            select "Always do";

        private static readonly Parser<string> ActionStartKeyWord = AlwaysDo.Or(Do);
        #endregion

        #region Commands
        private static readonly Parser<Command> Set =
            from start in Parse.IgnoreCase("SET").Token()
            select Command.Set;

        private static readonly Parser<Command> Delete =
            from start in Parse.IgnoreCase("Delete").Token()
            select Command.Delete;

        private static readonly Parser<Command> RelateThe =
            from start in Parse.IgnoreCase("RELATE").Token()
            from doKeyword in Parse.IgnoreCase("THE").Token()
            select Command.ReleteThe;

        private static readonly Parser<Command> CommandAction = RelateThe.Or(Set).Or(Delete);
        #endregion

        #region RelateOperators 
        private static readonly Parser<Operators> ToThisAndAllRelated =
            from start in Parse.IgnoreCase("ToThisAndAllRelated").Token()
            select Operators.ToThisAndAllRelated;

        private static readonly Parser<Operators> ToAllRelated =
            from start in Parse.IgnoreCase("ToAllRelated").Token()
            select Operators.ToAllRelated;
        #endregion

        private static readonly Parser<Operators> RelateOperator = ToThisAndAllRelated.Or(ToAllRelated);

        private static readonly Parser<char> DoubleQuote = Parse.Char((char)39);
        private static readonly Parser<char> QuotedText = Parse.AnyChar.Except(DoubleQuote);

        private static readonly Parser<Expression> Constant =
            from open in DoubleQuote
            from text in QuotedText.Many().Text()
            from close in DoubleQuote
            select new ConststantExpression
            {
                Value = text
            };

        private static readonly Parser<string> Decimal =
            from open in Parse.Number
            from _ in Parse.Char(',').Or(Parse.Char('.'))
            from next in Parse.Number
            select open + _ + next;

        private static readonly Parser<Expression> Number =
            from open in Decimal.Or(Parse.Number)
            select new ConststantExpression
            {
                Value = open
            };

        public static readonly Parser<Expression> Target =
            from entityName in Parse.Identifier(Parse.Letter, Parse.LetterOrDigit) //Parse.AnyChar.Except(Parse.Char('.').Or(Parse.WhiteSpace)).AtLeastOnce().Text()
            from dot in Parse.String(".").Or(Parse.IgnoreCase("with")).Token().Optional()
            from propertyName in Parse.Identifier(Parse.Letter, Parse.LetterOrDigit).Optional()//Parse.AnyChar.Except(Parse.Char('.').Or(Parse.WhiteSpace)).AtLeastOnce().Text().Optional()
            select new Target
            {
                EntityName = propertyName.IsEmpty ? null : entityName,
                PropertyName = propertyName.IsEmpty ? entityName : propertyName.Get()
            };

        public static readonly Parser<Expression> TargetEntity =
            from entityName in Parse.Identifier(Parse.Letter, Parse.LetterOrDigit) //Parse.AnyChar.Except(Parse.Char('.').Or(Parse.WhiteSpace)).AtLeastOnce().Text()
            select new Target
            {
                EntityName = entityName
            };


        private static readonly Parser<Expression> Binary =
            (from left in Target
             from op in Operator
             from right in Constant.Or(Number)
             select new BinaryExpression()
             {
                 @operator = op,
                 Left = left,
                 Right = right
             }).Contained(Parse.Char('('), Parse.Char(')')).Token()
           .Or(from left in Target
               from op in Operator
               from right in Constant.Or(Number)
               select new BinaryExpression()
               {
                   @operator = op,
                   Left = left,
                   Right = right
               });

        private static readonly Parser<Expression> LogicalExpression =
            (from left in Binary.Or(Parse.Ref(() => LogicalExpression))
             from @operator in LogicalOperators
             from right in Binary.Or(Parse.Ref(() => LogicalExpression))
             select new BinaryExpression
             {
                 @operator = @operator,
                 Left = left,
                 Right = right
             }).Contained(Parse.Char('('), Parse.Char(')')).Token();
        //TODO FOR NOW NOT REQUIRED
        //.Or(from left in Binary.Or(Parse.Ref(() => LogicalExpression))
        //    from @operator in LogicalOperators
        //    from right in Binary.Or(Parse.Ref(() => LogicalExpression))
        //    select new BinaryExpression
        //    {
        //        @operator = @operator,
        //        Left = left,
        //        Right = right
        //    });


        private static readonly Parser<Expression> Expression = TargetEntity.Or(LogicalExpression.Or(Binary));

        /// <summary>
        ///  Used for the times when a rule starts with an '(' 
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        private static Expression ParseExpression(string expression)
        {
            if (expression.StartsWith("(") && expression.EndsWith(")") || expression.Trim().Split(' ').Length == 1)
            {
                return Expression.Parse(expression);
            }
            return Expression.Parse(string.Format("({0})", expression));
        }

        private static readonly Parser<Expression> RelateTarget =
            from entityName in Parse.Identifier(Parse.Letter, Parse.LetterOrDigit)
            select new Target
            {
                EntityName = entityName,
                PropertyName = null
            };

        private static readonly Parser<Expression> BinaryAction =
            from left in Target.Or(Constant)
            from op in Operator.Or(RelateOperator)
            from right in Constant.Or(Number).Or(RelateTarget)
            select new BinaryExpression()
            {
                Left = left,
                @operator = op,
                Right = right
            };

        private static readonly Parser<Expression> RelateAction =
            from left in Binary
            from op in RelateOperator
            from right in RelateTarget
            select new BinaryExpression()
            {
                Left = left,
                @operator = op,
                Right = right
            };

        private static readonly Parser<ActionExpression> Action =
            from command in CommandAction
            from action in RelateAction.Or(BinaryAction)
            from _ in Parse.Char(',').Optional()
            select new ActionExpression()
            {
                Command = command,
                Action = action
            };

        //Public just for test cases TODO:Return After
        public static readonly Parser<IEnumerable<ActionExpression>> ActionList =
            from @do in ActionStartKeyWord
            from list in Action.AtLeastOnce()
            select list;

        //Public just for test cases TODO:Return After
        public static readonly Parser<Expression> Trigger =
            from start in Starters
            from exp in Parse.AnyChar.Except(ActionList).AtLeastOnce().Text()
            select ParseExpression(exp);
        
        public static readonly Parser<Rule> Rule =
            from target in Trigger
            from action in ActionList
            select new Rule()
            {
                Target = target,
                Actions = action.ToList<Expression>()
            };
    }
}
