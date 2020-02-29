using System;
using System.Linq;
using HotChocolate.Execution;
using HotChocolate.Types.NodaTime.Extensions;
using NodaTime;
using Xunit;

namespace HotChocolate.Types.NodaTime.Tests
{
    public class OffsetDateTimeTypeIntegrationTests
    {
        public static class Schema
        {
            public class Query
            {
                public OffsetDateTime Hour =>
                    OffsetDateTime.FromDateTimeOffset(
                        new DateTimeOffset(2020, 12, 31, 18, 30, 13, TimeSpan.FromHours(2)));

                public OffsetDateTime HourAndMinutes =>
                    OffsetDateTime.FromDateTimeOffset(
                        new DateTimeOffset(2020, 12, 31, 18, 30, 13, TimeSpan.FromHours(2) + TimeSpan.FromMinutes(30)));
            }

            public class Mutation
            {
                public OffsetDateTime Test(OffsetDateTime arg)
                {
                    return arg + Duration.FromMinutes(10);
                }
            }
        }

        private readonly IQueryExecutor testExecutor;
        public OffsetDateTimeTypeIntegrationTests()
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
            var result = testExecutor.Execute("query { test: hour }");
            var queryResult = result as IReadOnlyQueryResult;
            Assert.Equal("2020-12-31T18:30:13+02", queryResult.Data["test"]);
        }

        [Fact]
        public void QueryReturnsWithMinutes()
        {
            var result = testExecutor.Execute("query { test: hourAndMinutes }");
            var queryResult = result as IReadOnlyQueryResult;
            Assert.Equal("2020-12-31T18:30:13+02:30", queryResult.Data["test"]);
        }

        [Fact]
        public void ParsesVariable()
        {
            var result = testExecutor
                .Execute(QueryRequestBuilder.New()
                    .SetQuery("mutation($arg: OffsetDateTime!) { test(arg: $arg) }")
                    .SetVariableValue("arg", "2020-12-31T18:30:13+02")
                    .Create());
            var queryResult = result as IReadOnlyQueryResult;
            Assert.Equal("2020-12-31T18:40:13+02", queryResult.Data["test"]);
        }

        [Fact]
        public void ParsesVariableWithMinutes()
        {
            var result = testExecutor
                .Execute(QueryRequestBuilder.New()
                    .SetQuery("mutation($arg: OffsetDateTime!) { test(arg: $arg) }")
                    .SetVariableValue("arg", "2020-12-31T18:30:13+02:35")
                    .Create());
            var queryResult = result as IReadOnlyQueryResult;
            Assert.Equal("2020-12-31T18:40:13+02:35", queryResult.Data["test"]);
        }

        [Fact]
        public void DoesntParseAnIncorrectVariable()
        {
            var result = testExecutor
                .Execute(QueryRequestBuilder.New()
                    .SetQuery("mutation($arg: OffsetDateTime!) { test(arg: $arg) }")
                    .SetVariableValue("arg", "2020-12-31T18:30:13")
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
                    .SetQuery("mutation { test(arg: \"2020-12-31T18:30:13+02\") }")
                    .Create());
            var queryResult = result as IReadOnlyQueryResult;
            Assert.Equal("2020-12-31T18:40:13+02", queryResult.Data["test"]);
        }

        [Fact]
        public void ParsesLiteralWithMinutes()
        {
            var result = testExecutor
                .Execute(QueryRequestBuilder.New()
                    .SetQuery("mutation { test(arg: \"2020-12-31T18:30:13+02:35\") }")
                    .Create());
            var queryResult = result as IReadOnlyQueryResult;
            Assert.Equal("2020-12-31T18:40:13+02:35", queryResult.Data["test"]);
        }

        [Fact]
        public void DoesntParseIncorrectLiteral()
        {
            var result = testExecutor
                .Execute(QueryRequestBuilder.New()
                    .SetQuery("mutation { test(arg: \"2020-12-31T18:30:13\") }")
                    .Create());
            var queryResult = result as IReadOnlyQueryResult;
            Assert.DoesNotContain("test", queryResult.Data);
            Assert.Equal(1, queryResult.Errors.Count);
            Assert.Null(queryResult.Errors.First().Code);
            Assert.Equal("Unable to deserialize string to OffsetDateTime", queryResult.Errors.First().Message);
        }
    }
}
