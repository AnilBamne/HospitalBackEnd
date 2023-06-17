using CommonLayer.Model;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Text;
using RepositoryLayer.Interface;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Configuration;

namespace RepositoryLayer.Service
{
    public class PatientRepository: IPatientRepository
    {
        private readonly IConfiguration configuration;
        public static string connectionString;
        SqlConnection connection;
        public PatientRepository(IConfiguration configuration)
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
        public PatientRegModel Register(PatientRegModel model)
        {
            try
            {
                using (this.connection)
                {
                    SqlCommand command = new SqlCommand("spRegisterPatient", this.connection);
                    this.connection.Open();
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@PFirstName", model.PatientFirstName);
                    command.Parameters.AddWithValue("@PLastName", model.PatientLastName);
                    command.Parameters.AddWithValue("@PEmail", model.PatientEmail);
                    command.Parameters.AddWithValue("@PPassword", EncryptPassword(model.PatientPassword));
                    command.Parameters.AddWithValue("@PGender", model.PatientGender);
                    command.Parameters.AddWithValue("@PAddress", model.PatientAddress);
                    command.Parameters.AddWithValue("@PCity", model.PatientCity);
                    command.Parameters.AddWithValue("@PState", model.PatientState);
                    command.Parameters.AddWithValue("@PDesies", model.PatientDesies);

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

        public string PatientLogin(LoginModel model)
        {
            try
            {
                using (this.connection)
                {
                    
                    SqlCommand command = new SqlCommand("spLoginPatient", this.connection);
                    this.connection.Open();
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@PatientEmail", model.Email);
                    command.Parameters.AddWithValue("@PatientPassword", EncryptPassword(model.Password));
                    
                    SqlDataReader reader = command.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            var patientId = reader.GetInt32(0);
                            var token = GenerateToken(model.Email, patientId);
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
        //GenerateToken 
        private string GenerateToken(string emailId, long userId)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var claims = new[]
            {
                new Claim("Email",emailId),
                new Claim("UserId",userId.ToString())
            };
            var token = new JwtSecurityToken(
                issuer: configuration["Jwt:Issuer"],
                audience: configuration["Jwt:Audience"],
                claims,
                expires: DateTime.Now.AddHours(4),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
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
        public List<DocModel> GetAllDoctors()
        {
            try
            {
                using (this.connection)
                {
                    SqlCommand command = new SqlCommand("spGetActiveDoctors", this.connection);
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
                            model.DoctorEmail = reader.GetString(6);
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


        public string ForgotPassword(string patEmail)
        {
            using (this.connection)
            {
                string quary = @"select * from PatientTable where PatientEmail=@patEmail";
                SqlCommand cmd = new SqlCommand(quary, this.connection);
                cmd.Parameters.AddWithValue("@patEmail", patEmail);
                this.connection.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        int id = reader.GetInt32(0);
                        string name = reader.GetString(1);
                        string email = reader.GetString(3);
                        var token = GenerateToken(email, id);
                        //new MSMQ().SendMessage(token, email,name);
                        return token;
                    }
                }
                return "Email not found";
            }
        }

        public string ResetPassword(ResetPasswordModel model, string patEmail)
        {
            if (model.Password.Equals(model.ConfirmPassword))
            {
                using (this.connection)
                {
                    SqlCommand cmd = new SqlCommand("spPatResetPassword", this.connection);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Email", patEmail);
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
    }
}
