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
    public class BusinessController : Controller
    {
        private readonly string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

        // GET: Business
        public ActionResult CategoryMgmt()
        {
            return View();
        }

        [HttpPost]
        public JsonResult SaveCategory(int categoryId, string categoryName, bool isActive)
        {
            string action = categoryId == 0 ? "Insert" : "Update";
            string message = "";
            int status = 0;

            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("USP_ManageCategories", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("@Action", action);
                        cmd.Parameters.AddWithValue("@CategoryId", categoryId);
                        cmd.Parameters.AddWithValue("@CategoryName", (object)categoryName ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@IsActive", isActive);

                        con.Open();

                        using (SqlDataReader dr = cmd.ExecuteReader())
                        {
                            if (dr.Read())
                            {
                                status = 1;
                                message = action == "Insert" ? "Category added successfully!" : "Category updated successfully!";
                            }
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                status = 0;
                message = ex.Message;  
            }

            return Json(new { Status = status, Message = message }, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public JsonResult GetCategories(string search = "", int? status = null, int pageIndex = 1, int pageSize = 10)
        {
            List<object> list = new List<object>();
            int totalCount = 0;

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("USP_ManageCategories", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@Action", "SelectAll");
                    cmd.Parameters.AddWithValue("@CategoryName", search);
                    cmd.Parameters.AddWithValue("@IsActive", (object)status ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@PageIndex", pageIndex);
                    cmd.Parameters.AddWithValue("@PageSize", pageSize);

                    SqlParameter totalParam = new SqlParameter("@TotalCount", SqlDbType.Int);
                    totalParam.Direction = ParameterDirection.Output;
                    cmd.Parameters.Add(totalParam);

                    con.Open();

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            list.Add(new
                            {
                                CategoryId = dr["category_id"],
                                CategoryName = dr["category_name"],
                                IsActive = dr["IsActive"]
                            });
                        }
                    }

                    totalCount = Convert.ToInt32(cmd.Parameters["@TotalCount"].Value);
                }
            }

            return Json(new { Data = list, TotalCount = totalCount }, JsonRequestBehavior.AllowGet);
        }



        [HttpGet]
        public JsonResult GetCategoryById(int categoryId)
        {
            object category = null;

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("USP_ManageCategories", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@Action", "Select");
                    cmd.Parameters.AddWithValue("@CategoryId", categoryId);

                    con.Open();

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        if (dr.Read())
                        {
                            category = new
                            {
                                CategoryId = Convert.ToInt32(dr["category_id"]),
                                CategoryName = dr["category_name"].ToString(),
                                IsActive = Convert.ToBoolean(dr["IsActive"])
                            };
                        }
                    }
                }
            }

            return Json(category, JsonRequestBehavior.AllowGet);
        }



    }
}