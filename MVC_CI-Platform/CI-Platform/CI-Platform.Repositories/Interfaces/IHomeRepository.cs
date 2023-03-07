using CI_Platform.Models;
using CI_Platform.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CI_Platform.Repositories.Interfaces
{
    public interface IHomeRepository
    {
        List<Country> GetCountries();
        List<City> GetCities();
        List<MissionTheme> GetMissionThemes();
        List<Skill> GetSkills();
        List<Mission> GetMissions();
        List<GoalMission> GetGoalMissions();
        List<Timesheet> GetSumOfAction();
    }
}
