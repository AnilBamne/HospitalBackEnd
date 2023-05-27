using CommonLayer.Model;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using RepositoryLayer.Interface;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Reflection;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace RepositoryLayer.Service
{
    public class AdminRepository : IAdminRepository
    {
        private readonly IConfiguration configuration;
        public static string connectionString;
        SqlConnection connection;
        public AdminRepository(IConfiguration configuration)
        {
            this.configuration = configuration;
            connectionString = configuration.GetConnectionString("HospitalDB");
            connection = new SqlConnection(connectionString);
        }

        public AdminRegModel Register(AdminRegModel model)
        {
            try
            {
                using (this.connection)
                {
                    string quary = @"Insert into AdminTable(AdminName,AdminEmail,AdminPassword)Values(@name,@email,@pass)";
                    SqlCommand cmd = new SqlCommand(quary, this.connection);
                    cmd.Parameters.AddWithValue("@name", model.AdminName);
                    cmd.Parameters.AddWithValue("@email", model.AdminEmail);
                    cmd.Parameters.AddWithValue("@pass",EncryptPassword(model.AdminPass));
                    this.connection.Open();
                    int count = cmd.ExecuteNonQuery();
                    if (count != 0)
                    {
                        return model;
                    }
                    else
                    {
                        return null;

                    }
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
        public string EncryptPassword(string password)
        {
            try
            {
                var passwordTextForm = Encoding.UTF8.GetBytes(password);
                return Convert.ToBase64String(passwordTextForm);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public string AdminLogin(LoginModel model)
        {
            try
            {
                using (this.connection)
                {
                    SqlCommand command = new SqlCommand("spAdminLogin", this.connection);
                    this.connection.Open();
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@Email", model.Email);
                    command.Parameters.AddWithValue("@Password",EncryptPassword(model.Password));
                    SqlDataReader reader = command.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            var Id = reader.GetInt32(0);
                            var token = GenerateToken(model.Email, Id);
                            return token;
                        }
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
        private string GenerateToken(string adminEmailId, long adminId)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var claims = new[]
            {
                new Claim("Email",adminEmailId),
                new Claim("UserId",adminId.ToString())
            };
            var token = new JwtSecurityToken(
                issuer: configuration["Jwt:Issuer"],
                audience: configuration["Jwt:Audience"],
                claims,
                expires: DateTime.Now.AddHours(4),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
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
                            model.Desies = reader.GetString(9);
                            model.EndTime = reader.GetTimeSpan(10);
                            model.isTrash = reader.GetBoolean(11);
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

        public string EditAppointment()
        {
            return "";
        }

        public List<DocModel> GetAllDoctors()
        {
            try
            {
                using (this.connection)
                {
                    SqlCommand command = new SqlCommand("spGetAllDoctors", this.connection);
                    this.connection.Open();
                    command.CommandType = CommandType.StoredProcedure;
                    SqlDataReader reader = command.ExecuteReader();
                    if (reader.HasRows)
                    {
                        List<DocModel> list = new List<DocModel>();
                        while (reader.Read())
                        {
                            DocModel model = new DocModel();

                            model.DoctorId = reader.GetInt32(0);
                            model.DoctorName = reader.GetString(1);
                            model.DoctorGender = reader.GetString(2);
                            model.DoctorSpecialization = reader.GetString(3);
                            model.DoctorNumber = reader.GetInt32(4);
                            model.DoctorEmail = reader.GetString(5);
                            model.DoctorPassword = reader.GetString(6);
                            model.Status = reader.GetBoolean(7);
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

        public List<PatientRegModel> GetAllPatients()
        {
            try
            {
                using (this.connection)
                {
                    SqlCommand command = new SqlCommand("spGetAllPatients", this.connection);
                    this.connection.Open();
                    command.CommandType = CommandType.StoredProcedure;
                    SqlDataReader reader = command.ExecuteReader();
                    if (reader.HasRows)
                    {
                        List<PatientRegModel> list = new List<PatientRegModel>();
                        while (reader.Read())
                        {
                            PatientRegModel model = new PatientRegModel();

                            model.PatientFirstName = reader.GetString(1);
                            model.PatientLastName = reader.GetString(2);
                            model.PatientEmail = reader.GetString(3);
                            model.PatientPassword = reader.GetString(4);
                            model.PatientGender = reader.GetString(5);
                            model.PatientAddress = reader.GetString(6);
                            model.PatientCity = reader.GetString(7);
                            model.PatientState = reader.GetString(8);
                            model.PatientDesies = reader.GetString(9);
                           
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

        public int CheckUser(string email,string pass)
        {
            try
            {
                using (this.connection)
                {
                    SqlCommand command = new SqlCommand("spCheckEmailAnPassword", this.connection);
                    this.connection.Open();
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@email", email);
                    command.Parameters.AddWithValue("@password", EncryptPassword(pass));
                    SqlDataReader reader = command.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            var num=reader.GetInt32(0);
                            return num;
                        }
                    }

                    return 0;
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

        public string DeleteAppointment(int appointmentId)
        {
            try { 
                using (this.connection) 
                {
                    string query = @"Update AppointmentsTable Set Trash='true' where AppointmentId=@Id;";
                    SqlCommand sqlCommand = new SqlCommand(query, this.connection);
                    sqlCommand.Parameters.AddWithValue("@Id", appointmentId);
                    connection.Open();
                    int count= sqlCommand.ExecuteNonQuery();
                    if (count > 0)
                    {
                        return "Appointment Deleted Successfully";
                    }
                    return "Appointment Not found";
                }
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        public bool AllowDoctor(int docId)
        {
            try
            {
                using (this.connection)
                {
                    string query = @"Update DoctorTable Set Status='true' where DoctorId=@Id";
                    SqlCommand sqlCommand = new SqlCommand(query, this.connection);
                    sqlCommand.Parameters.AddWithValue("@Id", docId);
                    connection.Open();
                    int count = sqlCommand.ExecuteNonQuery();
                    if (count > 0)
                    {
                        return true;
                    }
                    return false;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public bool RestrictDoctor(int docId)
        {
            try
            {
                using (this.connection)
                {
                    string query = @"Update DoctorTable Set Status='false' where DoctorId=@Id";
                    SqlCommand sqlCommand = new SqlCommand(query, this.connection);
                    sqlCommand.Parameters.AddWithValue("@Id", docId);
                    connection.Open();
                    int count = sqlCommand.ExecuteNonQuery();
                    if (count > 0)
                    {
                        return true;
                    }
                    return false;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
