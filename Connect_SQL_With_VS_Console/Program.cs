using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Add_Song_To_SQL_Console
{

    public class Program
    {
        private static SqlConnection connection;
        public static void Main()
        {

            CultureInfo.CurrentCulture = CultureInfo.InvariantCulture;

            connection = new SqlConnection(@"Data Source=(local)\SQLExpress;Initial Catalog=MusicDataBase;Integrated Security=SSPI;");
            connection.Open();

            Console.WriteLine("Add a new song!");

            Console.Write("Title: ");
            string insertTitle = Console.ReadLine();

            Console.Write("Length: ");
            int insertLength = int.Parse(Console.ReadLine());

            Console.Write("Does your song have a music video [yes/no]: ");
            string insertVideo = Console.ReadLine();
            if (insertVideo == "yes" || insertVideo == "y")
            {
                insertVideo = true.ToString();
            }
            else
            {
                insertVideo = false.ToString();
            }

            Console.Write("Lyrics (if you have, otherwise press enter): ");
            string insertLyrics = Console.ReadLine();
            if (insertLyrics == string.Empty)
            {
                insertLyrics = "NULL";
            }


            string insertSQL = "INSERT INTO Song (Title, Length, HasMusicVideo, Lyrics) VALUES (@Title, @Length, @HasMusicVideo, @Lyrics)";
            SqlCommand insertCommand = new SqlCommand(insertSQL, connection);
            insertCommand.Parameters.Add(new SqlParameter
            {
                ParameterName = "@Title",
                Value = insertTitle
            });
            insertCommand.Parameters.Add(new SqlParameter
            {
                ParameterName = "@Length",
                Value = insertLength
            });
            insertCommand.Parameters.Add(new SqlParameter
            {
                ParameterName = "@HasMusicVideo",
                Value = insertVideo
            });
            insertCommand.Parameters.Add(new SqlParameter
            {
                ParameterName = "@Lyrics",
                Value = insertLyrics
            });

            insertCommand.ExecuteNonQuery();

            Console.WriteLine("\nSong archive:\n");
            ReadSQLlistFromSong();
        }

        public static void ReadSQLlistFromSong()
        {
            SqlCommand command = new SqlCommand("SELECT * FROM Song", connection);
            SqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                string title = Convert.ToString(reader["Title"]);
                int totalMinutes = Convert.ToInt32(reader["Length"]);
                double totalSeconds = Convert.ToDouble(reader["Length"]);
                int minutes = totalMinutes / 60;
                string seconds = (totalSeconds % 60).ToString();

                if (seconds.Length < 2)
                {
                    seconds = "0" + seconds;
                }

                string totalTime = minutes.ToString() + ":" + seconds.ToString();

                Console.WriteLine(title + ": " + totalTime);
            }
        }
    }
}
