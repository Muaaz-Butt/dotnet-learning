using System;
using System.Collections.Generic;
using PowerMonitoringSystem.BusinessObjects;
using PowerMonitoringSystem.DataAccessLayer;

namespace PowerMonitoringSystem.BusinessLogicLayer
{
    public class ElectricitySourceService
    {
        private readonly ElectricitySourceRepository _repository;

        public ElectricitySourceService()
        {
            _repository = new ElectricitySourceRepository();
        }

        public List<ElectricitySource> GetAllElectricitySources()
        {
            return _repository.GetAllElectricitySources();
        }

        public List<ElectricitySource> FindStationsGeneratingLessThan50Percent()
        {
            List<ElectricitySource> allSources = _repository.GetAllElectricitySources();
            List<ElectricitySource> lowOutputStations = new List<ElectricitySource>();

            foreach (var source in allSources)
            {
                if (source.Output < (source.TotalCapacity * 0.5))
                {
                    lowOutputStations.Add(source);
                }
            }

            return lowOutputStations;
        }
    }
}
