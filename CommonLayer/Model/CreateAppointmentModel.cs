using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Text;

namespace CommonLayer.Model
{
    public class CreateAppointmentModel
    {
        public int PatientId { get; set; }
        public int DoctorId { get; set; }
        public int Number { get; set; }
        public DateTime Date { get; set; }
        public SqlDateTime Time  { get; set; }

    }
}
