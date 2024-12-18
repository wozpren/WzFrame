namespace WzFrame.Entity.Workshop
{
    [SugarTable("Appointment")]
    public class Appointment : EntityBase
    {
        public string UserId { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;

        public DateTime StartTime { get; set; }

        public DateTime EndTime { get; set; }

        public int State { get; set; }
    }

    [SugarTable("AppointList")]
    public class AppointList : EntityBase
    {
        public int Count { get; set; }
    }




    public class AppointmentDto
    {
        public long AppointmentId { get; set; }       
        public long UserId { get; set; }
        public int IndexId { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
    }
}
