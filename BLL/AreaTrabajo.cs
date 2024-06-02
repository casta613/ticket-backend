using APITicket.Dato;
using APITicket.Modelo;
using System.Data;
using System.Data.SqlClient;
using System.Text.Json;

namespace APITicket.BLL
{
    public class AreaTrabajo
    {
        public IConfiguration configuration;
        private Conexion conexion;
        public AreaTrabajo(IConfiguration configuration)
        {
            this.configuration = configuration;

            conexion = new(this.configuration);
        }

        public object Listar()
        {
            try
            {
                var conn = conexion.GetConnection();
                List<AreaTrabajoMOD> areaTrabajoMOD = new();
                conn.Open();


                string cadena = "select * from dbo.AreaTrabajo ";
                SqlCommand command = new SqlCommand(cadena, conn);
                command.CommandType = CommandType.Text;
                command.CommandText = cadena;

                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {


                    areaTrabajoMOD.Add(new AreaTrabajoMOD
                    {
                        ID = (long)reader["ID"],
                        Nombre = reader["Nombre"].ToString(),
                        Descripcion = reader["Descripcion"].ToString()
                       

                    });

                }

                return areaTrabajoMOD;


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
                var areaTrabajoMOD = new AreaTrabajoMOD();
                conn.Open();


                string cadena = "select * from dbo.AreaTrabajo where ID = @ID ";
                SqlCommand command = new SqlCommand(cadena, conn);
                command.CommandType = CommandType.Text;
                command.CommandText = cadena;
                command.Parameters.AddWithValue("@ID", ID);

                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {


                    areaTrabajoMOD.ID = (long)reader["ID"];
                    areaTrabajoMOD.Nombre = reader["Nombre"].ToString();
                    areaTrabajoMOD.Descripcion = reader["Descripcion"].ToString();


                }

                return areaTrabajoMOD;


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
                var areaTrabajoMOD = request.Deserialize<AreaTrabajoMOD>();
                var conn = conexion.GetConnection();
                conn.Open();


                string cadena = "insert into dbo.AreaTrabajo (Nombre,Descripcion,FechaCreacion ) values (@Nombre,@Descripcion,CURRENT_TIMESTAMP )";
                SqlCommand command = new SqlCommand(cadena, conn);
                command.CommandType = CommandType.Text;
                command.CommandText = cadena;
                command.Parameters.AddWithValue("@Nombre", areaTrabajoMOD.Nombre);
                command.Parameters.AddWithValue("@Descripcion", areaTrabajoMOD.Descripcion);
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
                var areaTrabajoMOD = request.Deserialize<AreaTrabajoMOD>();
                var conn = conexion.GetConnection();
                conn.Open();


                string cadena = "update dbo.AreaTrabajo set Nombre = @Nombre,Descripcion = @Descripcion where ID = @ID";
                SqlCommand command = new SqlCommand(cadena, conn);
                command.CommandType = CommandType.Text;
                command.CommandText = cadena;
                command.Parameters.AddWithValue("@Nombre", areaTrabajoMOD.Nombre);
                command.Parameters.AddWithValue("@Descripcion", areaTrabajoMOD.Descripcion);            
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
