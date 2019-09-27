using System.Threading.Tasks;
using NUnit.Framework;

namespace SafeApp.Tests
{
    [TestFixture]
    internal class Nrs
    {
        [Test]
        public async Task ParseUrlTest()
        {
            var session = await TestUtils.CreateTestApp();
            var (xorurl, _) = await session.Keys.KeysCreatePreloadTestCoinsAsync("1");

            var api = session.Nrs;
            var xorUrlEncoder = await api.ParseUrlAsync(xorurl);
        }

        [Test]
        public async Task ParseAndResolveUrlTest()
        {
            var session = await TestUtils.CreateTestApp();
            var (xorurl, _) = await session.Keys.KeysCreatePreloadTestCoinsAsync("1");

            var api = session.Nrs;
            var (xorUrlEncoder, resolvesAsNrs) = await api.ParseAndResolveUrlAsync(xorurl);
        }

        [Test]
        public async Task CreateNrsMapContainerTest()
        {
            var session = await TestUtils.CreateTestApp();
            var name = "someName";

            // todo: put file, use its address as link
            // in the meanwhile put the file through cli.
            var link = "safe://hnyynys9j9sd6ku5wu9pz6uodmwk5446e1ee7u4x5gtbhocmastfcjuiksbnc?v=0";

            var directLink = true;
            var dryRun = false;
            var setDefault = true;

            var api = session.Nrs;
            var (nrsMap, xorUrl) = await api.CreateNrsMapContainerAsync(name, link, directLink, dryRun, setDefault);

            Assert.IsNotNull(nrsMap.SubNamesMap);
            Assert.IsNotEmpty(nrsMap.SubNamesMap.SubNames);
            Assert.IsNotNull(nrsMap.Default);
            nrsMap.SubNamesMap.SubNames.ForEach(s =>
            {
                Assert.IsNotNull(s.SubName);
                Assert.IsNotNull(s.SubNameRdf);
            });
            Assert.IsNotNull(xorUrl);
        }

        [Test]
        public async Task AddToNrsMapContainerTest()
        {
            var session = await TestUtils.CreateTestApp();
            var name = "someName";

            // todo: put file, use its address as link
            // in the meanwhile put the file through cli.
            var link = "safe://hnyynys9j9sd6ku5wu9pz6uodmwk5446e1ee7u4x5gtbhocmastfcjuiksbnc?v=0";

            var setDefault = true;
            var directLink = true;
            var dryRun = false;

            var api = session.Nrs;
            var (nrsMap, xorUrl, version) = await api.AddToNrsMapContainerAsync(name, link, setDefault, directLink, dryRun);

            Assert.IsNotNull(nrsMap.SubNamesMap);
            Assert.IsNotEmpty(nrsMap.SubNamesMap.SubNames);
            Assert.IsNotNull(nrsMap.Default);
            nrsMap.SubNamesMap.SubNames.ForEach(s =>
            {
                Assert.IsNotNull(s.SubName);
                Assert.IsNotNull(s.SubNameRdf);
            });
            Assert.IsNotNull(xorUrl);

            // todo: validate version
        }
    }
}
