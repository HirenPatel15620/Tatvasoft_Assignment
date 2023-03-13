using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CI.DataAcess.Repository.IRepository;
using CI.Models;
using CI.Models.Models;

namespace CI.DataAcess.Repository
{
    public class Mission : Repository<CI.Models.Models.Mission>, IMission
    {
        private readonly CiPlatformContext _db;
        List<CI.Models.Models.Mission> missions = new List<Models.Models.Mission>();
        List<CI.Models.MissionMedia> image = new List<Models.MissionMedia>();
        List<MissionTheme> theme = new List<MissionTheme>();
        List<Country> countries = new List<Country>();
        List<City> cities = new List<City>();
        List<Skill> skills = new List<Skill>();
        List<MissionSkill> missionskills = new List<MissionSkill>();
        public Mission(CiPlatformContext db) : base(db)
        {
            _db = db;
            getmissions();
        }
        public void getmissions()
        {
            missions = _db.Missions.ToList();
            //image = _db.MissionMedia.ToList();
            theme = _db.MissionThemes.ToList();
            countries = _db.Countries.ToList();
            cities = _db.Cities.ToList();
            skills = _db.Skills.ToList();
            missionskills = _db.MissionSkills.ToList();
        }
        public List<Models.ViewModels.Mission> GetAllMission()
        {
            var Missions = (from m in missions
                            join i in image on m.MissionId equals i.MissionId into data
                            from i in data.DefaultIfEmpty().Take(1)
                            select new CI.Models.ViewModels.Mission { image = i, Missions = m, Country = countries, themes = theme, skills = skills }).ToList();
            return Missions;
        }

        public List<Models.ViewModels.Mission> GetFilteredMissions(List<string> Countries, List<string> Cities, List<string> Themes, List<string> Skills, string sort_by)
        {
            List<CI.Models.ViewModels.Mission> Missions = new List<Models.ViewModels.Mission>();
            List<City> city = new List<City>();
            List<CI.Models.Models.Mission> mission = new List<CI.Models.Models.Mission>();
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
            if (Cities.Count > 0)
            {

                mission = (from m in missions
                           where Cities.Contains(m.City.Name) || Themes.Contains(m.Theme.Title)
                           select m).ToList();
                var skill_missions = (from s in missionskills
                                      where Skills.Contains(s.Skill.SkillName)
                                      select s.Mission).ToList();
                foreach (var skill_mission in skill_missions)
                {
                    if (!mission.Contains(skill_mission))
                    {
                        mission.Add(skill_mission);
                    }
                }
            }
            else if (Countries.Count > 0 || Themes.Count > 0 || Skills.Count > 0)
            {
                mission = (from m in missions
                           where Countries.Contains(m.Country.Name) || Cities.Contains(m.City.Name) || Themes.Contains(m.Theme.Title)
                           select m).ToList();
                var skill_missions = (from s in missionskills
                                      where Skills.Contains(s.Skill.SkillName)
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
                Missions = (from m in mission
                            orderby m.CreatedAt descending
                            join i in image on m.MissionId equals i.MissionId into data
                            from i in data.DefaultIfEmpty().Take(1)
                            select new CI.Models.ViewModels.Mission { image = i, Missions = m, Country = countries, Cities = city, Mission_city = m.City.Name, Mission_theme = m.Theme.Title }).ToList();
            }
            else if (sort_by == "lowest available seats")
            {
                Missions = (from m in mission
                            orderby m.Availability ascending
                            join i in image on m.MissionId equals i.MissionId into data
                            from i in data.DefaultIfEmpty().Take(1)
                            select new CI.Models.ViewModels.Mission { image = i, Missions = m, Country = countries, Cities = city, Mission_city = m.City.Name, Mission_theme = m.Theme.Title }).ToList();
            }
            else if (sort_by == "highest available seats")
            {
                Missions = (from m in mission
                            orderby m.Availability descending
                            join i in image on m.MissionId equals i.MissionId into data
                            from i in data.DefaultIfEmpty().Take(1)
                            select new CI.Models.ViewModels.Mission { image = i, Missions = m, Country = countries, Cities = city, Mission_city = m.City.Name, Mission_theme = m.Theme.Title }).ToList();
            }
            else if (sort_by == "registration deadline")
            {

                Missions = (from m in mission
                            orderby m.EndDate ascending
                            join i in image on m.MissionId equals i.MissionId into data
                            from i in data.DefaultIfEmpty().Take(1)
                            select new CI.Models.ViewModels.Mission { image = i, Missions = m, Country = countries, Cities = city, Mission_city = m.City.Name, Mission_theme = m.Theme.Title }).ToList();
            }
            else
            {

                Missions = (from m in mission
                            join i in image on m.MissionId equals i.MissionId into data
                            from i in data.DefaultIfEmpty().Take(1)
                            select new CI.Models.ViewModels.Mission { image = i, Missions = m, Country = countries, Cities = city, Mission_city = m.City.Name, Mission_theme = m.Theme.Title }).ToList();
            }
            return Missions;
        }

        public List<Models.ViewModels.Mission> GetSearchMissions(string key)
        {

            var mission = (from m in missions
                           where m.Title.ToLower().Contains(key) || m.Description.ToLower().Contains(key)
                           select m).ToList();
            var Missions = (from m in mission
                            join i in image on m.MissionId equals i.MissionId into data
                            from i in data.DefaultIfEmpty().Take(1)
                            select new CI.Models.ViewModels.Mission { image = i, Missions = m, Mission_city = m.City.Name, Mission_theme = m.Theme.Title }).ToList();
            return Missions.ToList();
        }

        public void Save()
        {
            _db.SaveChanges();
        }



        //public class PageInfo
        //{
        //    public int TotalItems { get; set; }
        //    public int ItemsPerPage { get; set; }
        //    public int CurrentPage { get; set; }

        //    public PageInfo()
        //    {
        //        CurrentPage = 1;
        //    }
        //    //starting item number in the page
        //    public int PageStart
        //    {
        //        get { return ((CurrentPage - 1) * ItemsPerPage + 1); }
        //    }
        //    //last item number in the page
        //    public int PageEnd
        //    {
        //        get
        //        {
        //            int currentTotal = (CurrentPage - 1) * ItemsPerPage + ItemsPerPage;
        //            return (currentTotal < TotalItems ? currentTotal : TotalItems);
        //        }
        //    }
        //    public int LastPage
        //    {
        //        get { return (int)Math.Ceiling((decimal)TotalItems / ItemsPerPage); }
        //    }
        //}





    }
}
