using System;
using System.Collections.Generic;
using System.Text;

namespace D6S.Tool.RulesV2.LanguageParser.Models
{
    /// <summary>
    /// Command that will be executed on Target.
    /// </summary>
    public class ActionExpression : Expression
    {
        public Command Command { get; set; }
        public Expression Action { get; set; }

        public override string ToString()
        {
            return $"{Command} {Action}";
        }
    }
}
