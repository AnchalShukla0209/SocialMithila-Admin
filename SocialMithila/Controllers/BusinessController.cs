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
    public class BusinessController : Controller
    {
        private readonly string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
        // GET: Business
        public ActionResult CategoryMgmt()
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
        public ActionResult SubCategoryMgmt()
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
        public ActionResult BusinessMgmt()
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
        public JsonResult GetCategoryForDropdown()
        {
            List<object> userList = new List<object>();

            using (SqlConnection con = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand("USP_CategoryDataForDD", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                con.Open();

                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        userList.Add(new
                        {
                            Id = Convert.ToInt32(dr["id"]),
                            Text = $"{dr["CategoryName"]}"
                        });
                    }
                }
            }

            return Json(userList, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetSubCategories(string categoryIds, string subCategoryName, string status, int pageIndex = 1, int pageSize = 10)
        {
            int? statusInt = null;
            if (!string.IsNullOrEmpty(status))
            {
                if (status.Equals("Active", StringComparison.OrdinalIgnoreCase)) statusInt = 1;
                else if (status.Equals("Inactive", StringComparison.OrdinalIgnoreCase)) statusInt = 0;
            }

            var dt = new DataTable();
            int total = 0;
            using (var cn = new SqlConnection(connectionString))
            using (var cmd = new SqlCommand("usp_SubCategory_CRUD", cn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Action", "SelectAll");
                cmd.Parameters.AddWithValue("@CategoryIds", (object)categoryIds ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@SubCategoryName", (object)subCategoryName ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@Status", (object)statusInt ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@PageIndex", pageIndex);
                cmd.Parameters.AddWithValue("@PageSize", pageSize);

                var outParam = new SqlParameter("@TotalCount", SqlDbType.Int) { Direction = ParameterDirection.Output };
                cmd.Parameters.Add(outParam);

                using (var da = new SqlDataAdapter(cmd))
                {
                    da.Fill(dt);
                }

                if (outParam.Value != DBNull.Value)
                    total = Convert.ToInt32(outParam.Value);
            }

            var rows = new List<object>();
            int sr = (pageIndex - 1) * pageSize + 1;
            foreach (DataRow r in dt.Rows)
            {
                rows.Add(new
                {
                    SubCategoryId = Convert.ToInt32(r["sub_category_id"]),
                    CategoryId = Convert.ToInt32(r["category_id"]),
                    CategoryName = r["category_name"]?.ToString(),
                    SubCategoryName = r["sub_category_name"]?.ToString(),
                    IsActive = Convert.ToBoolean(r["IsActive"]),
                    SrNo = sr++
                });
            }

            return Json(new { Data = rows, TotalCount = total }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetSubCategoryById(int id)
        {
            var result = new Dictionary<string, object>();
            using (var cn = new SqlConnection(connectionString))
            using (var cmd = new SqlCommand("usp_SubCategory_CRUD", cn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Action", "Select");
                cmd.Parameters.AddWithValue("@SubCategoryId", id);

                cn.Open();
                using (var rdr = cmd.ExecuteReader())
                {
                    if (rdr.Read())
                    {
                        result["SubCategoryId"] = rdr.GetInt32(rdr.GetOrdinal("sub_category_id"));
                        result["CategoryId"] = rdr.GetInt32(rdr.GetOrdinal("category_id"));
                        result["CategoryName"] = rdr["category_name"]?.ToString();
                        result["SubCategoryName"] = rdr["sub_category_name"]?.ToString();
                        result["IsActive"] = Convert.ToBoolean(rdr["IsActive"]);
                    }
                }
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        // POST: /SubCategory/Save (Insert or Update)
        [HttpPost]
        public JsonResult Save(int? subCategoryId, int categoryId, string subCategoryName, int? isActive)
        {
            try
            {
                using (var cn = new SqlConnection(connectionString))
                using (var cmd = new SqlCommand("usp_SubCategory_CRUD", cn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    if (subCategoryId == null || subCategoryId == 0)
                    {
                        // Insert
                        cmd.Parameters.AddWithValue("@Action", "Insert");
                    }
                    else
                    {
                        // Update
                        cmd.Parameters.AddWithValue("@Action", "Update");
                        cmd.Parameters.AddWithValue("@SubCategoryId", subCategoryId.Value);
                    }

                    cmd.Parameters.AddWithValue("@CategoryId", categoryId);
                    cmd.Parameters.AddWithValue("@SubCategoryName", subCategoryName);
                    if (isActive.HasValue) cmd.Parameters.AddWithValue("@IsActive", isActive.Value);
                    else cmd.Parameters.AddWithValue("@IsActive", DBNull.Value);

                    cn.Open();
                    using (var rdr = cmd.ExecuteReader())
                    {
                        // SP returns the inserted/updated row; read it to return to client if desired
                        if (rdr.Read())
                        {
                            var obj = new
                            {
                                SubCategoryId = rdr.GetInt32(rdr.GetOrdinal("sub_category_id")),
                                CategoryId = rdr.GetInt32(rdr.GetOrdinal("category_id")),
                                CategoryName = rdr["category_name"]?.ToString(),
                                SubCategoryName = rdr["sub_category_name"]?.ToString(),
                                IsActive = Convert.ToBoolean(rdr["IsActive"])
                            };
                            return Json(new { Success = true, Data = obj });
                        }
                        else
                        {
                            return Json(new { Success = true, Message = "Saved successfully." });
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                // Catch duplicate message from SP (we used RAISERROR there)
                return Json(new { Success = false, Message = ex.Message });
            }
            catch (Exception ex)
            {
                return Json(new { Success = false, Message = ex.Message });
            }
        }

        // POST: /SubCategory/Delete
        [HttpPost]
        public JsonResult Delete(int id)
        {
            try
            {
                using (var cn = new SqlConnection(connectionString))
                using (var cmd = new SqlCommand("usp_SubCategory_CRUD", cn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Action", "Delete");
                    cmd.Parameters.AddWithValue("@SubCategoryId", id);
                    cn.Open();
                    var rows = cmd.ExecuteScalar(); // returns @@ROWCOUNT as single value select
                    return Json(new { Success = true, Rows = rows });
                }
            }
            catch (Exception ex)
            {
                return Json(new { Success = false, Message = ex.Message });
            }
        }

        [HttpGet]
        public JsonResult GetSubCategoriesByCategories(string categoryIds)
        {
            var list = new List<object>();
            if (string.IsNullOrWhiteSpace(categoryIds))
                return Json(list, JsonRequestBehavior.AllowGet);

            using (var cn = new SqlConnection(connectionString))
            using (var cmd = new SqlCommand())
            {
                cmd.Connection = cn;
                cmd.CommandText = @"
                    SELECT DISTINCT sc.sub_category_id, sc.sub_category_name
                    FROM [SocialMithila_db].sub_categories sc
                    WHERE sc.category_id IN (SELECT TRY_CAST(value AS INT) FROM STRING_SPLIT(@ids,','))";
                cmd.Parameters.AddWithValue("@ids", categoryIds);
                cn.Open();
                using (var rdr = cmd.ExecuteReader())
                {
                    while (rdr.Read())
                    {
                        list.Add(new { Id = rdr.GetInt32(0), Text = rdr.GetString(1) });
                    }
                }
            }
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetPaymentMethods()
        {
            var list = new List<object>();
            using (var cn = new SqlConnection(connectionString))
            using (var cmd = new SqlCommand("SELECT payment_id Id, payment_type Text FROM [SocialMithila_db].payment_methods ORDER BY payment_type", cn))
            {
                cn.Open();
                using (var rdr = cmd.ExecuteReader())
                {
                    while (rdr.Read())
                        list.Add(new { Id = rdr.GetInt32(0), Text = rdr.GetString(1) });
                }
            }
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetBusinesses(string categoryIds, string subCategoryIds, string businessName, string fromDate, string toDate, string status, int pageIndex = 1, int pageSize = 10)
        {
            int? statusInt = null;
            if (!string.IsNullOrWhiteSpace(status))
            {
                if (status.Equals("Active", StringComparison.OrdinalIgnoreCase)) statusInt = 1;
                else if (status.Equals("Inactive", StringComparison.OrdinalIgnoreCase)) statusInt = 0;
            }

            var dt = new DataTable();
            int total = 0;
            using (var cn = new SqlConnection(connectionString))
            using (var cmd = new SqlCommand("usp_Business_CRUD", cn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Action", "SelectAll");
                cmd.Parameters.AddWithValue("@CategoryIds", (object)categoryIds ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@SubCategoryIds", (object)subCategoryIds ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@BusinessName", (object)businessName ?? DBNull.Value);

                DateTime? fd = ParseDateTimeOrNull(fromDate);
                DateTime? td = ParseDateTimeOrNull(toDate);
                cmd.Parameters.AddWithValue("@FromDate", (object)fd ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@ToDate", (object)td ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@Status", (object)statusInt ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@PageIndex", pageIndex);
                cmd.Parameters.AddWithValue("@PageSize", pageSize);

                var outParam = new SqlParameter("@TotalCount", SqlDbType.Int) { Direction = ParameterDirection.Output };
                cmd.Parameters.Add(outParam);

                using (var da = new SqlDataAdapter(cmd))
                {
                    da.Fill(dt);
                }

                if (outParam.Value != DBNull.Value)
                    total = Convert.ToInt32(outParam.Value);
            }

            // Build result
            var rows = new List<object>();
            int sr = (pageIndex - 1) * pageSize + 1;
            foreach (DataRow r in dt.Rows)
            {
                rows.Add(new
                {
                    BusinessId = Convert.ToInt32(r["business_id"]),
                    BusinessName = r["business_name"]?.ToString(),
                    CategoryId = r["category_id"] == DBNull.Value ? 0 : Convert.ToInt32(r["category_id"]),
                    CategoryName = r["category_name"]?.ToString(),
                    SubCategoryId = r["sub_category_id"] == DBNull.Value ? 0 : Convert.ToInt32(r["sub_category_id"]),
                    SubCategoryName = r["sub_category_name"]?.ToString(),
                    ContactNumber = r["contact_number"]?.ToString(),
                    Address = r["address"]?.ToString(),
                    ImageUrl = r["image_url"]?.ToString(),
                    Rating = r["rating"] == DBNull.Value ? (decimal?)null : Convert.ToDecimal(r["rating"]),
                    DistanceKm = r["distance_km"] == DBNull.Value ? (decimal?)null : Convert.ToDecimal(r["distance_km"]),
                    PaymentType = r["payment_type"]?.ToString(),
                    CreatedAt = r["created_at"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(r["created_at"]),
                    IsActive = r["IsActive"] == DBNull.Value ? false : Convert.ToBoolean(r["IsActive"]),
                    SrNo = sr++
                });
            }

            return Json(new { Data = rows, TotalCount = total }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetBusinessById(int id)
        {
            var dict = new Dictionary<string, object>();
            using (var cn = new SqlConnection(connectionString))
            using (var cmd = new SqlCommand("usp_Business_CRUD", cn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Action", "Select");
                cmd.Parameters.AddWithValue("@BusinessId", id);
                cn.Open();
                using (var rdr = cmd.ExecuteReader())
                {
                    if (rdr.Read())
                    {
                        dict["BusinessId"] = rdr.GetInt32(rdr.GetOrdinal("business_id"));
                        dict["BusinessName"] = rdr["business_name"]?.ToString();
                        dict["CategoryId"] = rdr["category_id"] == DBNull.Value ? 0 : rdr.GetInt32(rdr.GetOrdinal("category_id"));
                        dict["SubCategoryId"] = rdr["sub_category_id"] == DBNull.Value ? 0 : rdr.GetInt32(rdr.GetOrdinal("sub_category_id"));
                        dict["ContactNumber"] = rdr["contact_number"]?.ToString();
                        dict["Address"] = rdr["address"]?.ToString();
                        dict["Latitude"] = rdr["latitude"] == DBNull.Value ? (decimal?)null : Convert.ToDecimal(rdr["latitude"]);
                        dict["Longitude"] = rdr["longitude"] == DBNull.Value ? (decimal?)null : Convert.ToDecimal(rdr["longitude"]);
                        dict["Rating"] = rdr["rating"] == DBNull.Value ? (decimal?)null : Convert.ToDecimal(rdr["rating"]);
                        dict["DistanceKm"] = rdr["distance_km"] == DBNull.Value ? (decimal?)null : Convert.ToDecimal(rdr["distance_km"]);
                        dict["ImageUrl"] = rdr["image_url"]?.ToString();
                        dict["PaymentId"] = rdr["payment_id"] == DBNull.Value ? (int?)null : Convert.ToInt32(rdr["payment_id"]);
                        dict["Description"] = rdr["Descreption"]?.ToString();
                        dict["UserId"] = rdr["userid"] == DBNull.Value ? (int?)null : Convert.ToInt32(rdr["userid"]);
                        dict["IsActive"] = rdr["IsActive"] == DBNull.Value ? false : Convert.ToBoolean(rdr["IsActive"]);
                    }
                }
            }

            // load amenities and images
            var amenities = new List<object>();
            var images = new List<object>();

            using (var cn = new SqlConnection(connectionString))
            using (var cmd = new SqlCommand("SELECT AminitiesText FROM [SocialMithila_db].TblBusinessAminities WHERE BusinessId = @bid AND isDeleted = 0", cn))
            {
                cmd.Parameters.AddWithValue("@bid", id);
                cn.Open();
                using (var rdr = cmd.ExecuteReader())
                {
                    while (rdr.Read())
                    {
                        amenities.Add(rdr["AminitiesText"]?.ToString());
                    }
                }
            }

            using (var cn = new SqlConnection(connectionString))
            using (var cmd = new SqlCommand("SELECT image_id, image_url FROM [SocialMithila_db].business_images WHERE business_id = @bid", cn))
            {
                cmd.Parameters.AddWithValue("@bid", id);
                cn.Open();
                using (var rdr = cmd.ExecuteReader())
                {
                    while (rdr.Read())
                    {
                        images.Add(new { Id = rdr.GetInt32(0), Url = rdr["image_url"]?.ToString() });
                    }
                }
            }

            return Json(new { Business = dict, Amenities = amenities, Images = images }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult SaveBusiness(int? businessId, string businessName, int categoryId, int subCategoryId,
            string contactNumber, string address, decimal? latitude, decimal? longitude,
            decimal? rating, decimal? distanceKm, string imageUrlsCsv, int? paymentId, string description,
            int? userId, bool? isActive, string amenitiesPipe)   // amenities pipe separated
        {
            try
            {
                // server side validations
                if (string.IsNullOrWhiteSpace(businessName))
                    return Json(new { Success = false, Message = "Business name required." });

                if (categoryId <= 0 || subCategoryId <= 0)
                    return Json(new { Success = false, Message = "Category and Sub-Category required." });

                // amenities count check
                var amenitiesCount = 0;
                if (!string.IsNullOrWhiteSpace(amenitiesPipe))
                {
                    amenitiesCount = amenitiesPipe.Split(new[] { '|' }, StringSplitOptions.RemoveEmptyEntries).Length;
                    if (amenitiesCount > 20) return Json(new { Success = false, Message = "Max 20 amenities allowed." });
                }

                // image count check
                var imagesCount = 0;
                if (!string.IsNullOrWhiteSpace(imageUrlsCsv))
                {
                    imagesCount = imageUrlsCsv.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Length;
                    if (imagesCount > 5) return Json(new { Success = false, Message = "Max 5 images allowed." });
                }

                using (var cn = new SqlConnection(connectionString))
                using (var cmd = new SqlCommand("usp_Business_CRUD", cn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    if (businessId == null || businessId == 0)
                        cmd.Parameters.AddWithValue("@Action", "Insert");
                    else
                    {
                        cmd.Parameters.AddWithValue("@Action", "Update");
                        cmd.Parameters.AddWithValue("@BusinessId", businessId.Value);
                    }

                    cmd.Parameters.AddWithValue("@BusinessNameIn", businessName);
                    cmd.Parameters.AddWithValue("@CategoryId", categoryId);
                    cmd.Parameters.AddWithValue("@SubCategoryId", subCategoryId);
                    cmd.Parameters.AddWithValue("@ContactNumber", (object)contactNumber ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@Address", (object)address ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@Latitude", (object)latitude ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@Longitude", (object)longitude ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@Rating", (object)rating ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@DistanceKm", (object)distanceKm ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@ImageUrls", (object)imageUrlsCsv ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@PaymentId", (object)paymentId ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@Description", (object)description ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@UserId", (object)userId ?? DBNull.Value);
                    if (isActive.HasValue) cmd.Parameters.AddWithValue("@IsActiveIn", isActive.Value);
                    else cmd.Parameters.AddWithValue("@IsActiveIn", DBNull.Value);

                    cmd.Parameters.AddWithValue("@Amenities", (object)amenitiesPipe ?? DBNull.Value);

                    cn.Open();
                    using (var rdr = cmd.ExecuteReader())
                    {
                        if (rdr.Read())
                        {
                            // return inserted/updated business id and name
                            var res = new { BusinessId = Convert.ToInt32(rdr["business_id"]), BusinessName = rdr["business_name"].ToString() };
                            return Json(new { Success = true, Data = res });
                        }
                        else
                        {
                            return Json(new { Success = true, Message = "Saved." });
                        }
                    }
                }
            }
            catch (SqlException sqx)
            {
                return Json(new { Success = false, Message = sqx.Message });
            }
            catch (Exception ex)
            {
                return Json(new { Success = false, Message = ex.Message });
            }
        }

        private DateTime? ParseDateTimeOrNull(string s)
        {
            if (string.IsNullOrWhiteSpace(s)) return null;
            if (DateTime.TryParse(s, out var dt)) return dt;
            return null;
        }
    }
}