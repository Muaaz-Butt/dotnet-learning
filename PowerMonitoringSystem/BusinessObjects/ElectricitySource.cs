namespace PowerMonitoringSystem.BusinessObjects
{
    public class ElectricitySource{
        public string PowerStationName { get; set; }
        public int TotalCapacity { get; set; }
        public int Output { get; set; }
        public string Type { get; set; }
        public ElectricitySource(string powerStationName, int totalCapacity, int output, string type) {
            if (totalCapacity < 0 || output < 0)
                throw new ArgumentException("Capacity and output must be non-negative.");
            PowerStationName = powerStationName;
            TotalCapacity = totalCapacity;
            Output = output;
            Type = type;
        }

        public ElectricitySource() { }
    }
}