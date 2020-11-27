using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Add_Album_To_SQL_Console
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

            Console.WriteLine("Add a new Album!");

            Console.Write("Album title: ");
            string insertName;
            while (true)
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

            Console.Write("Release date (Ex: 1992-12-15): ");
            string insertReleaseDate = Console.ReadLine();

            string insertSQL = "INSERT INTO Album (Title, ReleaseDate) VALUES (@Title, @ReleaseDate)";
            SqlCommand insertCommand = new SqlCommand(insertSQL, connection);

            //Insert Title
            insertCommand.Parameters.Add(new SqlParameter
            {
                ParameterName = "@Title",
                Value = insertName
            });

            //Insert ReleaseDate
            insertCommand.Parameters.Add(new SqlParameter
            {
                ParameterName = "@ReleaseDate",
                Value = insertReleaseDate
            });

            insertCommand.ExecuteNonQuery();

            Console.WriteLine("\nAlbum archive:\n");
            ReadSQLlistFromAlbum();
        }
        public static void ReadSQLlistFromAlbum()
        {
            SqlCommand command = new SqlCommand("SELECT * FROM Album", connection);
            SqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                string bandName = Convert.ToString(reader["Title"]);
                DateTime releaseDate = Convert.ToDateTime(reader["ReleaseDate"]);

                Console.WriteLine(bandName + " | " + releaseDate.ToShortDateString());
            }
        }
    }
}
