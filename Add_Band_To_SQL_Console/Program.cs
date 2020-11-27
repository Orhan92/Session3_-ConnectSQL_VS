using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Add_Band_To_SQL_Console
{
    public class Program
    {
        private static SqlConnection connection;
        public static void Main()
        {
            CultureInfo.CurrentCulture = CultureInfo.InvariantCulture;

            //Establishing connection with SQL
            connection = new SqlConnection(@"Data Source=(local)\SQLExpress;Initial Catalog=MusicDataBase;Integrated Security=SSPI;");
            connection.Open();

            Console.WriteLine("Add a new Band!");

            //INSERT insertName (Band name)
            Console.Write("Band name: ");
            string insertName;
            while(true)
            {
                insertName = Console.ReadLine();
                if (insertName == string.Empty)
                {
                    Console.Write("Try again: ");
                }
                else
                {
                    break;
                }
            }

            //INSERT resultYear
            Console.Write("Year Formed: ");
            string insertYear;
            while (true)
            {
                int year = int.Parse(Console.ReadLine());
                if (year > 2020 || year < 1500)
                {
                    Console.Write("Try again: ");
                }
                else
                {
                    insertYear = year.ToString();
                    insertYear += "-01-01";
                    break;
                }
            }

            //INSERT resultCountryCode
            Console.Write("Country code (2 letters, Ex: US): ");
            string insertCountryCode;
            while (true)
            {
                insertCountryCode = Console.ReadLine().ToUpper();
                if (insertCountryCode.Length != 2)
                {
                    Console.Write("Try again: ");
                }
                else
                {
                    break;
                }
            }

            string insertSQL = "INSERT INTO Band (Name, YearFormed, Country) VALUES (@Name, @YearFormed, @Country)";
            SqlCommand insertCommand = new SqlCommand(insertSQL, connection);

            //Insert Song Title
            insertCommand.Parameters.Add(new SqlParameter
            {
                ParameterName = "@Name",
                Value = insertName
            });

            //Insert YearFormed
            insertCommand.Parameters.Add(new SqlParameter
            {
                ParameterName = "@YearFormed",
                Value = insertYear
            });

            //Insert CountryCode
            insertCommand.Parameters.Add(new SqlParameter
            {
                ParameterName = "@Country",
                Value = insertCountryCode
            });

            insertCommand.ExecuteNonQuery();

            Console.WriteLine("\nBand archive:\n");
            ReadSQLlistFromBand();
        }

        public static void ReadSQLlistFromBand()
        {
            SqlCommand command = new SqlCommand("SELECT * FROM Band", connection);
            SqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                string bandName = Convert.ToString(reader["Name"]);
                DateTime year = Convert.ToDateTime(reader["YearFormed"]);
                string countryCode = Convert.ToString(reader["Country"]);

                
                Console.WriteLine(bandName + " | " + countryCode + " | " + year.Year);
            }
        }
    }
}
