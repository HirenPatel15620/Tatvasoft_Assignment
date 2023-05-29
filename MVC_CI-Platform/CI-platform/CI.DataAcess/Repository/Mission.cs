using CI.Repository.Repository.IRepository;
using CI.Models;
using System.Net;
using System.Net.Mail;


namespace CI.Repository.Repository
{
    public class Mission : Repository<CI.Models.Mission>, IMission
    {
        private readonly CiPlatformContext _db;
        List<Country> countries = new List<Country>();
        List<City> cities = new List<City>();
        List<Comment> comments = new List<Comment>();
        List<Timesheet> timesheets = new List<Timesheet>();
        List<GoalMission> goalMissions = new List<GoalMission>();
        List<FavoriteMission> favoriteMissions = new List<FavoriteMission>();
        List<MissionApplication> missionApplications = new List<MissionApplication>();
        List<MissionDocument> mission_documents = new List<MissionDocument>();
        List<MissionInvite> already_recommended_users = new List<MissionInvite>();
        List<CI.Models.Mission> missions = new List<Models.Mission>();
        List<CI.Models.MissionMedia> image = new List<Models.MissionMedia>();
        List<MissionRating> ratings = new List<MissionRating>();
        List<MissionSkill> missionskills = new List<MissionSkill>();
        List<MissionTheme> theme = new List<MissionTheme>();
        List<Skill> skills = new List<Skill>();
        List<User> users = new List<User>();
        List<MissionDocument> document = new List<MissionDocument>();


        public Mission(CiPlatformContext db) : base(db)
        {
            _db = db;
            getAllDetails();
        }

        //home page

        //get data as per need
        public void getAllDetails()
        {
            cities = _db.Cities.ToList();
            countries = _db.Countries.ToList();
            comments = _db.Comments.ToList();
            favoriteMissions = _db.FavoriteMissions.ToList();
            missions = _db.Missions.Where(x=>x.Status == true).ToList();
            missionApplications = _db.MissionApplications.ToList();
            mission_documents = _db.MissionDocuments.ToList();
            already_recommended_users = _db.MissionInvites.ToList();
            image = _db.MissionMedia.ToList();
            missionskills = _db.MissionSkills.Where(x => x.SkillId != null).ToList();
            theme = _db.MissionThemes.Where(x => x.Status == 1).ToList();
            ratings = _db.MissionRatings.ToList();
            skills = _db.Skills.Where(x => x.Status == 1).ToList();
            users = _db.Users.ToList();
            timesheets = _db.Timesheets.ToList();
            goalMissions = _db.GoalMissions.ToList();
            document = _db.MissionDocuments.ToList();




        }

        //get first 9 mission in landing page
        public Models.ViewModels.Mission GetAllMission()
        {


            int total_missions = missions.Where(x => x.Status is true).Count();
            missions = missions.ToList();

            List<User> all_volunteers = new List<User>();
            List<User> already_recommended = new List<User>();



            var Missions = new CI.Models.ViewModels.Mission { missionApplications = missionApplications,goal = goalMissions, timesheet = timesheets, Missions = missions, Country = countries, themes = theme, skills = skills, total_missions = total_missions, All_volunteers = all_volunteers };
            return Missions;
        }


        public CI.Models.ViewModels.Mission GetMissionsByCityId(long id)
        {
            int total_missions = missions.Where(x => x.Status is true).Count();
            missions = missions.Where(x => x.CityId == id && x.Status is true).ToList();
            List<User> all_volunteers = new List<User>();

            var Missions = new CI.Models.ViewModels.Mission { missionApplications = missionApplications, goal = goalMissions, timesheet = timesheets, Missions = missions, Country = countries, themes = theme, skills = skills, total_missions = total_missions, All_volunteers = all_volunteers };
            return Missions;

        }

        //get filter missions
        public Models.ViewModels.Mission GetFilteredMissions(List<string> Countries, List<string> Cities, List<string> Themes, List<string> Skills, string sort_by, long user_id,string explore)
        {
           CI.Models.ViewModels.Mission Missions = new Models.ViewModels.Mission();
            List<City> city = new List<City>();
            List<User> all_volunteers = new List<User>();
            List<User> already_recommended = new List<User>();
            List<CI.Models.Mission> mission = new List<CI.Models.Mission>();


            missions = missions.Where(x => x.Status is true).ToList();



            //get cities as per country
            if (Countries.Count > 0)
            {
                city = (from c in cities
                        where Countries.Contains(c.Country.Name)
                        select c).ToList();
            }
            else
            {
                city = cities;
            }



           







            //filter as per city,theme and skill, if country selected it doesn't matter because filter work as per city first
            if (Cities.Count > 0)
            {

                mission = (from m in missions
                           where Cities.Contains(m.City.Name) || Themes.Contains(m.Theme?.Title) || Skills.Contains(m.Theme?.Title)
                           select m).ToList();
                //var skill_missions = (from s in missionskills
                //                      where Skills.Contains(s.Skill?.SkillName) && missions.Contains(s.Mission)
                //                      select s.Mission).ToList();
                //foreach (var skill_mission in skill_missions)
                //{
                //    if (!mission.Contains(skill_mission))
                //    {
                //        mission.Add(skill_mission);
                //    }
                //}
            }

            //filter as per country,theme and skills if city not selected
            else if (Countries.Count > 0 || Themes.Count > 0 || Skills.Count > 0)
            {
                mission = (from m in missions
                           where Countries.Contains(m.Country.Name) || Cities.Contains(m.City.Name) || Themes.Contains(m.Theme?.Title)
                           select m).ToList();
                var skill_missions = (from s in missionskills
                                      where Skills.Contains(s.Skill?.SkillName) && missions.Contains(s.Mission)
                                      select s.Mission).ToList();
                foreach (var skill_mission in skill_missions)
                {
                    if (!mission.Contains(skill_mission))
                    {
                        mission.Add(skill_mission);
                    }
                }
            }
            else
            {
                mission = missions;
            }

            //sort all mission as per user need
            if (sort_by == "newest")
            {
                Missions = new Models.ViewModels.Mission
                {
                    Missions = (from m in mission
                                orderby m.CreatedAt descending
                                select m).ToList(),
                    Country = countries,
                    Cities = city
                };
            }
            
            else if (sort_by == "lowest available seats")
            {
                Missions = new Models.ViewModels.Mission
                {
                    Missions = (from m in mission
                                orderby m.TotalSeats ascending
                                select m).ToList(),
                    Country = countries,
                    Cities = city
                };
            }
            else if (sort_by == "highest available seats")
            {
                Missions = new Models.ViewModels.Mission
                {
                    Missions = (from m in mission
                                orderby m.TotalSeats descending
                                select m).ToList(),
                    Country = countries,
                    Cities = city
                };
            }
            else if (sort_by == "registration deadline")
            {
                Missions = new Models.ViewModels.Mission
                {
                    Missions = (from m in mission
                                orderby m.Deadline ascending
                                select m).ToList(),
                    Country = countries,
                    Cities = city
                };
            }
            else if (sort_by == "my favourites")
            {
                Missions = new Models.ViewModels.Mission
                {
                    Missions = (from m in favoriteMissions
                                where m.UserId == user_id && mission.Contains(m.Mission)
                                select m.Mission).ToList(),
                    Country = countries,
                    Cities = city
                };
            }


            //good to have

            else if (sort_by == "theme")
            {
                var topThemes = (from m in mission
                                 group m by m.ThemeId into g
                                 orderby g.Count() descending
                                 select g.Key).Take(1).ToList(); // Get top 3 themes

                var topThemeMissions = (from m in mission
                                        where topThemes.Contains(m.ThemeId)
                                        orderby m.ThemeId descending
                                        select m).ToList(); // Get missions in top themes

                var mostCommonMissions = (from m in topThemeMissions
                                          group m by m.Title into g
                                          orderby g.Count() descending
                                          select g.Key).Take(10).ToList(); // Get top 10 most common missions

                Missions = new Models.ViewModels.Mission
                {
                    Missions = (from m in topThemeMissions
                                where mostCommonMissions.Contains(m.Title)
                                select m).ToList(),
                    Country = countries,
                    Cities = city
                };
            }
            else if (sort_by == "ranked")
            {
                var mostRankedMissions = (from m in mission
                                          where m.MissionRatings.Any()
                                          orderby m.MissionRatings.Select(r => r.Rating).Average() descending
                                          select m).ToList(); // Get top 10 most highly ranked missions

                Missions = new Models.ViewModels.Mission
                {
                    Missions = mostRankedMissions,
                    Country = countries,
                    Cities = city
                };


            }

            else if (sort_by == "top favourite")
            {
                var mostFavouriteMissions = (from m in mission
                                          where m.FavoriteMissions.Any()
                                          orderby m.FavoriteMissions.Select(r=>r.MissionId).Count() descending
                                          select m).ToList(); // Get top 10 most highly favourite  missions

                Missions = new Models.ViewModels.Mission
                {
                    Missions = mostFavouriteMissions,
                    Country = countries,
                    Cities = city
                };


            }
            else if (sort_by == "random")
            {
                Random random = new Random();
                Missions = new Models.ViewModels.Mission
                {
                    Missions = mission.OrderBy(x => random.Next()).ToList(),
                    Country = countries,
                    Cities = city
                };
            }



          

            //if no filter apply
            else
            {

                Missions = new Models.ViewModels.Mission
                {
                    Missions = mission,
                    Country = countries,
                    Cities = city
                };
            }
            if (mission.Count > 9)
            {
                //missions = missions.Where(x => x.Status is true).Take(9).ToList();
                missions = missions.Take(9).ToList();


                return Missions;
            }

            return Missions;
        }

        //get searched missions
        public Models.ViewModels.Mission GetSearchMissions(string key)
        {



            missions = missions.Where(x => x.Status is true).ToList();

            //}
            var mission = (from m in missions
                           where m.Title.ToLower().Contains(key) || m.Description.ToLower().Contains(key)
                           select m).ToList();
            var Missions = new Models.ViewModels.Mission
            {
                Missions = mission,
            };

            //get cities as per country

            //sort all mission as per user need



            return Missions;
        }





        //volunteer mission page

        //get mission 
        CI.Models.ViewModels.Volunteer_Mission IMission.Mission(long id, long user_id)
        {




            Models.Mission? mission = _db.Missions.Find(id);
            if (mission is not null)
            {
                List<User> all_volunteers = new List<User>();
                List<User> already_recommended = new List<User>();
                double avg_ratings = 0;
                bool applied_or_not = false;
                int rating_count = 0;
                int rating = 0;

                //get rating if user already rated
                var Rating = (from r in ratings
                              where r.UserId.Equals(user_id) && r.MissionId.Equals(id)
                              select r).ToList();
                if (Rating.Count > 0)
                {
                    rating = Rating.ElementAt(0).Rating;
                }

                //know that user add this mission as a favourite or not
                var favouritemission = (from fm in favoriteMissions
                                        where fm.UserId.Equals(user_id) && fm.MissionId.Equals(id)
                                        select fm).ToList();

                //get mission

                //find avg of mission rating
                if (mission.MissionRatings.Count > 0)
                {
                    avg_ratings = (from m in mission.MissionRatings
                                   select m.Rating).Average();
                    rating_count = (from m in mission.MissionRatings
                                    select m).ToList().Count;
                }


                //check that user applied or not
                List<MissionApplication> applied = (from ma in missionApplications
                                                    where ma.MissionId.Equals(mission?.MissionId) && ma.UserId.Equals(user_id) && ma.ApprovalStatus == "APPROVE"
                                                    select ma).ToList();
                if (applied.Count > 0)
                {
                    applied_or_not = true;
                }


                //get recent volunteers
                List<User> myusers = (from ma in missionApplications
                                      where ma.MissionId.Equals(mission?.MissionId) && !ma.UserId.Equals(user_id) && ma.ApprovalStatus=="APPROVE"
                                      select ma.User).ToList();


                //get related missions
                List<Models.Mission> related_mission = (from m in missions
                                                        where m.City.Name.Equals(mission?.City.Name) && !m.MissionId.Equals(mission.MissionId)
                                                        select m).Take(3).ToList();

                //get related mission if no mission available in city
                if (related_mission.Count == 0)
                {
                    related_mission = (from m in missions
                                       where m.Country.Name.Equals(mission?.Country.Name) && !m.MissionId.Equals(mission.MissionId)
                                       select m).Take(3).ToList();
                    if (related_mission.Count == 0)
                    {
                        related_mission = (from m in missions
                                           where m.Theme.Title.Equals(mission?.Theme.Title) && !m.MissionId.Equals(mission.MissionId)
                                           select m).Take(3).ToList();
                    }
                }

                //get all already recommended users
                already_recommended_users = (from a in already_recommended_users
                                             where a.MissionId == id && a.FromUserId == user_id
                                             select a).ToList();
                if (already_recommended_users.Count > 0)
                {
                    foreach (var item in already_recommended_users)
                    {
                        already_recommended.Add(item.ToUser);
                    }
                }

                //now get all remaining users for recomendation
                users = (from u in users
                         where myusers.Contains(u) && user_id != u.UserId 
                         select u).ToList();
                if (users.Count > 0)
                {
                    foreach (var item in users)
                    {
                        if (!already_recommended.Contains(item))
                        {
                            all_volunteers.Add(item);
                        }
                    }
                }
                var goalMissions = _db.GoalMissions.Where(x => x.MissionId == id).ToList();


                return new CI.Models.ViewModels.Volunteer_Mission { missionApplications = missionApplications, goal = goalMissions, timesheet = timesheets, mission = mission, related_mission = related_mission, Recent_volunteers = myusers.Take(9).ToList(), Total_volunteers = myusers.Count, Favorite_mission = favouritemission.Count, Rating = rating, All_volunteers = all_volunteers, Avg_ratings = avg_ratings, Rating_count = rating_count, Applied_or_not = applied_or_not };
            }
            else
            {
                return null;
            }
        }


        //add to favourite
        public bool add_to_favourite(long user_id, long mission_id)
        {
            if (user_id != 0 && mission_id != 0)
            {
                var favouritemission = (from fm in favoriteMissions
                                        where fm.UserId.Equals(user_id) && fm.MissionId.Equals(mission_id)
                                        select fm).ToList();
                if (favouritemission.Count == 0)
                {
                    _db.FavoriteMissions.Add(new FavoriteMission
                    {
                        UserId = user_id,
                        MissionId = mission_id
                    });
                    Save();
                    return true;
                }
                else
                {
                    _db.Remove(favouritemission.ElementAt(0));
                    Save();
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        //apply for mission
        public bool apply_for_mission(long user_id, long mission_id)
        {

            DateTime current = DateTime.Now;
            if (user_id != 0 && mission_id != 0)
            {
                var missionapplication = (from ma in missionApplications
                                          where ma.UserId.Equals(user_id) && ma.MissionId.Equals(mission_id)
                                          select ma).ToList();

                if (missionapplication.Count == 0)
                {
                    _db.MissionApplications.Add(new MissionApplication
                    {
                        AppliedAt = current,
                        UserId = user_id,
                        MissionId = mission_id
                    });


                    Save();
                    //for seat decreament
                    //var missionseats = _db.Missions.SingleOrDefault(e => e.MissionId == mission_id);
                    //if (missionseats == null)
                    //{
                    //    return false;
                    //}
                    //else
                    //{
                    //    missionseats.TotalSeats--;
                    //}
                    //finish seat decrement


                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        //get all comments of mission
        public IEnumerable<Models.ViewModels.Comment_Viewmodel> comment(long user_id, long mission_id, string comment, int length)
        {
            Comment mycomment = new Comment()
            {
                UserId = user_id,
                MissionId = mission_id,
                Comment1 = comment
            };
            _db.Comments.Add(mycomment);
            Save();
            comments = _db.Comments.ToList();
            IEnumerable<CI.Models.ViewModels.Comment_Viewmodel> mission_comments = (from c in comments
                                                                                    where c.MissionId.Equals(mission_id)
                                                                                    select new CI.Models.ViewModels.Comment_Viewmodel { User_Comment = c, user = c.User }).Skip(length);
            return mission_comments;
        }

        //get volunteers as per page
        public Models.ViewModels.Volunteer_Mission Next_Volunteers(int count, long user_id, long mission_id)
        {
            Models.Mission? mission = _db.Missions.Find(mission_id);
            List<User> users = (from ma in missionApplications
                                where ma.MissionId.Equals(mission?.MissionId) && !ma.UserId.Equals(user_id)
                                select ma.User).ToList();
            return new CI.Models.ViewModels.Volunteer_Mission { mission = mission, Recent_volunteers = users.Skip(9 * count).Take(9).ToList(), Total_volunteers = users.Count };
        }


        //rate mission
        public bool Rate_mission(long user_id, long mission_id, int rating)
        {
            var Rating = (from r in ratings
                          where r.UserId.Equals(user_id) && r.MissionId.Equals(mission_id)
                          select r).ToList();
            if (Rating.Count == 0)
            {
                _db.MissionRatings.Add(new MissionRating
                {
                    UserId = user_id,
                    MissionId = mission_id,
                    Rating = rating
                });
                Save();
                return true;
            }
            else
            {
                Rating.ElementAt(0).Rating = rating;
                Save();
                return true;
            }

        }

        //recommend to user by mail
        public bool Recommend(long user_id, long mission_id, List<long> co_workers)
        {
            foreach (var user in co_workers)
            {
                _db.MissionInvites.Add(new MissionInvite
                {
                    FromUserId = user_id,
                    ToUserId = user,
                    MissionId = mission_id
                });
            }
            Save();
            User from_user = _db.Users.FirstOrDefault(c => c.UserId.Equals(user_id));
            List<string> Email_users = (from u in users
                                        where co_workers.Contains(u.UserId)
                                        select u.Email).ToList();
            foreach (var email in Email_users)
            {
                var senderEmail = new MailAddress("tatvasoft51@gmail.com", "CI-Platform");
                var receiverEmail = new MailAddress(email, "Receiver");
                var password = "vlpzyhibrvpaewte";
                var sub = "Recommendation";
                var body = "<h1>Recommend Mission By:" + from_user?.FirstName + " " + from_user?.LastName + "</h1><br>" + $"<a href='https://localhost:7180/volunteering_mission/{mission_id}" + "'>" + "<b style='color:red;'>Click Here to go mission Details</b>  </a>";
                //var body = "Recommend By " + from_user?.FirstName + " " + from_user?.LastName + "\n" + $"https://localhost:44336/volunteering_mission/{mission_id}";
                var smtp = new SmtpClient
                {
                    Host = "smtp.gmail.com",
                    Port = 587,
                    EnableSsl = true,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential(senderEmail.Address, password)
                };
                using (var mess = new MailMessage(senderEmail, receiverEmail)
                {
                    Subject = sub,
                    Body = body,
                    IsBodyHtml = true
                })
                {
                    smtp.Send(mess);
                }
            }
            return true;
        }

        public void Save()
        {
            _db.SaveChanges();
        }


    }
}
