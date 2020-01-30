using System;
using HotChocolate.Execution;
using HotChocolate.Types.NodaTime.Extensions;
using NodaTime;
using Xunit;

namespace HotChocolate.Types.NodaTime.Tests
{
    public class IsoDayOfWeekTypeIntegrationTests
    {
        public static class Schema
        {
            public class Query
            {
                public IsoDayOfWeek Monday => IsoDayOfWeek.Monday;
                public IsoDayOfWeek Sunday => IsoDayOfWeek.Sunday;
                public IsoDayOfWeek Friday => IsoDayOfWeek.Friday;
                public IsoDayOfWeek None => IsoDayOfWeek.None;
            }

            public class Mutation
            {
                public IsoDayOfWeek Test(IsoDayOfWeek arg)
                {
                    var intRepr = (int)arg;
                    var nextIntRepr = Math.Max(1, (intRepr + 1) % 8);
                    return (IsoDayOfWeek)nextIntRepr;
                }
            }
        }

        private readonly IQueryExecutor testExecutor;
        public IsoDayOfWeekTypeIntegrationTests()
        {
            testExecutor = SchemaBuilder.New()
                .AddQueryType<Schema.Query>()
                .AddMutationType<Schema.Mutation>()
                .AddNodaTime()
                .Create()
                .MakeExecutable();
        }

        [Fact]
        public void QueryReturnsMonday()
        {
            var result = testExecutor.Execute("query { test: monday }");
            var queryResult = result as IReadOnlyQueryResult;
            Assert.Equal(1, queryResult.Data["test"]);
        }

        [Fact]
        public void QueryReturnsSunday()
        {
            var result = testExecutor.Execute("query { test: sunday }");
            var queryResult = result as IReadOnlyQueryResult;
            Assert.Equal(7, queryResult.Data["test"]);
        }

        [Fact]
        public void QueryReturnsFriday()
        {
            var result = testExecutor.Execute("query { test: friday }");
            var queryResult = result as IReadOnlyQueryResult;
            Assert.Equal(5, queryResult.Data["test"]);
        }

        [Fact]
        public void QueryDoesntReturnNone()
        {
            var result = testExecutor.Execute("query { test: none }");
            var queryResult = result as IReadOnlyQueryResult;
            Assert.DoesNotContain("test", queryResult.Data);
            Assert.NotEmpty(queryResult.Errors);
        }

        [Fact]
        public void MutationParsesMonday()
        {
            var result = testExecutor
                .Execute(QueryRequestBuilder.New()
                    .SetQuery("mutation($arg: IsoDayOfWeek!) { test(arg: $arg) }")
                    .SetVariableValue("arg", 1)
                    .Create());
            var queryResult = result as IReadOnlyQueryResult;
            Assert.Equal(2, queryResult.Data["test"]);
        }

        [Fact]
        public void MutationParsesSunday()
        {
            var result = testExecutor
                .Execute(QueryRequestBuilder.New()
                    .SetQuery("mutation($arg: IsoDayOfWeek!) { test(arg: $arg) }")
                    .SetVariableValue("arg", 7)
                    .Create());
            var queryResult = result as IReadOnlyQueryResult;
            Assert.Equal(1, queryResult.Data["test"]);
        }

        [Fact]
        public void MutationDoesntParseZero()
        {
            var result = testExecutor
                .Execute(QueryRequestBuilder.New()
                    .SetQuery("mutation($arg: IsoDayOfWeek!) { test(arg: $arg) }")
                    .SetVariableValue("arg", 0)
                    .Create());
            var queryResult = result as IReadOnlyQueryResult;
            Assert.DoesNotContain("test", queryResult.Data);
            Assert.NotEmpty(queryResult.Errors);
        }

        [Fact]
        public void MutationDoesntParseEight()
        {
            var result = testExecutor
                .Execute(QueryRequestBuilder.New()
                    .SetQuery("mutation($arg: IsoDayOfWeek!) { test(arg: $arg) }")
                    .SetVariableValue("arg", 8)
                    .Create());
            var queryResult = result as IReadOnlyQueryResult;
            Assert.DoesNotContain("test", queryResult.Data);
            Assert.NotEmpty(queryResult.Errors);
        }

        [Fact]
        public void MutationDoesntParseNegativeNumbers()
        {
            var result = testExecutor
                .Execute(QueryRequestBuilder.New()
                    .SetQuery("mutation($arg: IsoDayOfWeek!) { test(arg: $arg) }")
                    .SetVariableValue("arg", -2)
                    .Create());
            var queryResult = result as IReadOnlyQueryResult;
            Assert.DoesNotContain("test", queryResult.Data);
            Assert.NotEmpty(queryResult.Errors);
        }
    }
}
