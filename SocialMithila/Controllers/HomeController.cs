using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SocialMithila.Controllers
{
    public class HomeController : Controller
    {
        private readonly string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Contact()
        {
            
            return View();
        }

        [HttpGet]
        public JsonResult GetUserList(string userIds = null, DateTime? fromDate = null, DateTime? toDate = null, string status = null, int pageIndex = 1, int pageSize = 10)
        {
            List<object> users = new List<object>();
            int totalCount = 0;

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("USP_GetUserList", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@UserIds", (object)userIds ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@FromDate", (object)fromDate ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@ToDate", (object)toDate ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@Status", (object)status ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@PageIndex", pageIndex);
                    cmd.Parameters.AddWithValue("@PageSize", pageSize);

                    con.Open();
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            string photo = dr["ProfilePhoto"] == DBNull.Value || string.IsNullOrWhiteSpace(dr["ProfilePhoto"].ToString())
                                ? "https://cdn-icons-png.flaticon.com/512/149/149071.png"
                                : "https://socialmithila.com/" + dr["ProfilePhoto"].ToString();

                            users.Add(new
                            {
                                Id = dr["Id"],
                                FullName = $"{dr["FirstName"]} {dr["LastName"]}".Trim(),
                                Email = dr["Email"],
                                Phone = dr["Phone"],
                                Country = dr["Country"],
                                Address = dr["Address"],
                                TownCity = dr["TownCity"],
                                PostCode = dr["PostCode"],
                                Description = dr["Description"],
                                ProfilePhoto = photo,
                                IsActive = Convert.ToBoolean(dr["IsActive"]),
                                CreatedOn = Convert.ToDateTime(dr["CreatedOn"]).ToString("yyyy-MM-dd HH:mm")
                            });
                        }

                        if (dr.NextResult() && dr.Read())
                        {
                            totalCount = Convert.ToInt32(dr["TotalCount"]);
                        }
                    }
                }
            }

            return Json(new { Data = users, TotalCount = totalCount }, JsonRequestBehavior.AllowGet);
        }
    }
}