using CommonLayer.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace RepositoryLayer.Interface
{
    public interface IPatientRepository
    {
        public PatientRegModel Register(PatientRegModel model);
        public string PatientLogin(LoginModel model);
        public List<PatientRegModel> GetAllPatients();
        public List<DocModel> GetAllDoctors();
        public string ForgotPassword(string patEmail);
        public string ResetPassword(ResetPasswordModel model, string patEmail);
    }
}
