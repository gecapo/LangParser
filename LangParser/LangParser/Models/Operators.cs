using System;
using System.Collections.Generic;
using System.Text;

namespace D6S.Tool.RulesV2.LanguageParser.Models
{
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
        contains_only,

        //TODO NOT SURE IF CONTAINED
        ends_with,
        contains_snippet,
        contains_alias,

        //LOGICAL OPERATORS
        and,
        or,


        //Relate Operators
        ToThisAndAllRelated,
        ToAllRelated
    }
}
