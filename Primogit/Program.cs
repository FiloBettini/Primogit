using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System.Reflection.PortableExecutable;


namespace Primogit
{
    internal class Program
    {
        static void Main(string[] args)
        {

            var configuration = new ConfigurationBuilder()
                  .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false); 

            var config = configuration.Build();
            var connectionString = config["ConnectionString"];

            SqlConnection conn = new SqlConnection(connectionString);

            try
            {
                conn.Open();

                string command = "SELECT Id, GiornoSettimana, Ora_Da, Ora_A, TotaleOre FROM Prima_Tabella";

                SqlCommand cmd = new SqlCommand(command, conn);

                SqlDataReader reader = cmd.ExecuteReader();

                List<OrarioUfficio> orari_uffico = new List<OrarioUfficio>();

                while (reader.Read())
                {
                    OrarioUfficio orario = new OrarioUfficio();

                    orario.Id = Convert.ToInt32(reader[0]);
                    orario.GiornoSettimana = (DayOfWeek)reader[1];
                    orario.Ora_Da = (TimeSpan)reader[2];
                    orario.Ora_A = (TimeSpan)reader[3];
                    orario.TotaleOre = Convert.ToDouble(reader[4]);

                    orari_uffico.Add(orario);
                }

                DateTime totale;

                var totaleOreLavorative = 0.0;
                bool ok_d = true;
                bool ok_m = true;
                var date = new DateTime();
                TimeSpan Ora = new TimeSpan();
                TimeSpan Orafin = new TimeSpan();
                double orario_finale = new double();

                do
                {
                    Console.Write("inserisci la data e l'ora");
                    var dataInput = Console.ReadLine();

                    if (DateTime.TryParse(dataInput, out date))
                    {
                        ok_d = true;
                        DayOfWeek DayOfWeek = date.DayOfWeek;

                        Ora = date.TimeOfDay;

                    }
                    else
                    {
                        Console.WriteLine("non hai inserito dei dati corretti");
                        ok_d = false;
                    }
                }
                while (ok_d == false);

                do
                {
                    Console.Write("Inserisci il numero di minuti");
                    var minuti = Console.ReadLine();
                    var totalMinutes = 0.0;
                    if (double.TryParse(minuti, out totalMinutes))
                    {
                        Console.WriteLine("{0:00}:{1:00}", totalMinutes / 60, totalMinutes % 60);
                        totale = date.AddMinutes(totalMinutes);


                        totaleOreLavorative = totalMinutes / 60.0;

                        ok_m = true;
                    }
                    else
                    {
                        Console.WriteLine("non hai inserito dei dati corretti");
                        ok_m = false;
                    }
                }
                while (ok_m == false);
                double ora = Ora.Hours;
                do
                {

                    DayOfWeek dofw = date.DayOfWeek;

                    OrarioUfficio orario = orari_uffico.Where(x => x.GiornoSettimana== dofw).First();

                    double totale_ore = 0;
                    if (Ora > orario.Ora_Da && Ora <= orario.Ora_A)
                    {
                        Orafin = Ora - orario.Ora_Da;
                        double tot_diff_hour = Orafin.TotalHours;
                        totale_ore = orario.TotaleOre - tot_diff_hour;

                    }
                    else
                    {
                        totale_ore = orario.TotaleOre;
                    }
                    if (totaleOreLavorative < totale_ore)
                    {
                        totale_ore = totaleOreLavorative;
                    }
                    totaleOreLavorative = totaleOreLavorative - totale_ore;
                    orario_finale = orario.Ora_Da.Hours;
                    orario_finale = orario_finale + totale_ore;
                    if (totaleOreLavorative > 0)
                    {
                        date = date.AddDays(1);
                    }

                } while (totaleOreLavorative > 0);

                Console.WriteLine($"La data di fine sarà {date.ToString("dd/MM/yyyy")} alle ore {orario_finale}");
                return;
            }

            catch (Exception ex)
            {
                Console.WriteLine(ex.Message.ToString());
            }
            finally
            {
                conn.Close();
            }
        }
    }
}