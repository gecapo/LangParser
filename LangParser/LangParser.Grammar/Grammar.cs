namespace LangParser.Grammar
{
    using Sprache;
    using System;
    using System.Linq;
    class Grammar
    {
        private static readonly Parser<char> SeparatorChar = Parse.Chars("()<>@,;:\\\"/[]?={} \t");
        private static readonly Parser<char> ControlChar = Parse.Char(Char.IsControl, "Control character");

        private static readonly Parser<char> TokenChar = Parse.AnyChar.Except(SeparatorChar).Except(ControlChar);
        private static readonly Parser<string> Token = TokenChar.AtLeastOnce().Text();

        private static readonly Parser<char> DoubleQuote = Parse.Char('"');
        private static readonly Parser<char> Backslash = Parse.Char('\\');

        private static readonly Parser<char> QdText = Parse.AnyChar.Except(DoubleQuote);

        private static readonly Parser<char> QuotedPair =
            from _ in Backslash
            from c in Parse.AnyChar
            select c;

        private static readonly Parser<string> QuotedString =
            from open in DoubleQuote
            from text in QuotedPair.Or(QdText).Many().Text()
            from close in DoubleQuote
            select text;

        private static readonly Parser<string> PropertyValue = Token.Or(QuotedString);

        private static readonly Parser<char> EqualSign = Parse.Char('=');

        private static readonly Parser<Property> Parameter =
            from name in Token
            from _ in EqualSign
            from value in PropertyValue
            select new Property(name, value);

        private static readonly Parser<char> Comma = Parse.Char(',');

        private static readonly Parser<char> ListDelimiter =
            from leading in Parse.WhiteSpace.Many()
            from c in Comma
            from trailing in Parse.WhiteSpace.Or(Comma).Many()
            select c;

        private static readonly Parser<Property[]> Properties =
            from p in Parameter.DelimitedBy(ListDelimiter)
            select p.ToArray();

        public static readonly Parser<Entity> Entity =
            from scheme in Token
            from _ in Parse.WhiteSpace.AtLeastOnce()
            from parameters in Properties
            select new Entity(scheme, parameters);
    }
}
