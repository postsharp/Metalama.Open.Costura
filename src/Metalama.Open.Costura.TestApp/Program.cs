// This is an open-source Metalama example. See https://github.com/postsharp/Metalama.Samples for more.

namespace Metalama.Open.Costura.TestApp;

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