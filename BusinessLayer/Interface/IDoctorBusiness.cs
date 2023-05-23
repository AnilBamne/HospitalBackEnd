using CommonLayer.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLayer.Interface
{
    public interface IDoctorBusiness
    {
        public DocRegModel Register(DocRegModel model);
        public string LoginDoc(LoginModel model);
        public List<DocRegModel> GetAllDoctors();
        public List<AppointmentModel> GetMyAppointments(int doctorId);
        public string ForgotPassword(string docEmail);
        public string ResetPassword(ResetPasswordModel model, string docEmail);
      
    }
}
