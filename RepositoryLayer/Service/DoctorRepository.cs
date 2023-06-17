using CommonLayer;
using CommonLayer.Model;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using RepositoryLayer.Interface;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Reflection;
using System.Security.Claims;
using System.Text;
using System.Threading;

namespace RepositoryLayer.Service
{

    public class DoctorRepository: IDoctorRepository
    {
        private readonly IConfiguration configuration;
        public static string connectionString;
        SqlConnection connection;
        public DoctorRepository( IConfiguration configuration)
        {
            this.configuration = configuration;
            connectionString = configuration.GetConnectionString("HospitalDB");
            connection = new SqlConnection(connectionString);
        }
        

        /// <summary>
        /// Register doctor
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public DocRegModel Register(DocRegModel model)
        {
            try
            {
                using (this.connection)
                {
                    SqlCommand command = new SqlCommand("spRegisterDoctor", this.connection);
                    
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@DoctorName", model.DoctorName);
                    command.Parameters.AddWithValue("@DoctorGender", model.DoctorGender);
                    command.Parameters.AddWithValue("@DoctorSpecialization", model.DoctorSpecialization);
                    command.Parameters.AddWithValue("@DoctorNumber", model.DoctorNumber);
                    command.Parameters.AddWithValue("@DoctorEmail", model.DoctorEmail);
                    command.Parameters.AddWithValue("@DoctorPassword", EncryptPassword(model.DoctorPassword));
                    this.connection.Open();
                    var count = command.ExecuteNonQuery();
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
            catch(Exception ex)
            {
                throw ex;
            }
            finally
            {
                this.connection.Close();
            }
        }
        /// <summary>
        /// Encrypt password for security
        /// </summary>
        /// <param name="password"></param>
        /// <returns></returns>
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
        /// <summary>
        /// Login method for doctor
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public string LoginDoc(LoginModel model)
        {
            try
            {
                using (this.connection)
                {
                    SqlCommand command = new SqlCommand("spLoginDoctor", this.connection);
                    this.connection.Open();
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@DoctorEmail", model.Email);
                    command.Parameters.AddWithValue("@DoctorPassword", EncryptPassword(model.Password));
                    SqlDataReader reader = command.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            var doctorId = reader.GetInt32(0);
                            var token = GenerateToken(model.Email, doctorId);
                            return token;
                        }
                    }

                    return null;
                }
            }
            catch(Exception ex)
            {
                throw ex;
            }
            finally
            {
                this.connection.Close();
            }
        }

        //GenerateToken 
        /// <summary>
        /// JWT token for authorization
        /// </summary>
        /// <param name="docEmailId"></param>
        /// <param name="doctorId"></param>
        /// <returns></returns>
        private string GenerateToken(string docEmailId, long doctorId)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var claims = new[]
            {
                new Claim("Email",docEmailId),
                new Claim("UserId",doctorId.ToString())
            };
            var token = new JwtSecurityToken(
                issuer: configuration["Jwt:Issuer"],
                audience: configuration["Jwt:Audience"],
                claims,
                expires: DateTime.Now.AddHours(4),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        //Get all doctors
        public List<DocRegModel> GetAllDoctors()
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
                        List<DocRegModel> list = new List<DocRegModel>();
                        while (reader.Read())
                        {
                            DocRegModel model = new DocRegModel();
                            
                            model.DoctorName = reader.GetString(1);
                            model.DoctorGender=reader.GetString(2);
                            model.DoctorSpecialization=reader.GetString(3);
                            model.DoctorNumber=reader.GetInt32(4);
                            model.DoctorEmail=reader.GetString(5);
                            model.DoctorEmail=reader.GetString(6);
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
        /// <summary>
        /// Fetching appointments for doctor who is logged in
        /// </summary>
        /// <param name="doctorId"></param>
        /// <returns></returns>
        public List<AppointmentModel> GetMyAppointments(int doctorId)
        {
            try
            {
                using (this.connection)
                {
                    string query = @"select * from AppointmentsTable where DoctorId=@Id;";
                    SqlCommand command = new SqlCommand(query, this.connection);
                    command.Parameters.AddWithValue("@Id", doctorId);
                    this.connection.Open();
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
        /// <summary>
        /// method to forgot password
        /// </summary>
        /// <param name="docEmail"></param>
        /// <returns></returns>
        public string ForgotPassword(string docEmail)
        {
            using (this.connection)
            {
                string quary = @"select * from DoctorTable where DoctorEmail=@docEmail";
                SqlCommand cmd = new SqlCommand(quary, this.connection);
                cmd.Parameters.AddWithValue("@docEmail", docEmail);
                this.connection.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        int id = reader.GetInt32(0);
                        string name = reader.GetString(1);
                        string email = reader.GetString(5);
                        var token = GenerateToken(email, id);
                        //new MSMQ().SendMessage(token, email,name);
                        return token;
                    }
                }
                return "Email not found";
            }
        }

        public string ResetPassword(ResetPasswordModel model,string docEmail)
        {
            try
            {
                if (model.Password.Equals(model.ConfirmPassword))
                {
                    using (this.connection)
                    {
                        SqlCommand cmd = new SqlCommand("spDocResetPassword", this.connection);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Email", docEmail);
                        cmd.Parameters.AddWithValue("@ConfirmPassword", EncryptPassword(model.ConfirmPassword));
                        this.connection.Open();
                        var count = cmd.ExecuteNonQuery();
                        if (count != 0)
                        {
                            return "Password Reset Done";
                        }
                        return "Email not found";
                    }
                }
                return "Password and ConfirmPassword Not Matching";
            }
            catch (Exception e)
            {

                throw e;
            }
        }

        public List<PatientRegModel> GetMyPatients(int docId)
        {
            try
            {
                using (this.connection)
                {
                    SqlCommand sqlCommand=new SqlCommand("spGetMyPatients", connection);
                    sqlCommand.CommandType = CommandType.StoredProcedure;
                    sqlCommand.Parameters.AddWithValue("@docId", docId);
                    connection.Open();
                    SqlDataReader reader=sqlCommand.ExecuteReader();
                    if (reader.HasRows)
                    {
                        List<PatientRegModel> list = new List<PatientRegModel>();
                        while (reader.Read())
                        {
                            PatientRegModel model = new PatientRegModel();
                            model.PatientFirstName = reader.GetString(0);
                            model.PatientLastName = reader.GetString(1);
                            model.PatientEmail = reader.GetString(2);
                            model.PatientPassword = reader.GetString(3);
                            model.PatientGender = reader.GetString(4);
                            model.PatientAddress = reader.GetString(5);
                            model.PatientCity = reader.GetString(6);
                            model.PatientState = reader.GetString(7);
                            model.PatientDesies = reader.GetString(8);

                            list.Add(model);
                        }
                        return list;
                    }
                }

                return null;
            }
            catch(Exception e)
            {
                throw e;
            }
            finally
            {
                connection.Close();
            }
        }
    }
}
