using System;
using Newtonsoft.Json;
using Soothsilver.Random;
using Xunit;

namespace Metalama.Open.DependencyEmbedder.TestApp
{
    internal class ThenUse
    {
        public static void Stuff()
        {
            var srls = JsonConvert.SerializeObject(new string[] { "he", "ha" });
            var r = srls + R.Next(0, 1).ToString();
            Assert.Equal(@"[""he"",""ha""]0", r);
            Console.WriteLine("This is still working: " + r);
        }
    }
}