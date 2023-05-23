using CommonLayer.Model;
using Microsoft.Extensions.Configuration;
using RepositoryLayer.Interface;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace RepositoryLayer.Service
{
    public class AppointmentRepository:IAppointmentRepository
    {
        private readonly IConfiguration configuration;
        private static string connectionString;
        SqlConnection connection;
        public AppointmentRepository(IConfiguration configuration)
        {
            this.configuration = configuration;
            connectionString = configuration.GetConnectionString("HospitalDB");
            connection = new SqlConnection(connectionString);
        }

        public string CreateAppointment(int patientId,int doctorId,int number,DateTime date,DateTime time,DateTime endTime)
        {
            using (connection)
            {

                try
                {
                    SqlCommand command = new SqlCommand("spCreateAppointment", connection);
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@PatientId", patientId);
                    command.Parameters.AddWithValue("@DoctorId", doctorId);
                    command.Parameters.AddWithValue("@Number", number);
                    command.Parameters.AddWithValue("@Date", date);
                    command.Parameters.AddWithValue("@Time",time);
                    command.Parameters.AddWithValue("@EndTime",endTime);


                    connection.Open();
                    int count = command.ExecuteNonQuery();
                    if (count != 0)
                    {
                        return "Appointment created succesfully";
                    }
                    else
                    {
                        return "Failed";
                    }
                }
                catch (Exception ex)
                {

                    throw ex;
                }
                finally
                {
                    connection.Close();
                }
            }
        }
        public List<AppointmentModel> GetAllAppointments()
        {
            try
            {
                using (this.connection)
                {
                    SqlCommand command = new SqlCommand("spGetAllAppointments", this.connection);
                    this.connection.Open();
                    command.CommandType = CommandType.StoredProcedure;
                    SqlDataReader reader = command.ExecuteReader();
                    if (reader.HasRows)
                    {
                        List<AppointmentModel> list = new List<AppointmentModel>();
                        while (reader.Read())
                        {
                            AppointmentModel model = new AppointmentModel();
                            model.AppointmentId = reader.GetInt32(0);
                            model.PatientId = reader.GetInt32(1);
                            model.PatientName = reader.GetString(2);
                            model.PatientEmail = reader.GetString(3);
                            model.Date = reader.GetDateTime(4);
                            model.Time = reader.GetTimeSpan(5);
                            model.Number = reader.GetInt32(6);
                            model.DoctorId = reader.GetInt32(7);
                            model.DoctorName = reader.GetString(8);
                            model.Desies=reader.GetString(9);
                            model.EndTime = reader.GetTimeSpan(10);
                            list.Add(model);
                        }
                        return list;
                    }

                    return null;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                this.connection.Close();
            }
        }

        public string UpdateAppoinment(int appointmentId, DateTime date, DateTime time, DateTime endTime)
        {
            try
            {
                using (this.connection)
                {
                    SqlCommand command = new SqlCommand("spUpdateAppointment", this.connection);
                    connection.Open();
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@AppointmentId", appointmentId);
                    command.Parameters.AddWithValue("@Date", date);
                    command.Parameters.AddWithValue("@VisitTime", time);
                    command.Parameters.AddWithValue("@EndTime", endTime);
                    
                    int count = command.ExecuteNonQuery();
                    if (count == 1)
                    {
                        return "Appointment Updated succesfully";
                    }
                    else
                    {
                        return "Update Failed";
                    }
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally { this.connection.Close(); }
        }
    }
}
