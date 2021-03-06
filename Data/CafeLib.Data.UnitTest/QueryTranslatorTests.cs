using System.Collections.Generic;
using System.Linq;
using CafeLib.Data.SqlGenerator;
using CafeLib.Data.SqlGenerator.Models;
using CafeLib.Data.UnitTest.DbObjects;
using CafeLib.Data.UnitTest.TestDomain;
using Xunit;

namespace CafeLib.Data.UnitTest
{
    public class QueryTranslatorTests
    {
        [Fact]
        public void QueryTranslator_Translate_Where_GroupBy_Select()
        {
            var domain = new TestDomain.TestDomain();

            var query = new List<Post>().AsQueryable().
                Where(p => p.Content != null).
                GroupBy(p => p.BlogId).
                Select(g => new { cnt = g.Count() });

            var script = QueryTranslator.Translate(query.Expression, new EntityModelInfoProvider(domain), new SqliteObjectFactory());
            var sql = script.ToString();

            const string expected = @"
select count(1) as 'cnt'
from Post p0
where p0.Content is not null
group by p0.BlogId";

            TestUtils.AssertStringEqual(expected, sql);
        }

        [Fact]
        public void QueryTranslator_Translate_Where()
        {
            var domain = new TestDomain.TestDomain();

            var query = new List<Post>().AsQueryable()
                .Where(p => p.Content != null);

            var script = QueryTranslator.Translate(query.Expression, new EntityModelInfoProvider(domain), new SqliteObjectFactory());
            var sql = script.ToString();

            const string expected = @"
select p0.* from Post p0
where p0.Content is not null";

            TestUtils.AssertStringEqual(expected, sql);
        }

    }
}
