using BusinessLayer.Interface;
using BusinessLayer.Service;
using CommonLayer.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;

namespace HospitalApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PatientController : ControllerBase
    {
        private readonly IPatientBusiness patientBusiness;
        public PatientController(IPatientBusiness patientBusiness, IConfiguration configuration)
        {
            this.patientBusiness = patientBusiness;
        }

        [HttpPost("Register")]
        public ActionResult RegisterPatient(PatientRegModel model)
        {
            try
            {
                var result = this.patientBusiness.Register(model);
                if (result != null)
                {
                    return Ok(new ResponseModel<PatientRegModel> { Status = true, Message = "Registration successfull", Data = result });
                }
                else
                {
                    return BadRequest(new ResponseModel<PatientRegModel> { Status = false, Message = "Registration failed", Data = result });
                }
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost("Login")]     //this is controller
        public ActionResult PatientLogin(LoginModel loginModel)
        {
            try
            {
                var result = patientBusiness.PatientLogin(loginModel);
                if (result != null)
                {
                    return Ok(new ResponseModel<string> { Status = true, Message = "Login succesfull", Data = result });
                }
                else
                {
                    return BadRequest(new ResponseModel<string> { Status = false, Message = "Login failed", Data = result });
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //[HttpGet("GetAllPatients")]
        //public ActionResult GetAllPatients()
        //{
        //    try
        //    {
        //        var result = this.patientBusiness.GetAllPatients();
        //        if (result != null)
        //        {
        //            return Ok(new ResponseModel<List<PatientRegModel>> { Status = true, Message = "Fetching Patients successfull", Data = result });
        //        }
        //        else
        //        {
        //            return BadRequest(new ResponseModel<List<PatientRegModel>> { Status = false, Message = "Fetching Patients failed", Data = result });
        //        }
        //    }
        //    catch (System.Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        [Authorize]
        [HttpGet("GetAllDoctors")]
        public ActionResult GetAllDoctors()
        {
            try
            {
                var result = this.patientBusiness.GetAllDoctors();
                if (result != null)
                {
                    return Ok(new ResponseModel<List<DocModel>> { Status = true, Message = "Fetching doctors successfull", Data = result });
                }
                else
                {
                    return BadRequest(new ResponseModel<List<DocModel>> { Status = false, Message = "Fetching doctors failed", Data = result });
                }
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost("ForgotPassword")]
        public ActionResult ForgotPassword(string email)
        {
            try
            {
                var result = patientBusiness.ForgotPassword(email);
                if (result != null)
                {
                    return Ok(new ResponseModel<string> { Status = true, Message = "Reset link sent successfully", Data = result });
                }
                else
                {
                    return BadRequest(new ResponseModel<string> { Status = false, Message = "Invalid email address", Data = result });
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [HttpPut("ResetPassword")]
        public ActionResult ResetPassword(ResetPasswordModel model, string email)
        {
            try
            {
                var result = patientBusiness.ResetPassword(model, email);
                if (result != null)
                {
                    return Ok(new ResponseModel<string> { Status = true, Message = "Reset password successfull", Data = result });
                }
                else
                {
                    return BadRequest(new ResponseModel<string> { Status = false, Message = "Reset password failed", Data = result });
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}
