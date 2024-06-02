namespace APITicket.MOD
{
    public class RespAccesoMOD
    {
        public DateTime HoraExpiracion { get; set; }
        public string Token { get; set; }
        public string Rol { get; set; }

        public string? QR { get; set; }
    }
}
