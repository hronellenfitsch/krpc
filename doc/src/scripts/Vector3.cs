using KRPC.Client;
using KRPC.Client.Services.SpaceCenter;
using System;
using System.Net;

class VectorExample
{
    public static void Main ()
    {
        var connection = new KRPC.Client.Connection (address: IPAddress.Parse ("10.0.2.2"));
        var vessel = connection.SpaceCenter ().ActiveVessel;
        KRPC.Client.Tuple<double,double,double> v = vessel.Flight ().Prograde;
        Console.WriteLine (v.Item1 + "," + v.Item2 + "," + v.Item3);
    }
}
