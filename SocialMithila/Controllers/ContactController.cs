using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Configuration;

namespace SocialMithila.Controllers
{
    public class ContactController : Controller
    {
        private readonly string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

        // GET: Contact
        public ActionResult ContactDataList()
        {
            return View();
        }

        [HttpGet]
        public JsonResult GetContactList(string userIds = null, int pageIndex = 1, int pageSize = 10)
        {
            List<object> contacts = new List<object>();
            int totalCount = 0;

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("USP_GetContactList", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@UserIds", (object)userIds ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@PageIndex", pageIndex);
                    cmd.Parameters.AddWithValue("@PageSize", pageSize);

                    con.Open();

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            string firstName = dr["FirstName"]?.ToString() ?? "";
                            string lastName = dr["LastName"]?.ToString() ?? "";
                            string contactBy = (firstName + " " + lastName).Trim();

                            contacts.Add(new
                            {
                                ContactId = dr["ContactId"],
                                UserName = dr["ContactName"], 
                                ContactEmail = dr["ContactEmail"],
                                ContactMessage = dr["ContactMessage"],
                                ContactMobileNumber = dr["ContactMobileNumber"],
                                ContactUserId = dr["ContactUserId"],
                                ContactBy = string.IsNullOrEmpty(contactBy) ? "N/A" : contactBy
                            });
                        }

                        if (dr.NextResult() && dr.Read())
                        {
                            totalCount = Convert.ToInt32(dr["TotalCount"]);
                        }
                    }
                }
            }

            return Json(new { Data = contacts, TotalCount = totalCount }, JsonRequestBehavior.AllowGet);
        }



        [HttpPost]
        public JsonResult DeleteContact(int id)
        {
            try
            {
                string constr = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

                using (SqlConnection con = new SqlConnection(constr))
                using (SqlCommand cmd = new SqlCommand("USP_DeleteContact", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ContactId", id);
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                }

                return Json(new { success = true, message = "Contact deleted successfully." });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

       

    }
}