using System;
using System.Collections.Generic;
using PowerMonitoringSystem.BusinessObjects;

namespace PowerMonitoringSystem.PresentationLayer
{
    public class Program
    {
        public static void DisplayPowerStations(List<ElectricitySource> powerStations)
        {
            foreach (var station in powerStations)
            {
                Console.WriteLine($"{station.PowerStationName} - Total Capacity: {station.TotalCapacity}MW, Output: {station.Output}MW, Type: {station.Type}");
            }
        }
    }
}
