using APITicket.Dato;
using APITicket.Modelo;
using System.Data;
using System.Data.SqlClient;
using System.Text.Json;

namespace APITicket.BLL
{
    public class Atributos
    {
        public IConfiguration configuration;
        private Conexion conexion;
        public Atributos(IConfiguration configuration)
        {
            this.configuration = configuration;

            conexion = new(this.configuration);
        }

        public object ListaEstados()
        {
            try
            {
                var conn = conexion.GetConnection();
                List<AtributosMOD> atributosMOD = new();
                conn.Open();


                string cadena = "select * from dbo.Estado ";
                SqlCommand command = new SqlCommand(cadena, conn);
                command.CommandType = CommandType.Text;
                command.CommandText = cadena;

                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {


                    atributosMOD.Add(new AtributosMOD
                    {
                        ID = (long)reader["ID"],                   
                        Nombre = reader["Nombre"].ToString()

                    });

                }

                return atributosMOD;


            }
            catch (Exception ex)
            {

                throw new Exception("", ex);
            }
        }

        public object ListaPrioridad()
        {
            try
            {
                var conn = conexion.GetConnection();
                List<AtributosMOD> atributosMOD = new();
                conn.Open();


                string cadena = "select * from dbo.Prioridad ";
                SqlCommand command = new SqlCommand(cadena, conn);
                command.CommandType = CommandType.Text;
                command.CommandText = cadena;

                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {


                    atributosMOD.Add(new AtributosMOD
                    {
                        ID = (long)reader["ID"],
                        Nombre = reader["Nombre"].ToString()

                    });

                }

                return atributosMOD;


            }
            catch (Exception ex)
            {

                throw new Exception("", ex);
            }
        }


    }
}
