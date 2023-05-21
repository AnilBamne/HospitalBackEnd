using CommonLayer.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace RepositoryLayer.Interface
{
    public interface IAdminRepository
    {
        public string AdminLogin(string email, string password);
        public List<AppointmentModel> GetAllAppointments();
        public List<DocModel> GetAllDoctors();
        public List<PatientRegModel> GetAllPatients();
        public int CheckUser(string email, string pass);
        public AdminRegModel Register(AdminRegModel model);
        public string DeleteAppointment(int appointmentId);
        public bool AllowDoctor(int docId);
        public bool RestrictDoctor(int docId);
    }
}
