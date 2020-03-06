using System;

namespace HousingCommunalServicesClassLibrary
{
    public sealed class ConsoleReport : IReport
    {
        public void Display(string message)
        {
            Console.WriteLine(message);
        }
    }
}
