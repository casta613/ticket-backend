using APITicket.Dato;
using APITicket.Modelo;
using Newtonsoft.Json;
using System.Data;
using System.Data.SqlClient;
using System.Text.Json;

namespace APITicket.BLL
{
    public class Ticket
    {
        public IConfiguration configuration;
        private Conexion conexion;
        private Usuario usuario;
        public Ticket(IConfiguration configuration)
        {
            this.configuration = configuration;

            conexion = new(this.configuration);
            usuario = new(this.configuration);
        }

        public object Listar()
        {
            try
            {
                var conn = conexion.GetConnection();
                List<TicketMOD> ticketMOD = new();
                conn.Open();


                string cadena = "SELECT t.ID as IDTicket,IDEncargado,IDCreador,IDPlantilla,t.Descripcion,IDPrioridad,IDEstado, "+
                                "Titulo,ue.Nombre as Encargado,c.Nombre as Creador,pr.Nombre as Prioridad,es.Nombre as Estado "+
                                "FROM dbo.Ticket t inner join dbo.PlantillaTicket p on p.ID = t.IDPlantilla " +
                                "inner join dbo.Encargado e on e.ID = t.IDEncargado inner join dbo.Usuario ue on ue.ID = e.IDUsuario "+
                                "inner join dbo.Usuario c on c.ID = t.IDCreador inner join dbo.Prioridad pr on pr.ID = t.IDPrioridad "+
                                "inner join dbo.Estado es on es.ID = t.IDEstado ";
                SqlCommand command = new SqlCommand(cadena, conn);
                command.CommandType = CommandType.Text;
                command.CommandText = cadena;

                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {


                    ticketMOD.Add(new TicketMOD
                    {
                        ID = (long)reader["IDTicket"],
                        IDCreador = (long)reader["IDCreador"],
                        IDEstado = (long)reader["IDEstado"],
                        IDEncargado = (long)reader["IDEncargado"],
                        IDPlantilla = (long)reader["IDPlantilla"],
                        IDPrioridad = (long)reader["IDPrioridad"],
                        IDTicket = (long)reader["IDTicket"],
                        Titulo = reader["Titulo"].ToString().Trim(),
                        Descripcion = reader["Descripcion"].ToString().Trim(),
                        Encargado = reader["Encargado"].ToString().Trim(),
                        Estado = reader["Estado"].ToString().Trim(),
                        Creador = reader["Creador"].ToString().Trim(),
                        Prioridad = reader["Prioridad"].ToString().Trim(),



                    });

                }

                return ticketMOD;


            }
            catch (Exception ex)
            {

                throw new Exception("", ex);
            }
        }

        public object Listar(string userName)
        {
            try
            {
                List<TicketMOD> ticketMOD = new();
                long id = usuarioID(userName);
                UsuarioRolMOD rol = usuario.BuscarRolUsuario(id);
                if (rol.IDRol == 2)
                {

                    var conn = conexion.GetConnection();

                    conn.Open();


                    string cadena = "SELECT t.ID as IDTicket,IDEncargado,IDCreador,IDPlantilla,t.Descripcion,IDPrioridad,IDEstado, " +
                                    "Titulo,ue.Nombre as Encargado,c.Nombre as Creador,pr.Nombre as Prioridad,es.Nombre as Estado " +
                                    "FROM dbo.Ticket t inner join dbo.PlantillaTicket p on p.ID = t.IDPlantilla " +
                                    "inner join dbo.Encargado e on e.ID = t.IDEncargado inner join dbo.Usuario ue on ue.ID = e.IDUsuario " +
                                    "inner join dbo.Usuario c on c.ID = t.IDCreador inner join dbo.Prioridad pr on pr.ID = t.IDPrioridad " +
                                    "inner join dbo.Estado es on es.ID = t.IDEstado where e.IDUsuario = @IDUsuario";
                    SqlCommand command = new SqlCommand(cadena, conn);
                    command.CommandType = CommandType.Text;
                    command.CommandText = cadena;
                    command.Parameters.AddWithValue("@IDUsuario", id);

                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {


                        ticketMOD.Add(new TicketMOD
                        {
                            ID = (long)reader["IDTicket"],
                            IDCreador = (long)reader["IDCreador"],
                            IDEstado = (long)reader["IDEstado"],
                            IDEncargado = (long)reader["IDEncargado"],
                            IDPlantilla = (long)reader["IDPlantilla"],
                            IDPrioridad = (long)reader["IDPrioridad"],
                            IDTicket = (long)reader["IDTicket"],
                            Titulo = reader["Titulo"].ToString().Trim(),
                            Descripcion = reader["Descripcion"].ToString().Trim(),
                            Encargado = reader["Encargado"].ToString().Trim(),
                            Estado = reader["Estado"].ToString().Trim(),
                            Creador = reader["Creador"].ToString().Trim(),
                            Prioridad = reader["Prioridad"].ToString().Trim(),



                        });

                    }

                    return ticketMOD;


                }
                else
                {
                    return Listar();
                }


                }
            catch (Exception ex)
            {

                throw new Exception("", ex);
            }
        }

        public object Agregar(JsonElement request,string userName)
        {
            try
            {
                var ticketMOD = request.Deserialize<TicketMOD>();
                var conn = conexion.GetConnection();
                long usuario = usuarioID(userName);
                conn.Open();

                string cadena = "SELECT e.ID as IDEncargado FROM dbo.Encargado e inner join dbo.AreaTrabajo a on a.ID = e.IDEquipo " +
                    "inner join dbo.PlantillaTicket p on p.IDArea = a.ID where p.ID = @IDPlantilla";

                SqlCommand commandEncargado = new SqlCommand(cadena, conn);
                commandEncargado.CommandType = CommandType.Text;
                commandEncargado.CommandText = cadena;
                commandEncargado.Parameters.AddWithValue("@IDPlantilla", ticketMOD.IDPlantilla);

                SqlDataReader reader = commandEncargado.ExecuteReader();

                if (reader.Read())
                {
                    var conn2 = conexion.GetConnection();
                    long IDEncargado =  (long)reader["IDEncargado"];

                    conn2.Open();

                    cadena = "INSERT INTO dbo.Ticket ( IDEncargado, IDCreador, IDPlantilla, Descripcion, FechaCreacion, IDPrioridad, IDEstado)  " +
                       "  VALUES   ( @IDEncargado , @IDCreador  , @IDPlantilla , @Descripcion, current_timestamp , @IDPrioridad  , 1 )";
                    SqlCommand command = new SqlCommand(cadena, conn2);
                    command.CommandType = CommandType.Text;
                    command.CommandText = cadena;
                    command.Parameters.AddWithValue("@IDEncargado", IDEncargado);
                    command.Parameters.AddWithValue("@IDCreador", usuario);
                    command.Parameters.AddWithValue("@IDPlantilla", ticketMOD.IDPlantilla);
                    command.Parameters.AddWithValue("@Descripcion", ticketMOD.Descripcion);
                    command.Parameters.AddWithValue("@IDPrioridad", ticketMOD.IDPrioridad);
                    command.ExecuteNonQuery();

                    conn2.Close();

                }

                conn.Close();
                

                return new { mensaje = "Se ingreso" };


            }
            catch (Exception ex)
            {

                throw new Exception("", ex);
            }
        }

        private long usuarioID(string correo)
        {
            try
            {
                long id = 0;
                var conn = conexion.GetConnection();
                conn.Open();


                string cadena = "select * from dbo.Usuario u  where Correo = @Correo ";
                SqlCommand command = new SqlCommand(cadena, conn);
                command.CommandType = CommandType.Text;
                command.CommandText = cadena;
                command.Parameters.AddWithValue("@Correo", correo);

                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {

                    id = (long)reader["ID"];


                }

                return id;


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
                var ticketMOD = request.Deserialize<TicketMOD>();
                var conn = conexion.GetConnection();
                conn.Open();


                string cadena = "update dbo.Ticket set IDPrioridad = @IDPrioridad,IDEstado = @IDEstado where ID = @ID";
                SqlCommand command = new SqlCommand(cadena, conn);
                command.CommandType = CommandType.Text;
                command.CommandText = cadena;
                command.Parameters.AddWithValue("@IDEstado", ticketMOD.IDEstado);
                command.Parameters.AddWithValue("@IDPrioridad", ticketMOD.IDPrioridad);
                command.Parameters.AddWithValue("@ID", id);


                command.ExecuteNonQuery();

                return new { mensaje = "Se modifico" };


            }
            catch (Exception ex)
            {

                throw new Exception("", ex);
            }
        }

        public object AgregarRegistro(JsonElement request)
        {
            try
            {
                var ticketLogMOD = request.Deserialize<LogTicketMOD>();
                var conn = conexion.GetConnection();
                conn.Open();


                string cadena = "INSERT INTO dbo.LogTicket ( Fecha, IDTicket, Accion, Descripcion)  " +
                    "  VALUES   ( current_timestamp , @IDTicket  , @Accion , @Descripcion )";
                SqlCommand command = new SqlCommand(cadena, conn);
                command.CommandType = CommandType.Text;
                command.CommandText = cadena;
                command.Parameters.AddWithValue("@IDTicket", ticketLogMOD.IDTicket);
                command.Parameters.AddWithValue("@Accion", ticketLogMOD.Accion);
                command.Parameters.AddWithValue("@Descripcion", ticketLogMOD.Descripcion);
                command.ExecuteNonQuery();
              
                return new { mensaje = "Se ingreso" };


            }
            catch (Exception ex)
            {

                throw new Exception("", ex);
            }
        }

        public object BuscarRegistro(long ticket)
        {
            try
            {
                List<LogTicketMOD> ticketMOD = new();

                var conn = conexion.GetConnection();
                conn.Open();


                string cadena = "select * from dbo.LogTicket u  where IDTicket = @IDTicket ";
                SqlCommand command = new SqlCommand(cadena, conn);
                command.CommandType = CommandType.Text;
                command.CommandText = cadena;
                command.Parameters.AddWithValue("@IDTicket", ticket);

                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {


                    ticketMOD.Add(new LogTicketMOD
                    {
                        ID = (long)reader["ID"],
                        IDTicket = (long)reader["IDTicket"],
                        Accion = reader["Accion"].ToString(),
                        Descripcion = reader["Descripcion"].ToString(),
                        



                    });

                }

                return ticketMOD;


            }
            catch (Exception ex)
            {

                throw new Exception("", ex);
            }
        }
    }
}
