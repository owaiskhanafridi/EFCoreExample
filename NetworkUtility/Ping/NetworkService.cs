using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace NetworkUtility.Ping
{
    public class NetworkService
    {
        public string SendPing()
        => "Success: Ping Sent!";

        public int PingTimeout(int a, int b)
         => a + b;

        public DateTime LastPingDate()
            => DateTime.Now;

        public PingOptions GetPingOptions()
            => new PingOptions()
            {
                DontFragment = true,
                Ttl = 1
            };

        public IEnumerable<PingOptions> MostRecentPings()
        {
            IEnumerable<PingOptions> pingOptions = new[]
            {
                new PingOptions()
                {
                    DontFragment = true,
                    Ttl = 1
                },
                new PingOptions()
                {
                    DontFragment = true,
                    Ttl = 2
                },
                new PingOptions()
                {
                    DontFragment = true,
                    Ttl = 3
                },
            };

            return pingOptions;
        } 
    }
}
