using System;
using System.Collections.Generic;
using System.Text;

namespace CommonLayer.Model
{
    public class AppointmentModel
    {
        public int AppointmentId { get; set; }
        public int PatientId { get; set; }
        public string PatientName { get; set; }
        public string PatientEmail { get; set; }
        public DateTime Date { get; set; }
        public TimeSpan Time { get; set; }
        public int Number { get; set; }
        public int DoctorId { get; set; }
        public string DoctorName { get; set; }
        public string Desies { get; set; }
        public TimeSpan EndTime { get; set; }
        public bool isTrash { get; set; }

    }
}
