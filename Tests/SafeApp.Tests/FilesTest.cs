using System;
using System.Threading.Tasks;
using NUnit.Framework;

namespace SafeApp.Tests
{
    [TestFixture]
    internal class FilesTest
    {
        [Test]
        public async Task CreateFilesContainerTest()
        {
            var session = await TestUtils.CreateTestApp();
            await session.Keys.KeysCreatePreloadTestCoinsAsync("1");

            var api = session.Files;
            var (xorUrl, processedFiles, filesMap) = await api.CreateFilesContainerAsync(
                @"../../../",
                null,
                false,
                false);

            Assert.IsNotNull(xorUrl);
            Assert.AreNotEqual(string.Empty, xorUrl, $"{nameof(xorUrl)} is empty!");
            Assert.IsTrue(xorUrl.StartsWith("safe://", StringComparison.Ordinal));

            Assert.IsNotNull(processedFiles.Files);
            Assert.AreEqual(1, processedFiles.Files.Count, $"{nameof(processedFiles)} is empty!");
            processedFiles.Files.ForEach(
                c =>
                {
                    // Assert.IsNotNull(c.FileMetadata);
                    Assert.IsNotNull(c.FileName);
                    Assert.IsNotNull(c.FileXorUrl);

                    // Assert.AreNotEqual(string.Empty, c.FileMetadata, $"{nameof(processedFiles)} {nameof(c.FileMetadata)} is empty!");
                    Assert.AreNotEqual(string.Empty, c.FileName, $"{nameof(processedFiles)} {nameof(c.FileName)} is empty!");
                    Assert.AreNotEqual(string.Empty, c.FileXorUrl, $"{nameof(processedFiles)} {nameof(c.FileXorUrl)} is empty!");

                    Assert.IsTrue(c.FileName.EndsWith(".csproj"));
                });

            Assert.IsNotNull(filesMap.Files);
            Assert.AreEqual(1, filesMap.Files.Count, $"{nameof(filesMap)} is empty!");
            filesMap.Files.ForEach(
                c =>
                {
                    // Assert.IsNotNull(c.FileMetadata);
                    Assert.IsNotNull(c.FileXorUrl);

                    // Assert.AreNotEqual(string.Empty, c.FileMetadata, $"{nameof(filesMap)} {nameof(c.FileMetadata)} is empty!");
                    Assert.AreNotEqual(string.Empty, c.FileXorUrl, $"{nameof(filesMap)} {nameof(c.FileXorUrl)} is empty!");
                });
        }
    }
}
