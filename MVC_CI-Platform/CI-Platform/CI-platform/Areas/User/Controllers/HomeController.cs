using CI.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using CI.DataAcess;
using CI.DataAcess.Repository.IRepository;
using CI.Models.ViewModels;
using CI.Models.Models;
using Microsoft.Data.SqlClient;
using System.Data;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CI_platform.Controllers
{
    [Area("User")]
    public class HomeController : Controller
    {
        private readonly IAllRepository allRepository;

        public object TrustServerCertificate { get; private set; }

        public HomeController(IAllRepository _allRepository)
        {
            allRepository = _allRepository;
    }
        [Route("Home")]
        public IActionResult home()
        {
            List<CI.Models.ViewModels.Mission>  missions = allRepository.Mission.GetAllMission();
            return View(missions);
        }
        [HttpPost]
        [Route("Home")]
        public  JsonResult home(List<string> countries, List<string> cities, List<string> themes, List<string> skills,string key,string sort_by)
        {
            if (key is not null)
            {
                List<CI.Models.ViewModels.Mission> search_missions = allRepository.Mission.GetSearchMissions(key);
                return Json(new { missions=search_missions, success = true });
            }
            else
            {
                List<CI.Models.ViewModels.Mission> missions = allRepository.Mission.GetFilteredMissions(countries, cities, themes, skills,sort_by);
                return Json(new { missions, success = true });
            }
        
        }
        [Route("volunteering_mission")]
        public IActionResult volunteering_mission()
        {
            return View();
        }
        public IActionResult Privacy()
        {
            return View();
        }

    

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult filter(string sortoption= "sortoption")
        {
            List<CI.Models.Models.Mission> missions = new List<CI.Models.Models.Mission>();
            using (var connection = new SqlConnection("SqlConnectionString"))
            {
                connection.Open();
                var command = new SqlCommand("_SpRetrieveMission", connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@pg", 1);
                command.Parameters.AddWithValue("@PageSize", 9);
                command.Parameters.AddWithValue("@SearchString", 'A');
                command.Parameters.AddWithValue("@SortOption", sortoption);
                command.Parameters.AddWithValue("@TotalRecord", 99);
            }
            return View(missions);
            }

            // Create a SqlConnection object and set the connection string to your database
            //    using (SqlConnection conn = new SqlConnection("Server=PCA107\SQL2017;Database=CI-Platform;Trusted_Connection=true; Encrypt=false;TrustServerCertificate=false;"))
            //    {
            //        // Create a SqlCommand object and set its properties
            //        SqlCommand cmd = new SqlCommand("_SpRetrieveMission", conn);
            //        cmd.CommandType = CommandType.StoredProcedure;

            //        // Open the database connection
            //        conn.Open();
            //        cmd.Parameters.AddWithValue("@pg", pg);
            //        cmd.Parameters.AddWithValue("@pagesize", pageSize);
            //        cmd.Parameters.AddWithValue("@CountryId", countryid);
            //        cmd.Parameters.AddWithValue("@CityId", cityId);
            //        cmd.Parameters.AddWithValue("@SearchString", string.IsNullOrEmpty(searchString) ? DBNull.Value : (object)searchString);
            //        cmd.Parameters.AddWithValue("@SortOption", sortOption);
            //        SqlParameter totalRecordsParameter = new SqlParameter("@TotalRecords", SqlDbType.Int);
            //        totalRecordsParameter.Direction = ParameterDirection.Output;
            //        cmd.Parameters.Add(totalRecordsParameter);

            //        // Execute the stored procedure and read the results into a SqlDataReader object
            //        SqlDataReader reader = cmd.ExecuteReader();

            //        // Loop through the SqlDataReader object and add each row to the missions list
            //        while (reader.Read())
            //        {
            //            CI.Models.Models.Mission mission = new CI.Models.Models.Mission();

            //            mission.Title = reader["Title"].ToString();
            //            mission.ShortDescription = reader["ShortDescription"].ToString();
            //            mission.CityName = reader["CityName"].ToString();
            //            mission.Theme = reader["Theme"].ToString();
            //            mission.OrganizationName = reader["OrganizationName"].ToString();
            //            mission.CountryId = (Int64)reader["CountryId"];
            //            mission.MissionType = reader["MissionType"].ToString();
            //            mission.StartDate = reader["StartDate"].ToString();
            //            mission.EndDate = reader["EndDate"].ToString();
            //            mission.GoalObjectiveText = reader["GoalObjectiveText"].ToString();
            //            mission.EndDate= reader["Deadline"].ToString();
            //            mission.CityId = (Int64)reader["CityId"];
            //            mission.CreatedAt = (DateTime)reader["CreatedAt"];
            //            mission.Country = reader["Country"].ToString();

            //            missions.Add(mission);
            //        }

            //        // Close the SqlDataReader and the database connection
            //        reader.Close();
            //        conn.Close();
            //        int totalRecords = (int)totalRecordsParameter.Value;
            //        var paging = new Pager(totalRecords, pg, pageSize);
            //        this.ViewBag.Pager = paging;
            //    }
            //    return View();
            //}

            //public IActionResult Pagination(int page = 1, int pageSize = 10)
            //{
            //    var items = _dbContext.Items.Skip((page - 1) * pageSize).Take(pageSize).ToList();
            //    var totalItems = _dbContext.Items.Count();
            //    var viewModel = new Mission
            //    {
            //        CurrentPage = page,
            //        PageSize = pageSize,
            //        TotalItems = totalItems,
            //        Items = items
            //    };
            //    return View(viewModel);
            //}




        }
    }