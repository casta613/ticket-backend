using System.Text;
using System.Text.Json;
using APITicket.MOD;
using APITicket.Dato;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Data;
using System.Data.SqlClient;
using TwoFactorAuthNet.Providers.Qr;
using TwoFactorAuthNet;

namespace APITicket.BLL
{
    public class Acceso
    {
        private readonly IConfiguration configuration;
        private Conexion dataBase;

        public Acceso(IConfiguration configuration)
        {
            this.configuration = configuration;
            dataBase = new(this.configuration);

        }


        public (object, int) ValidarAcceso(ReqAccesoMOD reqAccesoMod)
        {
            try
            {
                object response;
                int estatus = 0;
                //var reqAccesoMod = request.Deserialize<ReqAccesoMOD>();
                (string contrasenia,string rol )= Buscar(reqAccesoMod.Usuario);
                if (BCrypt.Net.BCrypt.Verify( reqAccesoMod.Contrasenia,contrasenia.Trim())) {
                    var issuer = configuration.GetSection("Jwt:Issuer").Value;
                    var audience = configuration.GetSection("Jwt:Audience").Value;
                    var key = Encoding.UTF8.GetBytes(configuration.GetSection("Jwt:KeyCode").Value);
                    var tokenDescripcion = new SecurityTokenDescriptor
                    {
                        Subject = new ClaimsIdentity(new[]
                        {
                            new Claim("Id", Guid.NewGuid().ToString().ToUpper()),
                            new Claim(JwtRegisteredClaimNames.Sub, reqAccesoMod.Usuario),
                            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString().ToUpper())
                        }),
                        Expires = DateTime.UtcNow.AddMinutes(double.Parse(configuration.GetSection("TokenExpires").Value)),
                        Issuer = issuer,
                        Audience = audience,
                        SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha512Signature),

                    };

                    var tokenHandler = new JwtSecurityTokenHandler();
                    var securityToken = tokenHandler.CreateToken(tokenDescripcion);
                    var jwtToken = tokenHandler.WriteToken(securityToken);
                    RespAccesoMOD respAcceso = new();
                    respAcceso.HoraExpiracion = DateTime.UtcNow.AddMinutes(double.Parse(configuration.GetSection("TokenExpires").Value));
                    respAcceso.Token = jwtToken;
                    respAcceso.Rol = rol;
                    estatus = 200;

                    string secret = Secret2FA(reqAccesoMod.Usuario);

                    if(secret == null || secret == "")
                    {
                        var tfa = new TwoFactorAuth("app-ticket", 6, 30, Algorithm.SHA256, new ImageChartsQrCodeProvider());
                        secret = tfa.CreateSecret(160);
                        AgregarSecret2FA(reqAccesoMod.Usuario, secret);
                        string imgQR = tfa.GetQrCodeImageAsDataUri(reqAccesoMod.Usuario,secret,250);
                        respAcceso.QR = imgQR;
                    }                    

                     response = respAcceso;
                }
                else
                {
                    response = new {mensage ="El usuario o la contraseña son incorrectos" };
                    estatus = 400;
                }
                return (response, estatus);

            }
            catch (Exception ex)
            {
                return (ex, 500);
            }
        }

        private string Secret2FA(string correo)
        {
            try
            {
                string secret = string.Empty;
                var conn = dataBase.GetConnection();
                conn.Open();


                string cadena = "select * from dbo.Usuario u  where Correo = @Correo ";
                SqlCommand command = new SqlCommand(cadena, conn);
                command.CommandType = CommandType.Text;
                command.CommandText = cadena;
                command.Parameters.AddWithValue("@Correo", correo);

                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {

                    secret = reader["LlaveAutenticacion"].ToString();


                }

                return secret.Trim();


            }
            catch (Exception ex)
            {

                throw new Exception("", ex);
            }
        }

        private void AgregarSecret2FA(string correo,string secret)
        {
            try
            {
                var conn = dataBase.GetConnection();
                conn.Open();


                string cadena = "update dbo.Usuario set LlaveAutenticacion = @LlaveAutenticacion where Correo = @Correo";
                SqlCommand command = new SqlCommand(cadena, conn);
                command.CommandType = CommandType.Text;
                command.CommandText = cadena;
                command.Parameters.AddWithValue("@LlaveAutenticacion", secret);
                command.Parameters.AddWithValue("@Correo", correo);

                command.ExecuteNonQuery();

            }
            catch (Exception ex)
            {

                throw new Exception("", ex);
            }
        }
        public (string,string) Buscar(string Correo)
        {
            try
            {
                string contrasenia = string.Empty;
                string rol = string.Empty;
                var conn = dataBase.GetConnection();
                conn.Open();


                string cadena = "select * from dbo.Usuario u  where Correo = @Correo ";
                SqlCommand command = new SqlCommand(cadena, conn);
                command.CommandType = CommandType.Text;
                command.CommandText = cadena;
                command.Parameters.AddWithValue("@Correo", Correo);

                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {

                    rol = reader["Nombre"].ToString();
                    contrasenia = reader["Contraseña"].ToString();


                }

                return (contrasenia,rol);


            }
            catch (Exception ex)
            {

                throw new Exception("", ex);
            }
        }

        public (object, int) ValidarCodigo(string codigo,string usuario)
        {
            try
            {

                string secret = Secret2FA(usuario);
                var tfa = new TwoFactorAuth("app-ticket",6,30,Algorithm.SHA256);
                bool resp = tfa.VerifyCode(secret, codigo);


                object response;
                int estatus = 0;
                if (resp)
                {
                    var issuer = configuration.GetSection("Jwt:Issuer").Value;
                    var audience = configuration.GetSection("Jwt:Audience").Value;
                    var key = Encoding.UTF8.GetBytes(configuration.GetSection("Jwt:Key").Value);
                    var tokenDescripcion = new SecurityTokenDescriptor
                    {
                        Subject = new ClaimsIdentity(new[]
                        {
                            new Claim("Id", Guid.NewGuid().ToString().ToUpper()),
                            new Claim(JwtRegisteredClaimNames.Sub, usuario),
                            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString().ToUpper())
                        }),
                        Expires = DateTime.UtcNow.AddMinutes(double.Parse(configuration.GetSection("TokenExpires").Value)),
                        Issuer = issuer,
                        Audience = audience,
                        SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha512Signature),

                    };

                    var tokenHandler = new JwtSecurityTokenHandler();
                    var securityToken = tokenHandler.CreateToken(tokenDescripcion);
                    var jwtToken = tokenHandler.WriteToken(securityToken);
                    RespAccesoMOD respAcceso = new();
                    respAcceso.HoraExpiracion = DateTime.UtcNow.AddMinutes(double.Parse(configuration.GetSection("TokenExpires").Value));
                    respAcceso.Token = jwtToken;
                    estatus = 200;
                   
                    response = respAcceso;
                }
                else
                {
                    response = new { mensage = "El codigo es incorrectos" };
                    estatus = 400;
                }
                return (response, estatus);


            }
            catch (Exception ex)
            {

                throw new Exception("", ex);
            }
        }


        public object ValidarPermiso(string usuario)
        {
            try
            {

                var conn = dataBase.GetConnection();
                List<long> Permisos = new();

                conn.Open();


                string cadena = "select p.ID  from dbo.Usuario u "+
                                "inner join dbo.UsuarioRol ur on ur.IDUsuario = u.ID "+
                                "inner join dbo.PermisoRol pr on pr.IDRol = ur.IDRol " +
                                "inner join dbo.Permiso p on p.ID = pr.IDPermiso " +
                                "where u.Correo = @Correo";
                SqlCommand command = new SqlCommand(cadena, conn);
                command.CommandType = CommandType.Text;
                command.CommandText = cadena;
                command.Parameters.AddWithValue("@Correo", usuario);

                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    Permisos.Add((long)reader["ID"]);
                }

                return Permisos;


            }
            catch (Exception ex)
            {

                throw new Exception("", ex);
            }
        }
    }

    
}
