using BusinessLayer.Interface;
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
    public class AdminController : ControllerBase
    {
        private readonly IConfiguration configuration;
        private readonly IAdminBusiness adminBusiness;

        public AdminController(IConfiguration configuration, IAdminBusiness adminBusiness)
        {
            this.configuration = configuration;
            this.adminBusiness = adminBusiness;
        }
        [HttpPost("login")]
        public ActionResult AdminLogin(LoginModel model)
        {
            var result = adminBusiness.AdminLogin(model);
            if(result != null)
            {
                return Ok(new ResponseModel<string> { Status = true, Message = "Login Successfull", Data = result });
            }
            else
            {
                return BadRequest(new ResponseModel<string> { Status = false, Message = "Login Failed", Data = result });
            }
        }
        [Authorize]
        [HttpGet("GetAllAppointments")]
        public ActionResult GetAllAppointments()
        {
            try
            {
                var result = this.adminBusiness.GetAllAppointments();
                if (result != null)
                {
                    return Ok(new ResponseModel<List<AppointmentModel>> { Status = true, Message = "Fetching Appontments successfull", Data = result });
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

        [Authorize]
        [HttpGet("GetAllDocs")]
        public ActionResult GetAllDoctors()
        {
            try
            {
                var result = this.adminBusiness.GetAllDoctors();
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
        [Authorize]
        [HttpGet("GetAllPatients")]
        public ActionResult GetAllPatients()
        {
            try
            {
                var result = this.adminBusiness.GetAllPatients();
                if (result != null)
                {
                    return Ok(new ResponseModel<List<PatientRegModel>> { Status = true, Message = "Fetching Patients successfull", Data = result });
                }
                else
                {
                    return BadRequest(new ResponseModel<List<PatientRegModel>> { Status = false, Message = "Fetching Patients failed", Data = result });
                }
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost("CheckUser")]
        public ActionResult CheckUser(string email, string password)
        {
            var result = adminBusiness.CheckUser(email, password);
            if (result != 0)
            {
                return Ok(new ResponseModel<int> { Status = true, Message = "Login Successfull", Data = result });
            }
            else
            {
                return BadRequest(new ResponseModel<int> { Status = false, Message = "Login Failed", Data = result });
            }
        }

        //[HttpPost("Register")]
        //public ActionResult RegisterAdmin(AdminRegModel model)
        //{
        //    try
        //    {
        //        var result = this.adminBusiness.Register(model);
        //        if (result != null)
        //        {
        //            return Ok(new ResponseModel<AdminRegModel> { Status = true, Message = "Registration successfull", Data = result });
        //        }
        //        else
        //        {
        //            return BadRequest(new ResponseModel<AdminRegModel> { Status = false, Message = "Registration failed", Data = result });
        //        }
        //    }
        //    catch (System.Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        [Authorize]
        [HttpDelete("DeleteAppointment")]
        public ActionResult DeleteAppointment(int Id)
        {
            var result = adminBusiness.DeleteAppointment(Id);
            if (result != null)
            {
                return Ok(new ResponseModel<string> { Status = true, Message = "Appointment Deleted Successfully", Data = result });
            }
            else
            {
                return BadRequest(new ResponseModel<string> { Status = false, Message = "Deletion Failed", Data = result });
            }
        }

        [Authorize]
        [HttpPut("AllowAccess")]
        public ActionResult AllowAccess(int docId)
        {
            try
            {
                var result=adminBusiness.AllowDoctor(docId);
                if (result == true)
                {
                    return Ok(new ResponseModel<bool> { Status = true, Message = "Access Granted", Data = result });
                }
                else
                {
                    return BadRequest(new ResponseModel<bool> { Status = false, Message = "Access Granting Failed", Data = result });
                }
            }
            catch(Exception EX)
            {
                throw EX;
            }
        }


        [Authorize]
        [HttpPut("RemoveAccess")]
        public ActionResult RemoveAccess(int docId)
        {
            try
            {
                var result = adminBusiness.RestrictDoctor(docId);
                if (result == true)
                {
                    return Ok(new ResponseModel<bool> { Status = true, Message = "Access Removed", Data = result });
                }
                else
                {
                    return BadRequest(new ResponseModel<bool> { Status = false, Message = "Access Removal Failed", Data = result });
                }
            }
            catch (Exception EX)
            {
                throw EX;
            }
        }
    }
}
