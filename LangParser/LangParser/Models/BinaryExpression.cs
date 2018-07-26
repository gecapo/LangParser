using System;
using System.Collections.Generic;
using System.Text;

namespace D6S.Tool.RulesV2.LanguageParser.Models
{
    public class BinaryExpression : Expression
    {
        /// <summary>
        /// Template for prop 1-Property of the entity, 2-Operation, 3-Value of prop
        /// </summary>
        private static string propTemplate = @"<rule prop=""{0}"" op=""{1}""><values><v>{2}</v></values></rule>";
        /// <summary>
        /// Template for entity 0-Name of the entity, 1-Props or/and Entities
        /// </summary>
        private static string entityTemplate = @"<e name=""{0}"">{1}</e>";

        public Expression Left { get; set; }
        public Operators @operator { get; set; }
        public Expression Right { get; set; }

        public override string ToString()
        {
            return $"{Left} {@operator} {Right}";
        }

        /// <summary>
        /// Builds Advf xml
        /// </summary>
        /// <param name="mainEntity"></param>
        /// <param name="once">Once is passed true only the first time in oreder for the most simples rules to be build with the right template</param>
        /// <returns></returns>
        public string ToAdvf(string mainEntity, bool once = false)
        {
            string leftAdvf = string.Empty, rightAdvf = string.Empty;

            if (Left is BinaryExpression left)
            {
                leftAdvf = left.ToAdvf(mainEntity);
            }

            if (Right is BinaryExpression right)
            {
                rightAdvf = right.ToAdvf(mainEntity);
            }

            if (Left is Target target && Right is ConststantExpression constant)
            {
                var template = string.Format(propTemplate, target.PropertyName, @operator, constant.Value);
                if (once)
                {
                    return string.Format(entityTemplate, mainEntity, template);
                }
                return template;
            }

            var result = string.Empty;
            switch (@operator)
            {
                case Operators.and:
                    return string.Format(entityTemplate, mainEntity, leftAdvf + rightAdvf);
                case Operators.or:
                    return string.Format(entityTemplate, mainEntity, leftAdvf) + string.Format(entityTemplate, mainEntity, rightAdvf);
                default:
                    throw new Exception("Couldn't get the advanced find filter from the rule");
            }
        }

    }
}
