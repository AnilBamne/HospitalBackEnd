using BusinessLayer.Interface;
using CommonLayer.Model;
using RepositoryLayer.Interface;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace BusinessLayer.Service
{
    public class PatientBusiness:IPatientBusiness
    {
        private readonly IPatientRepository patientRepository;
        public PatientBusiness(IPatientRepository patientRepository)
        {
            this.patientRepository = patientRepository;
        }

        public PatientRegModel Register(PatientRegModel model)
        {
            try
            {
                return this.patientRepository.Register(model);
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }
        public string PatientLogin(LoginModel model)
        {
            try
            {
                return this.patientRepository.PatientLogin(model);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<PatientRegModel> GetAllPatients()
        {
            try
            {
                return this.patientRepository.GetAllPatients();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<DocModel> GetAllDoctors()
        {
            return this.patientRepository.GetAllDoctors();
        }
        public string ForgotPassword(string patEmail)
        {
            return this.patientRepository.ForgotPassword(patEmail);
        }
        public string ResetPassword(ResetPasswordModel model, string patEmail)
        {
            return this.patientRepository.ResetPassword(model, patEmail);
        }
    }
}
