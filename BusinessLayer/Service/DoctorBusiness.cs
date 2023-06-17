using BusinessLayer.Interface;
using CommonLayer.Model;
using RepositoryLayer.Interface;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace BusinessLayer.Service
{
    public class DoctorBusiness:IDoctorBusiness
    {
        private readonly IDoctorRepository doctorRepository;
        public DoctorBusiness(IDoctorRepository doctorRepository)
        {
            this.doctorRepository = doctorRepository;
        }
        public DocRegModel Register(DocRegModel model)
        {
            try
            {
                return doctorRepository.Register(model);
            }
            catch (Exception e)
            {

                throw e;
            }
        }
        public string LoginDoc(LoginModel model)
        {
            
            try
            {
                return this.doctorRepository.LoginDoc(model);
            }
            catch (Exception e)
            {

                throw e;
            }
        }
        public List<DocRegModel> GetAllDoctors()
        {
            
            try
            {
                return doctorRepository.GetAllDoctors();
            }
            catch (Exception e)
            {

                throw e;
            }
        }
        public List<AppointmentModel> GetMyAppointments(int doctorId)
        {
            try
            {
                return doctorRepository.GetMyAppointments(doctorId);

            }
            catch (Exception e)
            {

                throw e;
            }
        }
        public string ForgotPassword(string docEmail)
        {
            try
            {
                return doctorRepository.ForgotPassword(docEmail);

            }
            catch (Exception e)
            {

                throw e;
            }
        }
        public string ResetPassword(ResetPasswordModel model, string docEmail)
        {
            return doctorRepository.ResetPassword(model, docEmail);
        }

        public List<PatientRegModel> GetMyPatients(int docId)
        {
            return doctorRepository.GetMyPatients(docId);
        }
    }
}
