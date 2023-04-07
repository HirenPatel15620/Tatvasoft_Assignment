using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CI.Models;
namespace CI.Repository.Repository.IRepository
{
    public interface ISheet : IRepository<CI.Models.Timesheet>
    {

        List<Timesheet> GetAllTimeSheetRecordsByUser(string? userid);
        List<MissionApplication> GetTimetypeMissionsByUserId(string? userid);
        List<MissionApplication> GetGoaltypeMissionsByUserId(string? userid);
        bool AddTimeSheetRecords(Timesheet timesheet);
        Timesheet GetTimesheetrecordByTimesheetId(long timesheetid);
        bool UpdateTimeSheetRecord(Timesheet record);
        string GetMissionTypeById(long missionid);
        bool DeleteTimesheetRecord(Timesheet timesheet);
        List<City> GetCityByCountryName(string CountryName);
        //List<CmsPage> GetALLPolicies();


    }
}
