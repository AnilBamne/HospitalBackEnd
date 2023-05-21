using BusinessLayer.Interface;
using CommonLayer.Model;
using RepositoryLayer.Interface;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace BusinessLayer.Service
{
    public class AppointmentBusiness:IAppointmentBusiness
    {
        private readonly IAppointmentRepository appointmentRepository;
        public AppointmentBusiness(IAppointmentRepository appointmentRepository)
        {
            this.appointmentRepository = appointmentRepository;
        }

        public string CreateAppointment(int patientId, int doctorId, int number, DateTime date, DateTime time, DateTime endTime)

        {
            try
            {
                return appointmentRepository.CreateAppointment(patientId,doctorId,number,date,time,endTime);
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }
        public List<AppointmentModel> GetAllAppointments()
        {
            try
            {
                return appointmentRepository.GetAllAppointments();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
