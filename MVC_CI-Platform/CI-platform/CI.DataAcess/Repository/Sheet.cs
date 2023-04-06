using CI.Models;
using CI.Repository.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CI.Repository.Repository
{
    public class Sheet : Repository<CI.Models.Timesheet>, ISheet
    {
        private readonly CiPlatformContext _db;

        public Sheet(CiPlatformContext db) : base(db)
        {
            _db = db;

        }


        public Timesheet GetTimesheetDataById(long id)
        {
            return _db.Timesheets.Where(x=>x.TimesheetId == id).FirstOrDefault();   
        }

        public List<Timesheet> GetTimeSheetDataByUserId(long userId)
        {
            return _db.Timesheets.Where(x => x.UserId == userId && x.Status == "APPROVED").OrderByDescending(x => x.CreatedAt).Include(x => x.Mission).ToList();
        }

        public void InsertTimesheet(Timesheet timesheet)
        {
            _db.Timesheets.Add(timesheet);
        }

        public void UpdateTimesheet(Timesheet timesheet)
        {
            _db.Timesheets.Update(timesheet);
        }




        public List<Timesheet> GetAllTimeSheetRecordsByUser(string userid)
        {
            return _db.Timesheets.Include(x => x.Mission).Where(x => x.UserId == Convert.ToInt32(userid)).ToList();
        }

        public List<MissionApplication> GetTimetypeMissionsByUserId(string? userid)
        {
            return _db.MissionApplications.Include(x => x.Mission).Where(x => x.UserId == Convert.ToInt32(userid) && x.ApprovalStatus == "APPROVE" && x.Mission.MissionType == "TIME").ToList();
        }

        public List<MissionApplication> GetGoaltypeMissionsByUserId(string? userid)
        {
            return _db.MissionApplications.Include(x => x.Mission).Where(x => x.UserId == Convert.ToInt32(userid) && x.ApprovalStatus == "APPROVE" && x.Mission.MissionType == "GOAL").ToList();
        }
        public bool AddTimeSheetRecords(Timesheet timesheet)
        {
            _db.Timesheets.Add(timesheet);
            _db.SaveChanges();
            return true;
        }

        public Timesheet GetTimesheetrecordByTimesheetId(int timesheetid)
        {
            return _db.Timesheets.Where(x => x.TimesheetId == timesheetid).FirstOrDefault();
        }

        public bool UpdateTimeSheetRecord(Timesheet record)
        {
            _db.Timesheets.Update(record);
            _db.SaveChanges();
            return true;
        }

        public string GetMissionTypeById(int missionid)
        {
            return _db.Missions.Where(x => x.MissionId == missionid).Select(x => x.MissionType).FirstOrDefault();
        }

        public bool DeleteTimesheetRecord(Timesheet timesheet)
        {
            _db.Timesheets.Remove(timesheet);
            _db.SaveChanges();
            return true;
        }

        public List<City> GetCityByCountryName(string CountryName)
        {
            return _db.Cities.Where(x => x.Country.Name.Contains(CountryName)).ToList();
        }

        //public List<CmsPage> GetALLPolicies()
        //{
        //    return _db.CmsPages.Where(x => x.Status == 1).ToList();
        //}




    }
}