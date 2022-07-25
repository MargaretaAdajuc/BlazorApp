using System.Collections.Generic;

namespace PaySys.Server
{
    public static class CurrencyManager
    {
        public static List<string> Currencies { get; }

        static CurrencyManager()
        {
            Currencies = new List<string>
            {
                "EUR",
                "MDL",
                "USD",
                "EC",
                "BTC"
            };

        }
    }
}
