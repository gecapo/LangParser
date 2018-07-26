using System;
using System.Collections.Generic;
using System.Text;

namespace D6S.Tool.RulesV2.LanguageParser.Models
{
    public class ConststantExpression : Expression
    {
        public string Value { get; set; }

        public override string ToString()
        {
            return Value;
        }
    }
}
