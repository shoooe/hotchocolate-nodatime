using System.Linq;
using HotChocolate.Execution;
using HotChocolate.Types.NodaTime.Extensions;
using NodaTime;
using Xunit;

namespace HotChocolate.Types.NodaTime.Tests
{
    public class DateTimeZoneTypeIntegrationTests
    {
        public static class Schema
        {
            public class Query
            {
                public DateTimeZone Utc => DateTimeZone.Utc;
                public DateTimeZone Rome => DateTimeZoneProviders.Tzdb["Europe/Rome"];
                public DateTimeZone Chihuahua => DateTimeZoneProviders.Tzdb["America/Chihuahua"];
            }

            public class Mutation
            {
                public string Test(DateTimeZone arg)
                {
                    return arg.Id;
                }
            }
        }

        private readonly IQueryExecutor testExecutor;
        public DateTimeZoneTypeIntegrationTests()
        {
            testExecutor = SchemaBuilder.New()
                .AddQueryType<Schema.Query>()
                .AddMutationType<Schema.Mutation>()
                .AddNodaTime()
                .Create()
                .MakeExecutable();
        }

        [Fact]
        public void QueryReturnsUtc()
        {
            var result = testExecutor.Execute("query { test: utc }");
            var queryResult = result as IReadOnlyQueryResult;
            Assert.Equal("UTC", queryResult!.Data["test"]);
        }

        [Fact]
        public void QueryReturnsRome()
        {
            var result = testExecutor.Execute("query { test: rome }");
            var queryResult = result as IReadOnlyQueryResult;
            Assert.Equal("Europe/Rome", queryResult!.Data["test"]);
        }

        [Fact]
        public void QueryReturnsChihuahua()
        {
            var result = testExecutor.Execute("query { test: chihuahua }");
            var queryResult = result as IReadOnlyQueryResult;
            Assert.Equal("America/Chihuahua", queryResult!.Data["test"]);
        }

        [Fact]
        public void ParsesVariable()
        {
            var result = testExecutor
                .Execute(QueryRequestBuilder.New()
                    .SetQuery("mutation($arg: DateTimeZone!) { test(arg: $arg) }")
                    .SetVariableValue("arg", "Europe/Amsterdam")
                    .Create());
            var queryResult = result as IReadOnlyQueryResult;
            Assert.Equal("Europe/Amsterdam", queryResult!.Data["test"]);
        }

        [Fact]
        public void DoesntParseIncorrectVariable()
        {
            var result = testExecutor
                .Execute(QueryRequestBuilder.New()
                    .SetQuery("mutation($arg: DateTimeZone!) { test(arg: $arg) }")
                    .SetVariableValue("arg", "Europe/Hamster")
                    .Create());
            var queryResult = result as IReadOnlyQueryResult;
            Assert.DoesNotContain("test", queryResult!.Data);
            Assert.Equal(1, queryResult.Errors.Count);
            Assert.Equal("EXEC_INVALID_TYPE", queryResult.Errors.First().Code);
        }

        [Fact]
        public void ParsesLiteral()
        {
            var result = testExecutor
                .Execute(QueryRequestBuilder.New()
                    .SetQuery("mutation { test(arg: \"Europe/Amsterdam\") }")
                    .Create());
            var queryResult = result as IReadOnlyQueryResult;
            Assert.Equal("Europe/Amsterdam", queryResult!.Data["test"]);
        }

        [Fact]
        public void DoesntParseIncorrectLiteral()
        {
            var result = testExecutor
                .Execute(QueryRequestBuilder.New()
                    .SetQuery("mutation { test(arg: \"Europe/Hamster\") }")
                    .Create());
            var queryResult = result as IReadOnlyQueryResult;
            Assert.DoesNotContain("test", queryResult!.Data);
            Assert.Equal(1, queryResult.Errors.Count);
            Assert.Null(queryResult.Errors.First().Code);
            Assert.Equal("Unable to deserialize string to DateTimeZone", queryResult.Errors.First().Message);
        }
    }
}
