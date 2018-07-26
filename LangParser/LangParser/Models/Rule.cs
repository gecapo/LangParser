using System;
using System.Collections.Generic;
using System.Text;

namespace D6S.Tool.RulesV2.LanguageParser.Models
{
    public class Rule
    {
        public Expression Target { get; set; }
        public List<Expression> Actions { get; set; }

        public override string ToString()
        {
            var actionsToString = string.Empty;
            Actions.ForEach(action => actionsToString += $"{action.ToString()} ");
            return $"{Target} {actionsToString.Trim()}";
        }
    }
}
