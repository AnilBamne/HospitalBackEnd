using BusinessLayer.Interface;
using CommonLayer.Model;
using RepositoryLayer.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLayer.Service
{
    public class AdminBusiness: IAdminBusiness
    {
        private readonly IAdminRepository adminRepository;

        public AdminBusiness(IAdminRepository adminRepository)
        {
            this.adminRepository = adminRepository;
        }
        public string AdminLogin(string email, string password)
        {
            try
            {
                return adminRepository.AdminLogin(email, password);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public List<AppointmentModel> GetAllAppointments()
        {
            return adminRepository.GetAllAppointments();
        }

        public List<DocModel> GetAllDoctors()
        {
            return adminRepository.GetAllDoctors();
        }

        public List<PatientRegModel> GetAllPatients()
        {
            return adminRepository.GetAllPatients();
        }
        public int CheckUser(string email, string pass)
        {
            return adminRepository.CheckUser(email, pass);
        }
        public AdminRegModel Register(AdminRegModel model)
        {
            return adminRepository.Register(model);
        }
        public string DeleteAppointment(int appointmentId)
        {
            return adminRepository.DeleteAppointment(appointmentId);
        }

        public bool AllowDoctor(int docId)
        {
            return adminRepository.AllowDoctor(docId);
        }
        public bool RestrictDoctor(int docId)
        {
            return adminRepository.RestrictDoctor(docId);
        }
    }
}
