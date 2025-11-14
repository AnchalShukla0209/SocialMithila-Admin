using SocialMithila.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
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
            if (Session["AdminId"] != null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Login", "Home");
            }
        }

        public ActionResult Contact()
        {
            if (Session["AdminId"] != null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Login", "Home");
            }
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
                    cmd.Parameters.AddWithValue("@UserIds", (object)userIds ?? null);
                    cmd.Parameters.AddWithValue("@FromDate", (object)fromDate ?? null);
                    cmd.Parameters.AddWithValue("@ToDate", (object)toDate ?? null);
                    cmd.Parameters.AddWithValue("@Status", (object)status ?? null);
                    cmd.Parameters.AddWithValue("@PageIndex", pageIndex);
                    cmd.Parameters.AddWithValue("@PageSize", pageSize);

                    con.Open();
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            string photo = dr["ProfilePhoto"] == DBNull.Value || string.IsNullOrWhiteSpace(dr["ProfilePhoto"].ToString())
                                ? "https://cdn-icons-png.flaticon.com/512/149/149071.png"
                                : "https://socialmithila.com" + dr["ProfilePhoto"].ToString();

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

        [HttpGet]
        public JsonResult GetUsersForDropdown()
        {
            List<object> userList = new List<object>();

            using (SqlConnection con = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand("USP_UserDataForDD", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                con.Open();

                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        string profilePhoto = Convert.ToString(dr["ProfilePhoto"]);
                        string basePhoto = string.IsNullOrWhiteSpace(profilePhoto)
                            ? "https://cdn-icons-png.flaticon.com/512/149/149071.png"
                            : "https://socialmithila.com/" + profilePhoto.TrimStart('/');

                        userList.Add(new
                        {
                            Id = Convert.ToInt32(dr["Id"]),
                            Text = $"{dr["FirstName"]} {dr["LastName"]} ({dr["Email"]})",
                            ProfilePhoto = basePhoto
                        });
                    }
                }
            }

            return Json(userList, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetDashboardSummary()
        {
            var result = new
            {
                success = false
            };

            var totals = new DashboardDto();
            var months = new List<MonthlySeriesDto>();
            using (var conn = new SqlConnection(connectionString))
            using (var cmd = new SqlCommand("sp_GetDashboardSummary", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                conn.Open();

                using (var reader = cmd.ExecuteReader())
                {
                    // First result set -> totals (single row)
                    if (reader.Read())
                    {
                        totals.TotalUsers = reader["TotalUsers"] != DBNull.Value ? Convert.ToInt32(reader["TotalUsers"]) : 0;
                        totals.TotalCategories = reader["TotalCategories"] != DBNull.Value ? Convert.ToInt32(reader["TotalCategories"]) : 0;
                        totals.TotalSubCategories = reader["TotalSubCategories"] != DBNull.Value ? Convert.ToInt32(reader["TotalSubCategories"]) : 0;
                        totals.TotalBusinesses = reader["TotalBusinesses"] != DBNull.Value ? Convert.ToInt32(reader["TotalBusinesses"]) : 0;
                        totals.TotalStories = reader["TotalStories"] != DBNull.Value ? Convert.ToInt32(reader["TotalStories"]) : 0;
                        totals.TotalPosts = reader["TotalPosts"] != DBNull.Value ? Convert.ToInt32(reader["TotalPosts"]) : 0;
                    }

                    // Move to next result set -> monthly rows
                    if (reader.NextResult())
                    {
                        while (reader.Read())
                        {
                            months.Add(new MonthlySeriesDto
                            {
                                MonthLabel = reader["MonthLabel"] != DBNull.Value ? reader["MonthLabel"].ToString() : "",
                                UsersCount = reader["UsersCount"] != DBNull.Value ? Convert.ToInt32(reader["UsersCount"]) : 0,
                                BusinessesCount = reader["BusinessesCount"] != DBNull.Value ? Convert.ToInt32(reader["BusinessesCount"]) : 0,
                                PostsCount = reader["PostsCount"] != DBNull.Value ? Convert.ToInt32(reader["PostsCount"]) : 0
                            });
                        }
                    }
                }
            }

            return Json(new
            {
                success = true,
                totals,
                months
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public JsonResult AdminLogin(string username, string password)
        {
            try
            {
                using (SqlConnection cnn = new SqlConnection(connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("SP_AdminLogin", cnn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@UserName", username);
                        cmd.Parameters.AddWithValue("@Password", password);

                        cnn.Open();
                        SqlDataReader dr = cmd.ExecuteReader();

                        if (dr.Read())
                        {
                            Session["AdminId"] = dr["Id"];
                            Session["AdminUserName"] = dr["UserName"];

                            return Json(new { success = true, message = "Login successful" });
                        }
                        else
                        {
                            return Json(new { success = false, message = "Invalid credentials" });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        public ActionResult Logout()
        {
            Session.Clear();
            Session.Abandon();
            Response.Cookies.Clear();
            return RedirectToAction("Login", "Home");
        }

    }


}