using System.Collections.Generic;
using Sprache;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using D6S.Tool.RulesV2.LanguageParser;
using D6S.Tool.RulesV2.LanguageParser.Models;
using System;

namespace RulesV2.Tests.Parser
{
    [TestClass]
    public class TriggerExpressionParserTest
    {
        [TestMethod]
        public void TryParseSimpleExpressionSucceeds()
        {
            string input = @"For Activity with Subject = 555 or (sss = 'ssss' or kkk = 'kkk')";

            var output = Grammar.Trigger.TryParse(input);

            Assert.IsTrue(output.WasSuccessful);
        }

        [TestMethod]
        public void ParseSimpleExpressionSucceeds()
        {
            string input = @"For Activity with Subject = 555 or (sss = 'ssss' or kkk = 'kkk')";

            var number = 555;
            var target1 = new D6S.Tool.RulesV2.LanguageParser.Models.Target() { EntityName = "Activity", PropertyName = "Subject" };
            var constant1 = new ConststantExpression() { Value = string.Format("{0}", number) };

            var target2 = new D6S.Tool.RulesV2.LanguageParser.Models.Target() { PropertyName = "sss" };
            var constant2 = new ConststantExpression() { Value = "ssss" };

            var target3 = new Target() { PropertyName = "kkk" };
            var constant3 = new ConststantExpression() { Value = "kkk" };

            var mock1 = new BinaryExpression { Left = target1, @operator = Operators.@is, Right = constant1 };
            var mock2 = new BinaryExpression { Left = target2, @operator = Operators.@is, Right = constant2 };
            var mock3 = new BinaryExpression { Left = target3, @operator = Operators.@is, Right = constant3 };
            var mock4 = new BinaryExpression { Left = mock2, @operator = Operators.or, Right = mock3 };
            var mock = new BinaryExpression { Left = mock1, @operator = Operators.or, Right = mock4 };

            var output = Grammar.Trigger.Parse(input);

            Assert.AreEqual(((Expression)mock).ToString(), output.ToString());
        }

        [TestMethod]
        public void TryParseComplicatedExpressionSucceeds()
        {
            string input =
                "for ssad= 7777 or (((Activity with Subject = 'LUrena' or Body is 'HUR') or ((Name = 'smqh' or Body = 'smqh') or Kurami contains 'parafin')) or sss= 'ss')";

            var output = Grammar.Trigger.TryParse(input);

            Assert.IsTrue(output.WasSuccessful);
        }

        [TestMethod]
        public void TryParseForFilterExpressionSucceeds()
        {
            string input = "for filter";

            var output = Grammar.Trigger.TryParse(input);

            Assert.IsTrue(output.WasSuccessful);
        }

        [TestMethod]
        public void ParseForFilterExpressionSucceeds()
        {
            string input = "for filter";

            var output = Grammar.Trigger.Parse(input);

            Assert.AreEqual("filter", output.ToString().Trim());
        }
    }

    [TestClass]
    public class CommandExpressionParserTests
    {
        [TestMethod]
        public void TryParseSimpleSetCommandSucceeds()
        {
            string input = "Do set Activity.Body = 'Debel'";

            var output = Grammar.ActionList.TryParse(input);

            Assert.IsTrue(output.WasSuccessful);
        }

        [TestMethod]
        public void ParseSimpleSetCommandSucceeds()
        {
            string input = "Do set Activity.Body = 'Debel'";

            var output = Grammar.ActionList.Parse(input);

        }

        [TestMethod]
        public void TryParseSimpleRelateCommandSucceeds()
        {
            string input = "Do relate the 'Funds' ToAllRelated Activities";

            var output = Grammar.ActionList.TryParse(input);

            Assert.IsTrue(output.WasSuccessful);
        }

        [TestMethod]
        public void ParseSimpleRealateCommandSucceeds()
        {
            string input = "Do relate the 'Funds' ToAllRelated Activities";

            var output = Grammar.ActionList.Parse(input);
        }

        [TestMethod]
        public void TryParseCompositeCommandSucceeds()
        {
            string input = "Do set Activity.Body = 'Debel', relate the 'Funds' ToAllRelated Activities";

            var output = Grammar.ActionList.TryParse(input);

            Assert.IsTrue(output.WasSuccessful);
        }

        [TestMethod]
        public void ParseCompositeCommandSucceeds()
        {
            string input = "Do set Activity.Body = 'Debel', relate the 'Funds' ToAllRelated Activities";

            var output = Grammar.ActionList.Parse(input);
            ;
        }
    }

    [TestClass]
    public class ParseRuleTest
    {
        [TestMethod]
        public void TryParseRuleWithNoCondition()
        {

            string input = @"for Activity do set Activity.Body = 'Debel'";

            var output = Grammar.Rule.TryParse(input);

            Assert.IsTrue(output.WasSuccessful);
        }

        [TestMethod]
        public void ParseRuleWithNoCondition()
        {

            string input = @"for Activity do set Activity.Body = 'Debel'";


            var mockCommand = new ActionExpression()
            {
                Command = Command.Set,
                Action = new BinaryExpression()
                {
                    @operator = Operators.@is,
                    Left = new Target()
                    {
                        EntityName = "Activity",
                        PropertyName = "Body"
                    },
                    Right = new ConststantExpression()
                    {
                        Value = "Debel"
                    }
                }
            };
            var mockCommandList = new List<Expression>();
            mockCommandList.Add(mockCommand);

            var mockTarget = new Target()
            {
                EntityName = "Activity",
            };

            var rule = new Rule()
            {
                Actions = mockCommandList,
                Target = mockTarget
            };

            var output = Grammar.Rule.Parse(input);

            Assert.AreEqual(rule.ToString(), output.ToString());
        }

        [TestMethod]
        public void TryParseSimpleRule()
        {

            string input = @"for Activity with Subject = 'Lorena' do set Activity.Body = 'Debel'";

            var output = Grammar.Rule.TryParse(input);

            Assert.IsTrue(output.WasSuccessful);
        }

        [TestMethod]
        public void ParseSimpleRuleSuccessfull()
        {

            string input = @"for Activity with Subject = 'Lorena' do set Subject = 'Lararea'";


            var mockCommand = new ActionExpression()
            {
                Command = Command.Set,
                Action = new BinaryExpression()
                {
                    @operator = Operators.@is,
                    Left = new Target()
                    {
                        PropertyName = "Subject"
                    },
                    Right = new ConststantExpression()
                    {
                        Value = "Lararea"
                    }
                }
            };
            var mockCommandList = new List<Expression>();
            mockCommandList.Add(mockCommand);

            var mockTarget = new BinaryExpression()
            {
                @operator = Operators.@is,
                Left = new Target()
                {
                    EntityName = "Activity",
                    PropertyName = "Subject"
                },
                Right = new ConststantExpression()
                {
                    Value = "Lorena"
                }
            };

            var rule = new Rule()
            {
                Actions = mockCommandList,
                Target = mockTarget
            };

            var output = Grammar.Rule.Parse(input);

            Assert.AreEqual(rule.ToString(), output.ToString());
        }

        [TestMethod]
        public void TryParseComplicatedRule()
        {

            string input = @"for ssad= 7777 or 
                    (((Activity with Subject = 'LUrena' or Body is 'HUR') or ((Name = 'smqh' or Body = 'smqh') or Karami contains 'parafin')) or sss= 'ss')
                    do set Activity.Body = 'Debel' and set Subject = 'Lararea'";

            var output = Grammar.Rule.TryParse(input);

            Assert.IsTrue(output.WasSuccessful);
        }

        [TestMethod]
        public void ParseComplicatedRule()
        {

            string input = @"for ssad = 7777 or 
                    (((Activity with Subject = 'LUrena' or Body is 'HUR') or ((Name = 'smqh' or Body = 'smqh') or Karami contains 'parafin')) or sss = 'ss')
                    do set Activity.Body = 'Debel', set Subject = 'Lararea'";

            #region DEFINE
            var mockCommand = new ActionExpression()
            {
                Command = Command.Set,
                Action = new BinaryExpression()
                {
                    @operator = Operators.@is,
                    Left = new Target()
                    {
                        EntityName = "Activity",
                        PropertyName = "Body"
                    },
                    Right = new ConststantExpression()
                    {
                        Value = "Debel"
                    }
                }
            };

            var mockCommandList = new List<Expression>();
            mockCommandList.Add(mockCommand);

            mockCommand = new ActionExpression()
            {
                Command = Command.Set,
                Action = new BinaryExpression()
                {
                    @operator = Operators.@is,
                    Left = new Target()
                    {
                        PropertyName = "Subject"
                    },
                    Right = new ConststantExpression()
                    {
                        Value = "Lararea"
                    }
                }
            };
            mockCommandList.Add(mockCommand);

            var mock = new BinaryExpression()
            {
                @operator = Operators.or,
                Left = new BinaryExpression()
                {
                    Left = new Target()
                    {
                        PropertyName = "ssad"
                    },
                    @operator = Operators.@is,
                    Right = new ConststantExpression()
                    {
                        Value = 7777.ToString()
                    }
                },
                Right = new BinaryExpression()
                {
                    @operator = Operators.or,
                    Right = new BinaryExpression()
                    {
                        Left = new Target()
                        {
                            PropertyName = "sss"
                        },
                        @operator = Operators.@is,
                        Right = new ConststantExpression()
                        {
                            Value = "ss"
                        }
                    },
                    Left = new BinaryExpression()
                    {
                        Left = new BinaryExpression()
                        {
                            Left = new BinaryExpression()
                            {
                                Left = new Target()
                                {
                                    EntityName = "Activity",
                                    PropertyName = "Subject"
                                },
                                @operator = Operators.@is,
                                Right = new ConststantExpression()
                                {
                                    Value = "LUrena"
                                }
                            },
                            @operator = Operators.or,
                            Right = new BinaryExpression()
                            {
                                Left = new Target()
                                {
                                    PropertyName = "Body"
                                },
                                @operator = Operators.@is,
                                Right = new ConststantExpression()
                                {
                                    Value = "HUR"
                                }
                            }
                        },
                        @operator = Operators.or,
                        Right = new BinaryExpression()
                        {
                            Left = new BinaryExpression()
                            {
                                Left = new BinaryExpression()
                                {
                                    Left = new Target()
                                    {
                                        PropertyName = "Name"
                                    },
                                    @operator = Operators.@is,
                                    Right = new ConststantExpression()
                                    {
                                        Value = "smqh"
                                    }
                                },
                                @operator = Operators.or,
                                Right = new BinaryExpression()
                                {
                                    Left = new Target()
                                    {
                                        PropertyName = "Body"
                                    },
                                    @operator = Operators.@is,
                                    Right = new ConststantExpression()
                                    {
                                        Value = "smqh"
                                    }
                                }
                            },
                            @operator = Operators.or,
                            Right = new BinaryExpression()
                            {
                                Left = new Target()
                                {
                                    PropertyName = "Karami"
                                },
                                @operator = Operators.contains,
                                Right = new ConststantExpression()
                                {
                                    Value = "parafin"
                                }
                            }
                        }
                    }
                }
            };

            var mockRule = new Rule()
            {
                Actions = mockCommandList,
                Target = mock
            };
            #endregion

            var output = Grammar.Rule.Parse(input);

            Assert.AreEqual(mockRule.ToString().Trim(), output.ToString().Trim());
        }

        [TestMethod]
        public void TryParseRuleWithDecmalAndActionToAllRelatedCommand()
        {

            string input = @"for Activity with Subject = 2.4 do set Document.Category = 'Debel' ToAllRelated Activities";

            var output = Grammar.Rule.TryParse(input);

            Assert.IsTrue(output.WasSuccessful);
        }

        [TestMethod]
        public void ParseRuleWithDecmalAndActionToAllRelatedCommand()
        {

            string input = @"for Activity with Subject = 24 do set Document.Category = 'Debel' ToAllRelated Activities";

            var mockCommand = new ActionExpression()
            {
                Command = Command.Set,
                Action = new BinaryExpression()
                {
                    @operator = Operators.ToAllRelated,
                    Left = new BinaryExpression()
                    {
                        @operator = Operators.@is,
                        Left = new Target()
                        {
                            EntityName = "Document",
                            PropertyName = "Category"
                        },
                        Right = new ConststantExpression()
                        {
                            Value = "Debel"
                        }
                    },
                    Right = new ConststantExpression()
                    {
                        Value = "Activities"
                    }
                }
            };
            var mockCommandList = new List<Expression>();
            mockCommandList.Add(mockCommand);

            var mockTarget = new BinaryExpression()
            {
                @operator = Operators.@is,
                Left = new Target()
                {
                    EntityName = "Activity",
                    PropertyName = "Subject"
                },
                Right = new ConststantExpression()
                {
                    Value = 24.ToString()
                }
            };

            var mockrule = new Rule()
            {
                Actions = mockCommandList,
                Target = mockTarget
            };

            var output = Grammar.Rule.Parse(input);

            Assert.AreEqual(mockrule.ToString().Trim(), output.ToString().Trim());
        }
    }

    [TestClass]
    public class ToAdvfTests
    {
        [TestMethod]
        public void ExtractSimpleRuleAdvf()
        {
            string input = @"for Activity with Subject = 'DevNote' do set Document.Category = 'Debel' ToAllRelated Activities";
            var expectedAdvf = @"<advf><e name=""Activity""><rule prop=""Subject"" op=""is""><values><v>DevNote</v></values></rule></e></advf>";

            var output = Grammar.Rule.Parse(input);
            var actualAdvf = D6S.Tool.RulesV2.RuleOperations.ExtractEntitiesAdfv(output);

            Assert.AreEqual(expectedAdvf, actualAdvf);
        }

        [TestMethod]
        public void ExtractRuleAdvfWithTwoConditionAndOperatorAndBetweenThem()
        {
            string input = @"for Activity with Subject = 24 and Body = 10 do set Document.Category = 'Debel' ToAllRelated Activities";
            var expectedAdvf = @"<advf><e name=""Activity""><rule prop=""Subject"" op=""is""><values><v>24</v></values></rule><rule prop=""Body"" op=""is""><values><v>10</v></values></rule></e></advf>";

            var output = Grammar.Rule.Parse(input);
            var actualAdvf = D6S.Tool.RulesV2.RuleOperations.ExtractEntitiesAdfv(output);

            Assert.AreEqual(expectedAdvf, actualAdvf);
        }

        [TestMethod]
        public void ExtractRuleAdvfWithTwoConditionAndOperatorOrBetweenThem()
        {
            string input = @"for Activity with Subject = 24 or Body = 10 do set Document.Category = 'Debel' ToAllRelated Activities";
            var expectedAdvf = @"<advf><e name=""Activity""><rule prop=""Subject"" op=""is""><values><v>24</v></values></rule></e><e name=""Activity""><rule prop=""Body"" op=""is""><values><v>10</v></values></rule></e></advf>";

            var output = Grammar.Rule.Parse(input);
            var actualAdvf = D6S.Tool.RulesV2.RuleOperations.ExtractEntitiesAdfv(output);

            Assert.AreEqual(expectedAdvf, actualAdvf);
        }

        [TestMethod]
        public void ExtractFromRuleWithOnlyMainEntityToAdvf()
        {
            string input = @"for Activity do set Document.Category = 'Debel' ToAllRelated Activities";
            var expectedAdvf = @"<advf><e name=""Activity""></e></advf>";

            var output = Grammar.Rule.Parse(input);
            var actualAdvf = D6S.Tool.RulesV2.RuleOperations.ExtractEntitiesAdfv(output);

            Assert.AreEqual(expectedAdvf, actualAdvf);
        }

        [TestMethod]
        public void TryExtractFromRuleWithoutEntityThrowsNullReferenceExeption()
        {
            string input = @"for subject = 'Vazov' do set Document.Category = 'Debel' ToAllRelated Activities";

            var output = Grammar.Rule.Parse(input);
            Assert.ThrowsException<NullReferenceException>(() => (D6S.Tool.RulesV2.RuleOperations.ExtractEntitiesAdfv(output)));
        }

        //Todo
        [TestMethod]
        public void ExtractRuleAdvfWithThreeCondition()
        {
            string input = @"for Activity.Subject = 'Magapasa' and (Activity with From = 'Maya' or From = 'Georgi') do set Document.Category = 'Debel' ToAllRelated Activities";
            var expectedAdvf = @"";

            var output = Grammar.Rule.Parse(input);
            var actualAdvf = D6S.Tool.RulesV2.RuleOperations.ExtractEntitiesAdfv(output);
            
        }
    }

}

