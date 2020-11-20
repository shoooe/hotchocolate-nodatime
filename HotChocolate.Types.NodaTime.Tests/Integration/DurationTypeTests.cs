using System;
using System.Linq;
using HotChocolate.Execution;
using HotChocolate.Types.NodaTime.Extensions;
using NodaTime;
using Xunit;

namespace HotChocolate.Types.NodaTime.Tests
{
    public class DurationTypeIntegrationTests
    {
        public static class Schema
        {
            public class Query
            {
                public Duration PositiveWithDecimals => Duration.FromTimeSpan(new TimeSpan(123, 7, 53, 10, 19));
                public Duration NegativeWithDecimals => -Duration.FromTimeSpan(new TimeSpan(123, 7, 53, 10, 19));
                public Duration PositiveWithoutDecimals => Duration.FromTimeSpan(new TimeSpan(123, 7, 53, 10));
                public Duration PositiveWithoutSeconds => Duration.FromTimeSpan(new TimeSpan(123, 7, 53, 0));
                public Duration PositiveWithoutMinutes => Duration.FromTimeSpan(new TimeSpan(123, 7, 0, 0));
                public Duration PositiveWithRoundtrip => Duration.FromTimeSpan(new TimeSpan(123, 26, 0, 70));
            }

            public class Mutation
            {
                public Duration Test(Duration arg)
                    => arg + Duration.FromMinutes(10);
            }
        }

        private readonly IQueryExecutor testExecutor;
        public DurationTypeIntegrationTests()
        {
            testExecutor = SchemaBuilder.New()
                .AddQueryType<Schema.Query>()
                .AddMutationType<Schema.Mutation>()
                .AddNodaTime()
                .Create()
                .MakeExecutable();
        }

        [Fact]
        public void QueryReturnsSerializedDataWithDecimals()
        {
            var result = testExecutor.Execute("query { test: positiveWithDecimals }");
            var queryResult = result as IReadOnlyQueryResult;
            Assert.Equal("123:07:53:10.019", queryResult!.Data["test"]);
        }

        [Fact]
        public void QueryReturnsSerializedDataWithNegativeValue()
        {
            var result = testExecutor.Execute("query { test: negativeWithDecimals }");
            var queryResult = result as IReadOnlyQueryResult;
            Assert.Equal("-123:07:53:10.019", queryResult!.Data["test"]);
        }

        [Fact]
        public void QueryReturnsSerializedDataWithoutDecimals()
        {
            var result = testExecutor.Execute("query { test: positiveWithoutDecimals }");
            var queryResult = result as IReadOnlyQueryResult;
            Assert.Equal("123:07:53:10", queryResult!.Data["test"]);
        }

        [Fact]
        public void QueryReturnsSerializedDataWithoutSeconds()
        {
            var result = testExecutor.Execute("query { test: positiveWithoutSeconds }");
            var queryResult = result as IReadOnlyQueryResult;
            Assert.Equal("123:07:53:00", queryResult!.Data["test"]);
        }

        [Fact]
        public void QueryReturnsSerializedDataWithoutMinutes()
        {
            var result = testExecutor.Execute("query { test: positiveWithoutMinutes }");
            var queryResult = result as IReadOnlyQueryResult;
            Assert.Equal("123:07:00:00", queryResult!.Data["test"]);
        }

        [Fact]
        public void QueryReturnsSerializedDataWithRoundtrip()
        {
            var result = testExecutor.Execute("query { test: positiveWithRoundtrip }");
            var queryResult = result as IReadOnlyQueryResult;
            Assert.Equal("124:02:01:10", queryResult!.Data["test"]);
        }

        [Fact]
        public void MutationParsesInputWithDecimals()
        {
            var result = testExecutor
                .Execute(QueryRequestBuilder.New()
                    .SetQuery("mutation($arg: Duration!) { test(arg: $arg) }")
                    .SetVariableValue("arg", "09:22:01:00.019")
                    .Create());
            var queryResult = result as IReadOnlyQueryResult;
            Assert.Equal("9:22:11:00.019", queryResult!.Data["test"]);
        }

        [Fact]
        public void MutationParsesInputWithoutDecimals()
        {
            var result = testExecutor
                .Execute(QueryRequestBuilder.New()
                    .SetQuery("mutation($arg: Duration!) { test(arg: $arg) }")
                    .SetVariableValue("arg", "09:22:01:00")
                    .Create());
            var queryResult = result as IReadOnlyQueryResult;
            Assert.Equal("9:22:11:00", queryResult!.Data["test"]);
        }

        [Fact]
        public void MutationParsesInputWithoutLeadingZero()
        {
            var result = testExecutor
                .Execute(QueryRequestBuilder.New()
                    .SetQuery("mutation($arg: Duration!) { test(arg: $arg) }")
                    .SetVariableValue("arg", "9:22:01:00")
                    .Create());
            var queryResult = result as IReadOnlyQueryResult;
            Assert.Equal("9:22:11:00", queryResult!.Data["test"]);
        }

        [Fact]
        public void MutationParsesInputWithNegativeValue()
        {
            var result = testExecutor
                .Execute(QueryRequestBuilder.New()
                    .SetQuery("mutation($arg: Duration!) { test(arg: $arg) }")
                    .SetVariableValue("arg", "-9:22:01:00")
                    .Create());
            var queryResult = result as IReadOnlyQueryResult;
            Assert.Equal("-9:21:51:00", queryResult!.Data["test"]);
        }

        [Fact]
        public void MutationDoesntParseInputWithPlusSign()
        {
            var result = testExecutor
                .Execute(QueryRequestBuilder.New()
                    .SetQuery("mutation($arg: Duration!) { test(arg: $arg) }")
                    .SetVariableValue("arg", "+09:22:01:00")
                    .Create());
            var queryResult = result as IReadOnlyQueryResult;
            Assert.DoesNotContain("test", queryResult!.Data);
            Assert.Equal(1, queryResult.Errors.Count);
            Assert.Equal("EXEC_INVALID_TYPE", queryResult.Errors.First().Code);
        }

        [Fact]
        public void MutationDoesntParseInputWithOverflownHours()
        {
            var result = testExecutor
                .Execute(QueryRequestBuilder.New()
                    .SetQuery("mutation($arg: Duration!) { test(arg: $arg) }")
                    .SetVariableValue("arg", "9:26:01:00")
                    .Create());
            var queryResult = result as IReadOnlyQueryResult;
            Assert.DoesNotContain("test", queryResult!.Data);
            Assert.Equal(1, queryResult.Errors.Count);
            Assert.Equal("EXEC_INVALID_TYPE", queryResult.Errors.First().Code);
        }

        [Fact]
        public void MutationParsesLiteralWithDecimals()
        {
            var result = testExecutor
                .Execute(QueryRequestBuilder.New()
                    .SetQuery("mutation { test(arg: \"09:22:01:00.019\") }")
                    .Create());
            var queryResult = result as IReadOnlyQueryResult;
            Assert.Equal("9:22:11:00.019", queryResult!.Data["test"]);
        }

        [Fact]
        public void MutationParsesLiteralWithoutDecimals()
        {
            var result = testExecutor
                .Execute(QueryRequestBuilder.New()
                    .SetQuery("mutation { test(arg: \"09:22:01:00\") }")
                    .Create());
            var queryResult = result as IReadOnlyQueryResult;
            Assert.Equal("9:22:11:00", queryResult!.Data["test"]);
        }

        [Fact]
        public void MutationParsesLiteralWithoutLeadingZero()
        {
            var result = testExecutor
                .Execute(QueryRequestBuilder.New()
                    .SetQuery("mutation { test(arg: \"09:22:01:00\") }")
                    .Create());
            var queryResult = result as IReadOnlyQueryResult;
            Assert.Equal("9:22:11:00", queryResult!.Data["test"]);
        }

        [Fact]
        public void MutationParsesLiteralWithNegativeValue()
        {
            var result = testExecutor
                .Execute(QueryRequestBuilder.New()
                    .SetQuery("mutation { test(arg: \"-9:22:01:00\") }")
                    .Create());
            var queryResult = result as IReadOnlyQueryResult;
            Assert.Equal("-9:21:51:00", queryResult!.Data["test"]);
        }

        [Fact]
        public void MutationDoesntParseLiteralWithPlusSign()
        {
            var result = testExecutor
                .Execute(QueryRequestBuilder.New()
                    .SetQuery("mutation { test(arg: \"+09:22:01:00\") }")
                    .Create());
            var queryResult = result as IReadOnlyQueryResult;
            Assert.DoesNotContain("test", queryResult!.Data);
            Assert.Equal(1, queryResult.Errors.Count);
            Assert.Equal("arg", queryResult.Errors.First().Extensions["argument"].ToString());
        }

        [Fact]
        public void MutationDoesntParseLiteralWithOverflownHours()
        {
            var result = testExecutor
                .Execute(QueryRequestBuilder.New()
                    .SetQuery("mutation { test(arg: \"9:26:01:00\") }")
                    .Create());
            var queryResult = result as IReadOnlyQueryResult;
            Assert.DoesNotContain("test", queryResult!.Data);
            Assert.Equal(1, queryResult.Errors.Count);
            Assert.Equal("arg", queryResult.Errors.First().Extensions["argument"].ToString());
        }
    }
}
