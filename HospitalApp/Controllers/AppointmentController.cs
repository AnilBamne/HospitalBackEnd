using BusinessLayer.Interface;
using CommonLayer.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;

namespace HospitalApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AppointmentController : ControllerBase
    {
        private readonly IAppointmentBusiness appointmentBusiness;
        public AppointmentController(IConfiguration configuration, IAppointmentBusiness appointmentBusiness)
        {
            this.appointmentBusiness = appointmentBusiness;
        }
        [Authorize]
        [HttpPost]
        [Route("CreateAppointment")]
        public ActionResult CreateAppointment(int doctorId, int number, DateTime date, DateTime time, DateTime endTime)
        {
            int patId=Convert.ToInt32(User.Claims.FirstOrDefault(a=>a.Type=="UserId").Value);
            var result = appointmentBusiness.CreateAppointment(patId,doctorId,number,date,time,endTime);
            if (result == 1)
            {
                return Ok(new ResponseModel<int> { Status = true, Message = "Appointment Created successfull", Data = result });
            }
            else if(result == 2)
            {
                return BadRequest(new ResponseModel<int> { Status = false, Message = "Appointment failed, Invalid Input", Data = result });
            }
            else if (result == 0)
            {
                return BadRequest(new ResponseModel<int> { Status = false, Message = "Appointment Already Exists", Data = result });
            }
            else
            {
                return BadRequest(new ResponseModel<int> { Status = false, Message = "Appointment failed", Data = result });
            }
        }
        [Authorize]
        [HttpGet("GetAllAppointments")]
        public ActionResult GetAllAppointments()
        {
            try
            {
                var result = this.appointmentBusiness.GetAllAppointments();
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
        [HttpPut("UpdateAppointment")]
        public ActionResult UpdateAppointment(int appointmentId ,DateTime date, DateTime time, DateTime endTime)
        {
            //int appId = Convert.ToInt32(User.Claims.FirstOrDefault(a => a.Type == "UserId").Value);
            var result = appointmentBusiness.UpdateAppoinment(appointmentId, date, time, endTime);
            if (result != null)
            {
                return Ok(new ResponseModel<string> { Status = true, Message = "Appointment Updated successfull", Data = result });
            }
            else
            {
                return BadRequest(new ResponseModel<string> { Status = false, Message = "Appointment Update failed", Data = result });
            }
        }
    }
}
