using APITicket.Dato;
using APITicket.Modelo;
using System.Data;
using System.Data.SqlClient;
using System.Text.Json;

namespace APITicket.BLL
{
    public class Usuario
    {
        public IConfiguration configuration;
        private Conexion conexion;
        public Usuario(IConfiguration configuration)
        {
            this.configuration = configuration;

            conexion = new(this.configuration);
        }

        public object Listar()
        {
            try
            {
                var conn = conexion.GetConnection();
                List<UsuarioMOD> usuario = new();
                conn.Open();


                string cadena = "select u.ID ,u.Nombre,Apellido,Correo,Contraseña,Activo,IDRol from dbo.Usuario u inner join dbo.UsuarioRol ur on ur.IDUsuario = u.ID ";
                SqlCommand command = new SqlCommand(cadena, conn);
                command.CommandType = CommandType.Text;
                command.CommandText = cadena;

                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {


                    usuario.Add(new UsuarioMOD
                    {
                        ID = (long)reader["ID"],
                        Nombre = reader["Nombre"].ToString(),
                        Apellido = reader["Apellido"].ToString(),
                        Correo = reader["Correo"].ToString(),
                        Contraseña = reader["Contraseña"].ToString(),
                        Estatus = (bool)reader["Activo"] ,
                        IDRol = (long)reader["IDRol"]


                    }); ;

                }

                return usuario;


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
                var usuario = new UsuarioMOD();
                conn.Open();


                string cadena = "select * from dbo.Usuario where UsuarioID = @ID ";
                SqlCommand command = new SqlCommand(cadena, conn);
                command.CommandType = CommandType.Text;
                command.CommandText = cadena;
                command.Parameters.AddWithValue("@ID", ID);

                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {

                    usuario.ID = (long)reader["ID"];
                    usuario.Nombre = reader["Nombre"].ToString();
                    usuario.Apellido = reader["Apellido"].ToString();
                    usuario.Correo = reader["Correo"].ToString();
                    usuario.Contraseña = reader["Contraseña"].ToString();
                    usuario.Estatus = (int)reader["Activo"] == 1 ? true : false;



                }

                return usuario;


            }
            catch (Exception ex)
            {

                throw new Exception("", ex);
            }
        }

        public object Agregar(JsonElement Usuario)
        {
            try
            {
                var usuario = Usuario.Deserialize<UsuarioMOD>();
                var conn = conexion.GetConnection();
                conn.Open();


                string cadena = "insert into dbo.Usuario (Nombre,Apellido,Correo,Contraseña,Activo,FechaCreacion ) values (@Nombre,@Apellido,@Correo,@Contraseña,1,current_timestamp );SELECT SCOPE_IDENTITY();";
                SqlCommand command = new SqlCommand(cadena, conn);
                command.CommandType = CommandType.Text;
                command.CommandText = cadena;
                command.Parameters.AddWithValue("@Nombre", usuario.Nombre);
                command.Parameters.AddWithValue("@Contraseña", BCrypt.Net.BCrypt.HashPassword(usuario.Contraseña));
                command.Parameters.AddWithValue("@Apellido", usuario.Apellido);
                command.Parameters.AddWithValue("@Correo", usuario.Correo);

                var respuesta = command.ExecuteScalar();
                long IDUsuario = long.Parse(respuesta.ToString());
                conn.Close();

                conn.Open();


                cadena = "insert into dbo.UsuarioRol (IDUsuario,IDRol ) values (@IDUsuario,@IDRol )";
                command = new SqlCommand(cadena, conn);
                command.CommandType = CommandType.Text;
                command.CommandText = cadena;
                command.Parameters.AddWithValue("@IDUsuario", IDUsuario);
                command.Parameters.AddWithValue("@IDRol", usuario.IDRol);

                command.ExecuteNonQuery();
                conn.Close();


                return new { mensaje = "Se ingreso el usuario" };


            }
            catch (Exception ex)
            {

                throw new Exception("", ex);
            }
        }
        public object Modificar(int id, JsonElement Usuario)
        {
            try
            {
                var usuario = Usuario.Deserialize<UsuarioMOD>();
                int Activo = usuario.Estatus ? 1 : 0;
                var conn = conexion.GetConnection();
                conn.Open();


                string cadena = "update dbo.Usuario set Nombre = @Nombre,Apellido = @Apellido,Correo = @Correo where ID = @ID";
                SqlCommand command = new SqlCommand(cadena, conn);
                command.CommandType = CommandType.Text;
                command.CommandText = cadena;
                command.Parameters.AddWithValue("@Nombre", usuario.Nombre);
                command.Parameters.AddWithValue("@Apellido", usuario.Apellido);
                command.Parameters.AddWithValue("@Correo", usuario.Correo);

                command.Parameters.AddWithValue("@ID", id);
                command.ExecuteNonQuery();
                conn.Close();
                conn.Open();
                cadena = "update dbo.UsuarioRol set IDRol = @IDRol where IDUsuario = @IDUsuario";
                command = new SqlCommand(cadena, conn);
                command.CommandType = CommandType.Text;
                command.CommandText = cadena;
                command.Parameters.AddWithValue("@IDUsuario", id);
                command.Parameters.AddWithValue("@IDRol", usuario.IDRol);


                command.ExecuteNonQuery();
                conn.Close();
                return new { mensaje = "Se modifico el usuario" };


            }
            catch (Exception ex)
            {

                throw new Exception("", ex);
            }
        }

        public object ListarRol()
        {
            try
            {
                var conn = conexion.GetConnection();
                List<RolMOD> rol = new();
                conn.Open();


                string cadena = "select * from dbo.Rol ";
                SqlCommand command = new SqlCommand(cadena, conn);
                command.CommandType = CommandType.Text;
                command.CommandText = cadena;

                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {


                    rol.Add(new RolMOD
                    {
                        ID = (long)reader["ID"],
                        Nombre = reader["Nombre"].ToString(),
                        Descripcion = reader["Descripcion"].ToString(),


                    });

                }

                return rol;


            }
            catch (Exception ex)
            {

                throw new Exception("", ex);
            }
        }

        public object BuscarRol(int RolID)
        {
            try
            {

                var conn = conexion.GetConnection();
                var rol = new RolMOD();
                conn.Open();


                string cadena = "select * from dbo.Rol where RolID = @RolID ";
                SqlCommand command = new SqlCommand(cadena, conn);
                command.CommandType = CommandType.Text;
                command.CommandText = cadena;
                command.Parameters.AddWithValue("@RolID", RolID);

                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {

                    rol.ID = (long)reader["RolID"];
                    rol.Nombre = reader["Nombre"].ToString();
                    rol.Descripcion = reader["Descripcion"].ToString();



                }

                return rol;


            }
            catch (Exception ex)
            {

                throw new Exception("", ex);
            }
        }

        public object AgregarRol(JsonElement Rol)
        {
            try
            {
                var rol = Rol.Deserialize<RolMOD>();
                var conn = conexion.GetConnection();
                conn.Open();


                string cadena = "insert into dbo.Rol (Nombre,Descripcion ) values (@Nombre,@Descripcion )";
                SqlCommand command = new SqlCommand(cadena, conn);
                command.CommandType = CommandType.Text;
                command.CommandText = cadena;
                command.Parameters.AddWithValue("@Nombre", rol.Nombre);
                command.Parameters.AddWithValue("@Descripcion", rol.Descripcion);


                command.ExecuteNonQuery();

                return new { mensaje = "Se ingreso el rol" };


            }
            catch (Exception ex)
            {

                throw new Exception("", ex);
            }
        }
        public object ModificarRol(int id, JsonElement Rol)
        {
            try
            {
                var rol = Rol.Deserialize<RolMOD>();
                var conn = conexion.GetConnection();
                conn.Open();


                string cadena = "update dbo.Rol set Nombre = @Nombre,Descripcion = @Descripcion where RolID = @RolID";
                SqlCommand command = new SqlCommand(cadena, conn);
                command.CommandType = CommandType.Text;
                command.CommandText = cadena;
                command.Parameters.AddWithValue("@Nombre", rol.Nombre);
                command.Parameters.AddWithValue("@Descripcion", rol.Descripcion);
                command.Parameters.AddWithValue("@RolID", id);


                command.ExecuteNonQuery();

                return new { mensaje = "Se modifico el rol" };


            }
            catch (Exception ex)
            {

                throw new Exception("", ex);
            }
        }

        public object AsignalRol(JsonElement rol)
        {
            try
            {
                var usuarioRol = rol.Deserialize<UsuarioRolMOD>();
                var conn = conexion.GetConnection();
                conn.Open();


                string cadena = "insert into dbo.UsuarioRol (IDUsuario,IDRol ) values (@IDUsuario,@IDRol )";
                SqlCommand command = new SqlCommand(cadena, conn);
                command.CommandType = CommandType.Text;
                command.CommandText = cadena;
                command.Parameters.AddWithValue("@IDUsuario", usuarioRol.IDUsuario);
                command.Parameters.AddWithValue("@IDRol", usuarioRol.IDRol);




                command.ExecuteNonQuery();

                return new { mensaje = "Se ingreso el rol" };


            }
            catch (Exception ex)
            {

                throw new Exception("", ex);
            }
        }

        public object EditarUsuarioRol(long usuario,long rol)
        {
            try
            {
                var conn = conexion.GetConnection();
                conn.Open();


                string cadena = "update dbo.UsuarioRol set IDRol = @IDRol where IDUsuario = @IDUsuario";
                SqlCommand command = new SqlCommand(cadena, conn);
                command.CommandType = CommandType.Text;
                command.CommandText = cadena;
                command.Parameters.AddWithValue("@IDUsuario", usuario);
                command.Parameters.AddWithValue("@IDRol", rol);


                command.ExecuteNonQuery();

                return new { mensaje = "Se modifico el rol" };


            }
            catch (Exception ex)
            {

                throw new Exception("", ex);
            }
        }

        public UsuarioRolMOD BuscarRolUsuario(long usuarioID)
        {
            try
            {

                var conn = conexion.GetConnection();
                var usuario = new UsuarioRolMOD();
                conn.Open();


                string cadena = "select * from dbo.UsuarioRol where IDUsuario = @ID ";
                SqlCommand command = new SqlCommand(cadena, conn);
                command.CommandType = CommandType.Text;
                command.CommandText = cadena;
                command.Parameters.AddWithValue("@ID", usuarioID);

                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {

                    usuario.ID = (long)reader["ID"];
                    usuario.IDRol = (long)reader["IDRol"];




                }

                return usuario;


            }
            catch (Exception ex)
            {

                throw new Exception("", ex);
            }
        }
    }
}
