namespace APITicket.Modelo
{
    public class TicketMOD
    {
        public long ID { get; set; }
        public long IDTicket { get; set; }
        public long IDEncargado { get; set; }
        public long IDCreador { get; set; }
        public long IDPlantilla { get; set; }
        public string Descripcion { get; set; }
        public long IDPrioridad { get; set; }
        public long? IDEstado { get; set; }
        public string? Encargado { get; set; }
        public string? Creador { get; set; }
        public string? Titulo { get; set; }
        public string? Prioridad { get; set; }
        public string? Estado { get; set; }

                
    }
}
