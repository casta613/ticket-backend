using APITicket.Dato;
using APITicket.Modelo;
using System.Data;
using System.Data.SqlClient;
using System.Text.Json;

namespace APITicket.BLL
{
    public class Responsable
    {
        public IConfiguration configuration;
        private Conexion conexion;
        public Responsable(IConfiguration configuration)
        {
            this.configuration = configuration;

            conexion = new(this.configuration);
        }

        public object Listar()
        {
            try
            {
                var conn = conexion.GetConnection();
                List<ResponsableMOD> responsableMOD = new();
                conn.Open();


                string cadena = "select e.ID as IDEncargado,IDEquipo,IDUsuario,eq.Nombre as NombreEquipo,u.Nombre as NombreUsuario from dbo.Encargado e inner join dbo.Equipos eq on eq.ID = e.IDEquipo inner join dbo.Usuario u on u.ID = e.IDUsuario";
                SqlCommand command = new SqlCommand(cadena, conn);
                command.CommandType = CommandType.Text;
                command.CommandText = cadena;

                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {


                    responsableMOD.Add(new ResponsableMOD
                    {
                        ID = (long)reader["IDEncargado"],
                        IDEquipo = (long)reader["IDEquipo"],
                        IDUsuario = (long)reader["IDUsuario"],
                        NombreEquipo = reader["NombreEquipo"].ToString(),
                        NombreUsuario = reader["NombreUsuario"].ToString(),


                    });

                }

                return responsableMOD;


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
                var responsableMOD = new ResponsableMOD();
                conn.Open();


                string cadena = "select * from dbo.Encargado where ID = @ID ";
                SqlCommand command = new SqlCommand(cadena, conn);
                command.CommandType = CommandType.Text;
                command.CommandText = cadena;
                command.Parameters.AddWithValue("@ID", ID);

                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {


                    responsableMOD.ID = (long)reader["ID"];
                    responsableMOD.IDEquipo = (long)reader["IDEquipo"];
                    responsableMOD.IDUsuario = (long)reader["IDUsuario"];


                }

                return responsableMOD;


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
                var responsableMOD = request.Deserialize<ResponsableMOD>();
                var conn = conexion.GetConnection();
                conn.Open();


                string cadena = "insert into dbo.Encargado (IDEquipo,IDUsuario,FechaCreacion ) values (@IDEquipo,@IDUsuario,current_timestamp )";
                SqlCommand command = new SqlCommand(cadena, conn);
                command.CommandType = CommandType.Text;
                command.CommandText = cadena;
                command.Parameters.AddWithValue("@IDEquipo", responsableMOD.IDEquipo);
                command.Parameters.AddWithValue("@IDUsuario", responsableMOD.IDUsuario);
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
                var responsableMOD = request.Deserialize<ResponsableMOD>();
                var conn = conexion.GetConnection();
                conn.Open();


                string cadena = "update dbo.Encargado set IDEquipo = @IDEquipo,IDUsuario = @IDUsuario where ID = @ID";
                SqlCommand command = new SqlCommand(cadena, conn);
                command.CommandType = CommandType.Text;
                command.CommandText = cadena;
                command.Parameters.AddWithValue("@IDEquipo", responsableMOD.IDEquipo);
                command.Parameters.AddWithValue("@IDUsuario", responsableMOD.IDUsuario);
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
