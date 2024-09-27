using System;
using System.Collections.Generic;
using PowerMonitoringSystem.BusinessObjects;
using PowerMonitoringSystem.BusinessLogicLayer;
using PowerMonitoringSystem.PresentationLayer;

namespace PowerMonitoringSystem
{
    class MainClass
    {
        static void Main(string[] args)
        {
            ElectricitySourceService service = new ElectricitySourceService();

            List<ElectricitySource> allSources = service.GetAllElectricitySources();
            Console.WriteLine("All Power Stations:");
            Program.DisplayPowerStations(allSources);

            List<ElectricitySource> lowOutputStations = service.FindStationsGeneratingLessThan50Percent();
            Console.WriteLine("\nPower Stations Generating Less Than 50%:");
            Program.DisplayPowerStations(lowOutputStations);
        }
    }
}
