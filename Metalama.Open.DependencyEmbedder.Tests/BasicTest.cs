using System.Diagnostics;
using System.IO;
using System.Linq;
using Xunit;

namespace Caravela.Open.DependencyEmbedder.Tests
{
    public class BasicTest
    {
        private const string folder = @"..\net472\";

        [Fact]
        public void TestTestAssemblyWithReferences()
        {
            DeleteAllButExes(folder);
            var filename = "Caravela.Open.DependencyEmbedder.TestApp.exe";
            var p = Process.Start(Path.Combine(folder, filename));
            Assert.True(p.WaitForExit(5000));
            Assert.Equal(0, p.ExitCode);
        }

        [Fact]
        public void TestWPF()
        {
            DeleteAllButExes(folder);
            var filename = "Caravela.Open.DependencyEmbedder.WpfApp.exe";
            var p = Process.Start(Path.Combine(folder, filename));
            Assert.True(p.WaitForExit(35000));
            Assert.Equal(0, p.ExitCode);
        }

        private void DeleteAllButExes(string folder)
        {
            foreach (var filename in Directory.EnumerateFiles(folder).ToList())
                if (filename.EndsWith(".exe"))
                {
                    // keep
                }
                else
                {
                    File.Delete(filename);
                }
        }
    }
}