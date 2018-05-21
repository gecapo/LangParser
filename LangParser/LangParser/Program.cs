using Sprache;

namespace LangParser
{
    class Program
    {
        public static void Main(string[] args)
        {
            string input = "for Activity";
            var a = Grammar.Trigger.Parse(input);

            ;
        }
    }
    
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

        private static readonly Parser<Operators> Plus =
            from literal in Parse.IgnoreCase("+").Token()
            select Operators.plus;

        private static readonly Parser<Operators> Minus =
            from literal in Parse.IgnoreCase("-").Token()
            select Operators.minus;

        private static readonly Parser<Operators> Contains =
            from containsLiteral in Parse.IgnoreCase("CONTAINS").Token()
            select Operators.contains;

        private static readonly Parser<Operators> Is =
            from literal in Parse.IgnoreCase("IS").Token()
            select Operators.@is;

        private static readonly Parser<Operators> Max =
            from literal in Parse.IgnoreCase("MAX").Token()
            select Operators.max;

        private static readonly Parser<Operators> Sin =
            from literal in Parse.IgnoreCase("SIN").Token()
            select Operators.sin;

        private static readonly Parser<Operators> ContainsSnippet =
            from containsLiteral in Parse.IgnoreCase("CONTAINS").Token()
            from snippetLiteral in Parse.IgnoreCase("SNIPPET").Token()
            select Operators.contains_snippet;

        private static readonly Parser<Operators> ContainsExactPhrase =
            from containsLiteral in Parse.IgnoreCase("CONTAINS").Token()
            from exactLiteral in Parse.IgnoreCase("EXACT").Token()
            from phraseLiteral in Parse.IgnoreCase("PHRASE").Token()
            select Operators.contains_exact_phrase;

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
                .Or(Plus)
                .Or(Minus)
                .Or(Max)
                .Or(Sin)
                .Or(Is)
                .Or(ContainsAlias)
                .Or(ContainsSnippet)
                .Or(ContainsExactPhrase)
                .Or(ContainsAllWords)
                .Or(Contains);
        #endregion

        #region  BinaryOperators
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

        private static readonly Parser<Expression> Integer =
            from open in Parse.Number
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


        private static readonly Parser<Expression> Binary =
            (from left in Target
             from op in Operator
             from right in Constant.Or(Integer)
             select new BinaryExpression()
             {
                 @operator = op,
                 Left = left,
                 Right = right
             }).Contained(Parse.Char('('), Parse.Char(')')).Token()
           .Or(from left in Target
               from op in Operator
               from right in Constant.Or(Integer)
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
             }).Contained(Parse.Char('('), Parse.Char(')')).Token()
            .Or(from left in Binary.Or(Parse.Ref(() => LogicalExpression))
                from @operator in LogicalOperators
                from right in Binary.Or(Parse.Ref(() => LogicalExpression))
                select new BinaryExpression
                {
                    @operator = @operator,
                    Left = left,
                    Right = right
                });


        private static readonly Parser<Expression> Expression = Target.Or(LogicalExpression.Or(Binary));

        private static Expression ParseExpression(string expression)
        {
            if (expression.StartsWith('(') && expression.EndsWith(')') || expression.Trim().Split(' ').Length == 1)
            {
                return Expression.Parse(expression);
            }
            return Expression.Parse(string.Format("({0})", expression));
        }

        public static readonly Parser<Expression> Trigger =
            from start in Starters
            from exp in Parse.AnyChar.AtLeastOnce().Text()
            select ParseExpression(exp);


        //public static readonly Parser<Expression> Command

    }
    
    public class Target : Expression
    {
        public string EntityName { get; set; }
        public string PropertyName { get; set; }

        public override string ToString()
        {
            return $"{EntityName} {PropertyName}";
        }
    }

    public class ConststantExpression : Expression
    {
        public string Value { get; set; }

        public override string ToString()
        {
            return Value;
        }
    }

    public class BinaryExpression : Expression
    {
        public Expression Left { get; set; }
        public Operators @operator { get; set; }
        public Expression Right { get; set; }


        public override string ToString()
        {
            return $"{Left} {@operator} {Right}";
        }
    }

    public abstract class Expression
    {
    }

    public enum Operators
    {
        //Bi
        all,
        contains,
        starts_with,
        greater_than,
        greater_than_or_equal,
        @is,
        not_is,
        less_than,
        less_than_or_equal,
        not_contains,
        @in,
        EqualToProp,
        NotEqualToProp,

        //TODO NOT SURE IF CONTAINED
        ends_with,
        plus,
        minus,
        max,
        sin,
        contains_exact_phrase,
        contains_snippet,
        contains_alias,

        //LOGICAL OPERATORS
        and,
        or
    }
}

