using System;
using System.Collections.Generic;
using PowerMonitoringSystem.BusinessObjects;
using Microsoft.Data.Sqlite;

namespace PowerMonitoringSystem.DataAccessLayer
{
    public class ElectricitySourceRepository
    {
        private const string connectionString = "Data Source=PowerStations.db";

        public ElectricitySourceRepository()
        {
            InitializeDatabase();
        }

        private void take_input_from_user(ref string powerStationName, ref int totalCapacity, ref int output, ref string type) {
            Console.WriteLine("Enter Power Station Name:");
            powerStationName = Console.ReadLine();

            totalCapacity = 0;
            while (true) {
                Console.WriteLine("Enter Total Capacity (in MW):");
                string input = Console.ReadLine();
                  
                try {
                    totalCapacity = int.Parse(input);
                    if (totalCapacity <= 0) {
                        Console.WriteLine("Total capacity must be a positive integer. Please try again.");
                    }
                    else
                    {
                        break; 
                    }
                }
                catch (FormatException)
                {
                    Console.WriteLine("Invalid input. Please enter a valid number for the total capacity.");
                }
            }
        

          output = 0;
          while (true)
          {
              Console.WriteLine("Enter Output (in MW):");
              string input = Console.ReadLine();
              
              try
              {
                  output = int.Parse(input);
                  if (output <= 0)
                  {
                      Console.WriteLine("Output must be a positive integer. Please try again.");
                  }
                  else if (output > totalCapacity)
                  {
                      Console.WriteLine("Output cannot be greater than total capacity. Please enter a value less than or equal to " + totalCapacity);
                  }
                  else
                  {
                      break; 
                  }
              }
              catch (FormatException)
              {
                  Console.WriteLine("Invalid input. Please enter a valid number for the output.");
              }
          }

          Console.WriteLine("Enter Type (e.g., Oil-fired, Hydroelectric):");
          type = Console.ReadLine();
      }

        private void InitializeDatabase()
        {
            try
            {
                using (var connection = new SqliteConnection(connectionString))
                {
                    connection.Open();

                    string createTableQuery = @"
                    CREATE TABLE IF NOT EXISTS PowerStations (
                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                        PowerStationName TEXT NOT NULL,
                        TotalCapacity INTEGER NOT NULL,
                        Output INTEGER NOT NULL,
                        Type TEXT NOT NULL
                    );";
                    
                    using (var createTableCommand = new SqliteCommand(createTableQuery, connection))
                    {
                        createTableCommand.ExecuteNonQuery();
                    }

                    Console.WriteLine("Would you like to add data for power stations? (yes/no)");
                    string response = Console.ReadLine()?.ToLower();
                    
                    while (response == "yes")
                    {
                        try
                        {
                            string powerStationName = ""; 
                            int totalCapacity = 0;
                            int output = 0;
                            string type = "";

                            take_input_from_user(ref powerStationName, ref totalCapacity, ref output, ref type);

                            string insertDataQuery = @"
                            INSERT INTO PowerStations (PowerStationName, TotalCapacity, Output, Type) 
                            VALUES (@PowerStationName, @TotalCapacity, @Output, @Type);";

                            using (var insertCommand = new SqliteCommand(insertDataQuery, connection))
                            {
                                insertCommand.Parameters.AddWithValue("@PowerStationName", powerStationName);
                                insertCommand.Parameters.AddWithValue("@TotalCapacity", totalCapacity);
                                insertCommand.Parameters.AddWithValue("@Output", output);
                                insertCommand.Parameters.AddWithValue("@Type", type);

                                insertCommand.ExecuteNonQuery();
                                Console.WriteLine("Power station added successfully.");
                            }
                        }
                        catch (FormatException ex)
                        {
                            Console.WriteLine("Invalid input format. Error: " + ex.Message);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("An error occurred while inserting data: " + ex.Message);
                        }

                        Console.WriteLine("Would you like to add another power station? (yes/no)");
                        response = Console.ReadLine()?.ToLower();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred while initializing the database: " + ex.Message);
            }
        }

        public List<ElectricitySource> GetAllElectricitySources()
        {
            List<ElectricitySource> list = new List<ElectricitySource>();

            try
            {
                using (var connection = new SqliteConnection(connectionString))
                {
                    connection.Open();

                    string query = "SELECT PowerStationName, TotalCapacity, Output, Type FROM PowerStations";
                    using (var command = new SqliteCommand(query, connection))
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var powerStation = new ElectricitySource(
                                reader.GetString(0),
                                reader.GetInt32(1),
                                reader.GetInt32(2),
                                reader.GetString(3)
                            );
                            list.Add(powerStation);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred while fetching data: " + ex.Message);
            }

            return list;
        }

        public void UpdatePowerStationOutput(string powerStationName, int newOutput)
        {
            try
            {
                using (var connection = new SqliteConnection(connectionString))
                {
                    connection.Open();

                    string updateQuery = @"
                    UPDATE PowerStations 
                    SET Output = @NewOutput 
                    WHERE PowerStationName = @PowerStationName";
                    
                    using (var command = new SqliteCommand(updateQuery, connection))
                    {
                        command.Parameters.AddWithValue("@NewOutput", newOutput);
                        command.Parameters.AddWithValue("@PowerStationName", powerStationName);

                        int rowsAffected = command.ExecuteNonQuery();
                        Console.WriteLine($"{rowsAffected} row(s) updated.");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred while updating data: " + ex.Message);
            }
        }
    }
}
