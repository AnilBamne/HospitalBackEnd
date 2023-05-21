using System;
using System.Collections.Generic;
using System.Text;

namespace CommonLayer.Model
{
    public class DocModel
    {
        public int DoctorId { get; set; }
        public string DoctorName { get; set; }
        public string DoctorGender { get; set; }
        public string DoctorSpecialization { get; set; }
        public int DoctorNumber { get; set; }
        public string DoctorEmail { get; set; }
        public string DoctorPassword { get; set; }
        public bool Status { get; set; }
    }
}
