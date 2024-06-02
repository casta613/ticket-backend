using System.Data.SqlClient;

namespace APITicket.Dato
{
    public class Conexion
    {
        
            private readonly IConfiguration configuration;
            public Conexion(IConfiguration configuration)
            {
                this.configuration = configuration;
            }
            public SqlConnection GetConnection()
            {
                SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
                builder.DataSource = configuration.GetSection("SQLServer").Value;
                builder.InitialCatalog = configuration.GetSection("SQLDatabase").Value;
               builder.UserID = configuration.GetSection("SQLlUser").Value;
                builder.Password = configuration.GetSection("SQLPassword").Value;
                builder.ApplicationName = configuration.GetSection("SQLDatabase").Value;

                return new SqlConnection(builder.ConnectionString);

            }
        
    }
}
