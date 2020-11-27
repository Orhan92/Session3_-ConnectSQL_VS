using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Session3__ConnectSQL_VS
{
    public class Program
    {
        public static void Main()
        {
            CultureInfo.CurrentCulture = CultureInfo.InvariantCulture;

            SqlConnection connection = new SqlConnection(@"Data Source=(local)\SQLExpress;Initial Catalog=MusicDataBase;Integrated Security=SSPI;");
            connection.Open();

            SqlCommand command = new SqlCommand("SELECT * FROM Song", connection);
            SqlDataReader reader = command.ExecuteReader();

            Console.WriteLine("Song archive");
            // Denna loop fortsätter tills alla rader har lästs (genom att Read() returnerar en boolean).
            while (reader.Read())
            {
                string title = Convert.ToString(reader["Title"]);
                int totalMinutes = Convert.ToInt32(reader["Length"]);
                double totalSeconds = Convert.ToDouble(reader["Length"]);
                int minutes = totalMinutes / 60;
                string seconds = (totalSeconds % 60).ToString();

                if(seconds.Length < 2)
                {
                    seconds = "0" + seconds;
                }

                string totalTime = minutes.ToString() + ":" + seconds.ToString();

                Console.WriteLine(title + ": " + totalTime);
            }
        }
    }

    [TestClass]
    public class ProgramTests
    {
        [TestMethod]
        public void ExampleTest()
        {
            using FakeConsole console = new FakeConsole("First input", "Second input");
            Program.Main();
            Assert.AreEqual("Hello!", console.Output);
        }
    }
}
