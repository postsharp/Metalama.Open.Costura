namespace Caravela.Open.DependencyEmbedder.TestApp
{
    internal class Program
    {
        private static void Main()
        {
            Delay();
        }

        private static void Delay()
        {
            ThenUse.Stuff();
        }
    }
}