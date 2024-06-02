namespace APITicket.Modelo
{
    public class UsuarioMOD
    { 
        public long ID { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Contraseña { get; set; } 
        public string Correo { get; set; }
        public bool Estatus { get; set; }
        public long IDRol { get; set; }


    }
}
