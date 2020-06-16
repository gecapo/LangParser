using System;
using System.Collections.Generic;
using System.Text;
using D6S.Client;

namespace LanguageParser.Models
{
    public class Target : Expression
    {
        //Template for entity 0-Name of the entity, 1-Props or/and Entities
        private static string EntityTemplate = @"<e name=""{0}""></e>";

        public string EntityName { get; set; }
        public string PropertyName { get; set; }

        public override string ToString()
        {
            return $"{EntityName} {PropertyName}";
        }

        public string ToAdfv()
        {
            return string.Format(EntityTemplate, EntityName);
        }
    }
}
