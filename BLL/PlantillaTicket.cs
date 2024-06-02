using APITicket.Dato;
using APITicket.Modelo;
using System.Data;
using System.Data.SqlClient;
using System.Text.Json;

namespace APITicket.BLL
{
    public class PlantillaTicket
    {
        public IConfiguration configuration;
        private Conexion conexion;
        public PlantillaTicket(IConfiguration configuration)
        {
            this.configuration = configuration;

            conexion = new(this.configuration);
        }

        public object Listar()
        {
            try
            {
                var conn = conexion.GetConnection();
                List<PlantillaTicketMOD> plantillaTicketMOD = new();
                conn.Open();


                string cadena = "select pt.ID as IDPlantillaTicket,Titulo,IDArea,a.Nombre as NombreArea from dbo.PlantillaTicket pt inner join dbo.AreaTrabajo a on a.ID = pt.IDArea ";
                SqlCommand command = new SqlCommand(cadena, conn);
                command.CommandType = CommandType.Text;
                command.CommandText = cadena;

                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {


                    plantillaTicketMOD.Add(new PlantillaTicketMOD
                    {
                        ID = (long)reader["IDPlantillaTicket"],
                        Titulo = reader["Titulo"].ToString(),
                        IDArea = (long)reader["IDArea"],
                        NombreArea = reader["NombreArea"].ToString(),


                    });

                }

                return plantillaTicketMOD;


            }
            catch (Exception ex)
            {

                throw new Exception("", ex);
            }
        }

        public object Buscar(int ID)
        {
            try
            {

                var conn = conexion.GetConnection();
                var plantillaTicketMOD = new PlantillaTicketMOD();
                conn.Open();


                string cadena = "select * from dbo.PlantillaTicket where ID = @ID ";
                SqlCommand command = new SqlCommand(cadena, conn);
                command.CommandType = CommandType.Text;
                command.CommandText = cadena;
                command.Parameters.AddWithValue("@ID", ID);

                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {


                    plantillaTicketMOD.ID = (long)reader["ID"];
                    plantillaTicketMOD.Titulo = reader["Titulo"].ToString();
                    plantillaTicketMOD.IDArea = (long)reader["IDArea"];


                }

                return plantillaTicketMOD;


            }
            catch (Exception ex)
            {

                throw new Exception("", ex);
            }
        }

        public object Agregar(JsonElement request)
        {
            try
            {
                var plantillaTicketMOD = request.Deserialize<PlantillaTicketMOD>();
                var conn = conexion.GetConnection();
                conn.Open();


                string cadena = "insert into dbo.PlantillaTicket (Titulo,IDArea ) values (@Titulo,@IDArea )";
                SqlCommand command = new SqlCommand(cadena, conn);
                command.CommandType = CommandType.Text;
                command.CommandText = cadena;
                command.Parameters.AddWithValue("@Titulo", plantillaTicketMOD.Titulo);
                command.Parameters.AddWithValue("@IDArea", plantillaTicketMOD.IDArea);
                command.ExecuteNonQuery();

                return new { mensaje = "Se ingreso" };


            }
            catch (Exception ex)
            {

                throw new Exception("", ex);
            }
        }

        public object Modificar(int id, JsonElement request)
        {
            try
            {
                var plantillaTicketMOD = request.Deserialize<PlantillaTicketMOD>();
                var conn = conexion.GetConnection();
                conn.Open();


                string cadena = "update dbo.PlantillaTicket set Titulo = @Titulo,IDArea = @IDArea where ID = @ID";
                SqlCommand command = new SqlCommand(cadena, conn);
                command.CommandType = CommandType.Text;
                command.CommandText = cadena;
                command.Parameters.AddWithValue("@Titulo", plantillaTicketMOD.Titulo);
                command.Parameters.AddWithValue("@IDArea", plantillaTicketMOD.IDArea);
                command.Parameters.AddWithValue("@ID", id);


                command.ExecuteNonQuery();

                return new { mensaje = "Se modifico" };


            }
            catch (Exception ex)
            {

                throw new Exception("", ex);
            }
        }
    }
}
