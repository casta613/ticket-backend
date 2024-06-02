namespace APITicket.Modelo
{
    public class ReservaHabitacionMOD
    {
        public long ReservaHabitacionID { get; set; }
        public long ClienteID { get; set; }
        public long HabitacionID { get; set; }
      
        public DateTime? Fecha { get; set; }
        public DateTime? FechaEntrada { get; set; }
        public DateTime? FechaSalida { get; set; }
        public string? Cliente { get; set; }
        public string? Celular { get; set; }
        public string? NumeroHabitacion { get; set; }

    }
}
