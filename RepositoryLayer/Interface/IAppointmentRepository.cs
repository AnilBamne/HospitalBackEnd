using CommonLayer.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace RepositoryLayer.Interface
{
    public interface IAppointmentRepository
    {
        public string CreateAppointment(int patientId, int doctorId, int number, DateTime date, DateTime time, DateTime endTime);
        public List<AppointmentModel> GetAllAppointments();
    }
}
