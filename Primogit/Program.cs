using Microsoft.Data.SqlClient;
using System.Reflection.PortableExecutable;

namespace Primogit
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string connectionString = "data Source=DESKTOP-J5O3RI2;initial catalog=PrimoDB;uid=sa;pwd=duma2008sa;TrustServerCertificate=true";

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

                foreach(var item in orari_uffico) {
                    Console.Write(item.Id + " - ");
                    Console.Write(item.GiornoSettimana + " - ");
                    Console.Write(item.Ora_Da + " - ");
                    Console.Write(item.Ora_A + " - ");
                    Console.WriteLine(item.TotaleOre);
                }
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