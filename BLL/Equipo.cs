using APITicket.Dato;
using APITicket.Modelo;
using System.Data;
using System.Data.SqlClient;
using System.Text.Json;

namespace APITicket.BLL
{
    public class Equipo
    {
        public IConfiguration configuration;
        private Conexion conexion;
        public Equipo(IConfiguration configuration)
        {
            this.configuration = configuration;

            conexion = new(this.configuration);
        }

        public object Listar()
        {
            try
            {
                var conn = conexion.GetConnection();
                List<EquipoMOD> equipoMOD = new();
                conn.Open();


                string cadena = "select e.ID as EquipoID,e.Nombre as NombreEquipo,e.Descripcion as DescripcionEquipo,IDArea,a.Nombre as NombreArea from dbo.Equipos e inner join dbo.AreaTrabajo a on e.IDArea = a.ID";
                SqlCommand command = new SqlCommand(cadena, conn);
                command.CommandType = CommandType.Text;
                command.CommandText = cadena;

                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {


                    equipoMOD.Add(new EquipoMOD
                    {
                        ID = (long)reader["EquipoID"],
                        Nombre = reader["NombreEquipo"].ToString(),
                        Descripcion = reader["DescripcionEquipo"].ToString(),
                        IDArea = (long)reader["IDArea"],
                        NombreArea = reader["NombreArea"].ToString()


                    });

                }

                return equipoMOD;


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
                var equipoMOD = new EquipoMOD();
                conn.Open();


                string cadena = "select * from dbo.Equipos where ID = @ID ";
                SqlCommand command = new SqlCommand(cadena, conn);
                command.CommandType = CommandType.Text;
                command.CommandText = cadena;
                command.Parameters.AddWithValue("@ID", ID);

                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {


                    equipoMOD.ID = (long)reader["ID"];
                    equipoMOD.Nombre = reader["Nombre"].ToString();
                    equipoMOD.Descripcion = reader["Descripcion"].ToString();
                    equipoMOD.IDArea = (long)reader["IDArea"];


                }

                return equipoMOD;


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
                var equipoMOD = request.Deserialize<EquipoMOD>();
                var conn = conexion.GetConnection();
                conn.Open();


                string cadena = "insert into dbo.Equipos (Nombre,Descripcion,IDArea ) values (@Nombre,@Descripcion,@IDArea )";
                SqlCommand command = new SqlCommand(cadena, conn);
                command.CommandType = CommandType.Text;
                command.CommandText = cadena;
                command.Parameters.AddWithValue("@Nombre", equipoMOD.Nombre);
                command.Parameters.AddWithValue("@Descripcion", equipoMOD.Descripcion);
                command.Parameters.AddWithValue("@IDArea", equipoMOD.IDArea);
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
                var equipoMOD = request.Deserialize<EquipoMOD>();
                var conn = conexion.GetConnection();
                conn.Open();


                string cadena = "update dbo.Equipos set Nombre = @Nombre,Descripcion = @Descripcion,IDArea=@IDArea where ID = @ID";
                SqlCommand command = new SqlCommand(cadena, conn);
                command.CommandType = CommandType.Text;
                command.CommandText = cadena;
                command.Parameters.AddWithValue("@Nombre", equipoMOD.Nombre);
                command.Parameters.AddWithValue("@Descripcion", equipoMOD.Descripcion);
                command.Parameters.AddWithValue("@IDArea", equipoMOD.IDArea);
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
