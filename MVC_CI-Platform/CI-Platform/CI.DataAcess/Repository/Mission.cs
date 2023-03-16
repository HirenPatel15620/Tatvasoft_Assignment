using CI.DataAcess.Repository.IRepository;
using CI.Models;
using CI.Models.Models;

namespace CI.DataAcess.Repository
{
    public class Mission : Repository<CI.Models.Models.Mission>, IMission
    {
        private readonly CiPlatformContext _db;
        List<CI.Models.Models.Mission> missions = new List<Models.Models.Mission>();
        List<CI.Models.MissionMedia> image =new List<Models.MissionMedia>();
        List<MissionTheme> theme = new List<MissionTheme>();
        List<Country> countries =new List<Country>();
        List<City> cities = new List<City>();
        List<Skill> skills = new List<Skill>();
        List<MissionSkill> missionskills =new List<MissionSkill>();
        List<MissionDocument> mission_documents = new List<MissionDocument>();
        List<Comment> comments = new List<Comment>();
        List<User> users = new List<User>();
        List<MissionApplication> missionApplications = new List<MissionApplication>();
        List<FavoriteMission> favoriteMissions = new List<FavoriteMission>();
        List<MissionRating> ratings = new List<MissionRating>();
        public Mission(CiPlatformContext db) : base(db)
        {
            _db = db;
            getAllDetails();
        }

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

        public bool apply_for_mission(long user_id, long mission_id)
        {
           DateTime current=DateTime.Now;
           if(user_id!=0 && mission_id != 0)
            {
                var missionapplication = (from ma in missionApplications
                                          where ma.UserId.Equals(user_id) && ma.MissionId.Equals(mission_id)
                                          select ma).ToList();
                if (missionapplication.Count==0)
                {
                    _db.MissionApplications.Add(new MissionApplication
                    {
                        AppliedAt = current,
                        UserId = user_id,
                        MissionId = mission_id
                    });
                    Save();
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

        public IEnumerable<Models.ViewModels.Comment_Viewmodel> comment(long user_id, long mission_id, string comment,int length)
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
                                              select new CI.Models.ViewModels.Comment_Viewmodel {User_Comment=c,user=c.User}).Skip(length);
            return mission_comments;
        }

        public void getAllDetails()
        {
            missions = _db.Missions.ToList();
            image = _db.MissionMedia.ToList();
            theme = _db.MissionThemes.ToList();
            countries = _db.Countries.ToList();
            cities = _db.Cities.ToList();
            skills = _db.Skills.ToList();
            missionskills = _db.MissionSkills.ToList();
            mission_documents = _db.MissionDocuments.ToList();
            comments = _db.Comments.ToList();
            users = _db.Users.ToList();
            missionApplications = _db.MissionApplications.ToList();
            favoriteMissions = _db.FavoriteMissions.ToList();
            ratings = _db.MissionRatings.ToList();
        }
        public Models.ViewModels.Mission GetAllMission()
        {
            int total_missions = missions.Count;
            missions = missions.Take(9).ToList();
            var Missions = new CI.Models.ViewModels.Mission { Missions = missions,Country=countries,themes=theme,skills=skills,total_missions=total_missions};
            return Missions;
        }

        public Models.ViewModels.Mission GetFilteredMissions(List<string> Countries, List<string> Cities, List<string> Themes, List<string> Skills,string sort_by,int page_index)
        {
            CI.Models.ViewModels.Mission Missions = new Models.ViewModels.Mission();
            List<City> city = new List<City>();
            List<CI.Models.Models.Mission> mission = new List<CI.Models.Models.Mission>();
            if (page_index != 0)
            {
                missions = missions.Skip(9 * page_index).Take(9).ToList();
            }
            else
            {
                missions = missions.Take(9).ToList();
            }
            if (Countries.Count>0)
            {
                city = (from c in cities
                                   where Countries.Contains(c.Country.Name)
                                   select c).ToList();
            }
            else
            {
                city = cities;
            }
            if (Cities.Count > 0)
            {
               
                    mission = (from m in missions
                               where Cities.Contains(m.City.Name) || Themes.Contains(m.Theme.Title)
                               select m).ToList();
                    var skill_missions = (from s in missionskills
                                          where Skills.Contains(s.Skill.SkillName) && missions.Contains(s.Mission)
                                          select s.Mission).ToList();
                    foreach (var skill_mission in skill_missions)
                    {
                        if (!mission.Contains(skill_mission))
                        {
                            mission.Add(skill_mission);
                        }
                }
            }
            else if (Countries.Count > 0||Themes.Count>0|| Skills.Count > 0)
            {
                    mission = (from m in missions
                               where Countries.Contains(m.Country.Name) || Cities.Contains(m.City.Name) || Themes.Contains(m.Theme.Title)
                               select m).ToList();
                    var skill_missions = (from s in missionskills
                                          where Skills.Contains(s.Skill.SkillName) && missions.Contains(s.Mission)
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
                                orderby m.AvbSeat ascending
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
                                orderby m.AvbSeat descending
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
            else
            {

                Missions = new Models.ViewModels.Mission
                {
                    Missions = mission,
                    Country = countries,
                    Cities = city
                };
            }
            return Missions;
        }
        public Models.ViewModels.Mission GetSearchMissions(string key,int page_index)
        {
            if (page_index != 0)
            {
                missions = missions.Skip(9 * page_index).Take(9).ToList();
            }
            else
            {
                missions = missions.Take(9).ToList();
            }
            var mission= (from m in missions
                          where m.Title.ToLower().Contains(key) || m.Description.ToLower().Contains(key)
                          select m).ToList();
            var Missions = new Models.ViewModels.Mission
            {
                Missions = mission,
            };
            return Missions;
        }















        public Models.ViewModels.Volunteer_Mission Next_Volunteers(int count, long mission_id)
        {
            Models.Models.Mission? mission = _db.Missions.Find(mission_id);
            List<User> users = (from ma in missionApplications
                                where ma.MissionId.Equals(mission?.MissionId)
                                select ma.User).ToList();
            return new CI.Models.ViewModels.Volunteer_Mission {mission=mission, Recent_volunteers = users.Skip(9 * count).Take(9).ToList(),Total_volunteers=users.Count };
        }

        public bool Rate_mission(long user_id, long mission_id, int rating)
        {
            var Rating = (from r in ratings
                                    where r.UserId.Equals(user_id) && r.MissionId.Equals(mission_id)
                                    select r).ToList();
            if (Rating.Count == 0) {
                _db.MissionRatings.Add(new MissionRating
                {
                    UserId = user_id,
                    MissionId = mission_id,
                    Rating = rating.ToString()
                });
                Save();
                return true;
            }
            else
            {
                Rating.ElementAt(0).Rating = rating.ToString();
                Save();
                return true;
            }
            
        }

        public void Save()
        {
            _db.SaveChanges();
        }

        CI.Models.ViewModels.Volunteer_Mission IMission.Mission(long id,long user_id)
        {
            int rating=0;
            var Rating = (from r in ratings
                          where r.UserId.Equals(user_id) && r.MissionId.Equals(id)
                          select r).ToList();
            if (Rating.Count > 0)
            {
                rating = int.Parse(Rating.ElementAt(0).Rating);
            }
            var favouritemission = (from fm in favoriteMissions
                                    where fm.UserId.Equals(user_id) && fm.MissionId.Equals(id)
                                    select fm).ToList();
            Models.Models.Mission? mission=_db.Missions.Find(id);
            List<User> users=(from ma in missionApplications
                              where ma.MissionId.Equals(mission?.MissionId) && !ma.UserId.Equals(user_id)
                              select ma.User).ToList();
            List<Models.Models.Mission> related_mission = (from m in missions
                                                    where m.City.Name.Equals(mission?.City.Name) && !m.MissionId.Equals(mission.MissionId)
                                                    select m).Take(3).ToList();
            if(related_mission.Count == 0)
            {
                related_mission= (from m in missions
                                  where m.Country.Name.Equals(mission?.Country.Name) && !m.MissionId.Equals(mission.MissionId)
                                  select m).Take(3).ToList();
                if(related_mission.Count == 0)
                {
                    related_mission = (from m in missions
                                       where m.Theme.Title.Equals(mission?.Theme.Title) && !m.MissionId.Equals(mission.MissionId)
                                       select m).Take(3).ToList();
                }
            }
            return new CI.Models.ViewModels.Volunteer_Mission { mission=mission,related_mission=related_mission, Recent_volunteers=users.Take(9).ToList(),Total_volunteers=users.Count,Favorite_mission=favouritemission.Count,Rating=rating,All_volunteers=users};
        }
    }
}
