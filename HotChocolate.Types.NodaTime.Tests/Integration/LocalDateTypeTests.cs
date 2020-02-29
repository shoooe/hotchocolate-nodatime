using System;
using System.Linq;
using HotChocolate.Execution;
using HotChocolate.Types.NodaTime.Extensions;
using NodaTime;
using Xunit;

namespace HotChocolate.Types.NodaTime.Tests
{
    public class LocalDateTypeIntegrationTests
    {
        public static class Schema
        {
            public class Query
            {
                public LocalDate One => LocalDate.FromDateTime(new DateTime(2020, 02, 20, 17, 42, 59));
            }

            public class Mutation
            {
                public LocalDate Test(LocalDate arg)
                {
                    return arg + Period.FromDays(3);
                }
            }
        }

        private readonly IQueryExecutor testExecutor;
        public LocalDateTypeIntegrationTests()
        {
            testExecutor = SchemaBuilder.New()
                .AddQueryType<Schema.Query>()
                .AddMutationType<Schema.Mutation>()
                .AddNodaTime()
                .Create()
                .MakeExecutable();
        }

        [Fact]
        public void QueryReturns()
        {
            var result = testExecutor.Execute("query { test: one }");
            var queryResult = result as IReadOnlyQueryResult;
            Assert.Equal("2020-02-20", queryResult.Data["test"]);
        }

        [Fact]
        public void ParsesVariable()
        {
            var result = testExecutor
                .Execute(QueryRequestBuilder.New()
                    .SetQuery("mutation($arg: LocalDate!) { test(arg: $arg) }")
                    .SetVariableValue("arg", "2020-02-21")
                    .Create());
            var queryResult = result as IReadOnlyQueryResult;
            Assert.Equal("2020-02-24", queryResult.Data["test"]);
        }

        [Fact]
        public void DoesntParseAnIncorrectVariable()
        {
            var result = testExecutor
                .Execute(QueryRequestBuilder.New()
                    .SetQuery("mutation($arg: LocalDate!) { test(arg: $arg) }")
                    .SetVariableValue("arg", "2020-02-20T17:42:59")
                    .Create());
            var queryResult = result as IReadOnlyQueryResult;
            Assert.DoesNotContain("test", queryResult.Data);
            Assert.Equal(1, queryResult.Errors.Count);
            Assert.Equal("EXEC_INVALID_TYPE", queryResult.Errors.First().Code);
        }

        [Fact]
        public void ParsesLiteral()
        {
            var result = testExecutor
                .Execute(QueryRequestBuilder.New()
                    .SetQuery("mutation { test(arg: \"2020-02-20\") }")
                    .Create());
            var queryResult = result as IReadOnlyQueryResult;
            Assert.Equal("2020-02-23", queryResult.Data["test"]);
        }

        [Fact]
        public void DoesntParseIncorrectLiteral()
        {
            var result = testExecutor
                .Execute(QueryRequestBuilder.New()
                    .SetQuery("mutation { test(arg: \"2020-02-20T17:42:59\") }")
                    .Create());
            var queryResult = result as IReadOnlyQueryResult;
            Assert.DoesNotContain("test", queryResult.Data);
            Assert.Equal(1, queryResult.Errors.Count);
            Assert.Null(queryResult.Errors.First().Code);
            Assert.Equal("Unable to deserialize string to LocalDate", queryResult.Errors.First().Message);
        }
    }
}
