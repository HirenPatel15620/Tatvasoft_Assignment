using CI_Platform.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace CI_Platform.Entities.ViewModels
{
    public class AdminModel
    {
        public Admin admin { get; set; } = new Admin();

        public List<User> user { get; set; } = new List<User>();

        public List<CmsPage> cms { get; set; } = new List<CmsPage>();

        public List<StoryAdminModel> story { get; set; } = new List<StoryAdminModel>();

        public StoryAdminModel storyView  { get; set; } = new StoryAdminModel();

        public List<Banner> banner { get; set; } = new List<Banner>();

        public List<Mission>? missionModel { get; set; } = new List<Mission>();

        public List<MissionApplicationAdminModel> missionApplication { get; set; } = new List<MissionApplicationAdminModel>();

        public List<MissionTheme> missionTheme { get; set; } = new List<MissionTheme>();

        public List<Skill> missionSkill { get; set; } = new List<Skill>();

        public List<SelectListItem> SkillList { get; set; } = new List<SelectListItem>();

        public List<SelectListItem> ThemeList { get; set; } = new List<SelectListItem>();

        public List<SelectListItem> CountryList { get; set; } = new List<SelectListItem>();

    }
}
