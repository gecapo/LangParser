using Sprache;
using Xunit;

namespace LangParser.Tests
{
    public class TriggerExpressionParserTest
    {
        [Fact]
        public void TryParseSimpleExpressionSucceeds()
        {
            string input = @"For Activity with Subject = 555 or (sss = 'ssss' or kkk = 'kkk')";

            var output = Grammar.Trigger.TryParse(input);

            Assert.True(output.WasSuccessful);
        }

        [Fact]
        public void ParseSimpleExpressionSucceeds()
        {
            string input = @"For Activity with Subject = 555 or (sss = 'ssss' or kkk = 'kkk')";

            var number = 555;
            var target1 = new Target() { EntityName = "Activity", PropertyName = "Subject" };
            var constant1 = new ConststantExpression() { Value = string.Format("{0}", number) };

            var target2 = new Target() { PropertyName = "sss"};
            var constant2 = new ConststantExpression() { Value = "ssss" };

            var target3 = new Target() { PropertyName = "kkk"};
            var constant3 = new ConststantExpression() { Value = "kkk" };

            var mock1 = new BinaryExpression{Left = target1, @operator = Operators.@is, Right = constant1};
            var mock2 = new BinaryExpression{Left = target2, @operator = Operators.@is, Right = constant2};
            var mock3 = new BinaryExpression{Left = target3, @operator = Operators.@is, Right = constant3};
            var mock4 = new BinaryExpression{Left = mock2, @operator = Operators.or, Right = mock3};
            var mock = new BinaryExpression{Left = mock1, @operator = Operators.or, Right = mock4};

            var output = Grammar.Trigger.Parse(input);

            Assert.Equal(((Expression)mock).ToString(), output.ToString());
        }

        [Fact]
        public void TryParseComplicatedExpressionSucceeds()
        {
            string input = "for ssad= 7777 or (((Activity with Subject = 'LUrena' or Body is 'HUR') or ((Name = 'smqh' or Body = 'smqh') or Kurami contains 'parafin')) or sss= 'ss')";

            var output = Grammar.Trigger.TryParse(input);

            Assert.True(output.WasSuccessful);
        }

        [Fact]
        public void TryParseForFilterExpressionSucceeds()
        {
            string input = "for filter";

            var output = Grammar.Trigger.TryParse(input);

            Assert.True(output.WasSuccessful);
        }

        [Fact]
        public void ParseForFilterExpressionSucceeds()
        {
            string input = "for filter";

            var output = Grammar.Trigger.Parse(input);

            Assert.Equal("filter", output.ToString().Trim());
        }
    }

    public class CommandExpressionParserTests
    {
        [Fact]
        public void TryParseSimpleExpressionSucceeds()
        {
            
        }

        [Fact]
        public void ParseSimpleExpressionSucceeds()
        {
            
        }
    }
}
