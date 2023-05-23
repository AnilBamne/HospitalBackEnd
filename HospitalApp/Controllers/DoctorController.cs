using BusinessLayer.Interface;
using CommonLayer.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace HospitalApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DoctorController : ControllerBase
    {
        private readonly IDoctorBusiness doctorBusiness;
        public DoctorController(IDoctorBusiness doctorBusiness)
        {
            this.doctorBusiness = doctorBusiness;
        }   
        [HttpPost("Register")]
        public ActionResult RegisterDoctor(DocRegModel docRegModel)
        {
            try
            {
                var result=this.doctorBusiness.Register(docRegModel);
                if (result != null)
                {
                    return Ok(new ResponseModel<DocRegModel> { Status = true,Message="Registration successfull",Data=result });
                }
                else
                {
                    return BadRequest(new ResponseModel<DocRegModel> { Status = false, Message = "Registration failed", Data = result });
                }
            }
            catch(System.Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost("Login")]     //this is controller
        public ActionResult DoctorLogin(LoginModel loginModel)
        {
            try
            {
                var result = doctorBusiness.LoginDoc(loginModel);
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

        [HttpPost("ForgotPassword")]
        public ActionResult ForgotPassword(string email)
        {
            try
            {
                var result = doctorBusiness.ForgotPassword(email);
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
                var result = doctorBusiness.ResetPassword(model,email);
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



        [Authorize]
        [HttpGet("GetAppointments")]
        public ActionResult GetAppointments()
        {
            try
            {
                //fetching id from token
                int doctorId = Convert.ToInt32(User.Claims.FirstOrDefault(a => a.Type == "UserId").Value);
                var result = this.doctorBusiness.GetMyAppointments(doctorId);
                if (result != null)
                {
                    return Ok(new ResponseModel<List<AppointmentModel>> { Status = true, Message = "Fetching Appointments successfull", Data = result });
                }
                else
                {
                    return BadRequest(new ResponseModel<List<AppointmentModel>> { Status = false, Message = "Fetching Appointments failed", Data = result });
                }
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }
        
    }
}
