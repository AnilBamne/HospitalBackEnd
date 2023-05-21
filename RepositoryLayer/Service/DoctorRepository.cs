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

        public List<PatientRegModel> GetMyPatients(int doctorId)
        {
            try
            {
                using (this.connection)
                {
                    SqlCommand command = new SqlCommand("spGetMyPatients", this.connection);
                    this.connection.Open();
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@DoctorId", doctorId);
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
                    if (count!=0)
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
