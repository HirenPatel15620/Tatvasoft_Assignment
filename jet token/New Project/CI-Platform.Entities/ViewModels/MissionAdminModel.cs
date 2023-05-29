using CI_Platform.Entities.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace CI_Platform.Entities.ViewModels
{
    public class MissionAdminModel
    {
        public Mission mission { get; set; } = new Mission();

        public long missionId { get; set; } = 0;

        public string missionName { get; set; } = string.Empty;

        public byte missionType { get; set; } = 0;

        public int seats { get; set;} = 0;

        public long missionThemeId { get; set; } = 0;

        public long missionCityId { get; set; } = 0;

        public long missionCountryId { get; set; } = 0;

        public string description { get; set; } = string.Empty;

        public string Msn_Goal_Obj { get; set; } = string.Empty;

        public int Msn_Goal_Action { get; set; } = 0;

        public string shortDescription { get; set; } = string.Empty;

        public DateTime startDate { get; set; } = DateTime.Now;

        public DateTime endDate { get; set; } = DateTime.Now;

        public DateTime deadline { get; set; } = DateTime.Now;  

        public string organizationName { get; set; } = string.Empty;    

        public string organizationDetails { get; set; } = string.Empty;

        public List<string> VideoUrl { get; set; } = new List<string>();

        public List<long> skillIds { get; set; } = new List<long>();

        public List<IFormFile> fileList { get; set; } = new List<IFormFile>();

        public List<IFormFile> docList { get; set; } = new List<IFormFile>();

        public List<string> delImgList { get; set; } = new List<string>();

        public List<string> delDocList  { get; set; } = new List<string>();


        public List<string> mimgList { get; set; } = new List<string>();

        public List<MissionDocumentDetailsModel> missionDoc { get; set; } = new List<MissionDocumentDetailsModel>();

        public List<string> skillNames { get; set; } = new List<string>();

        
        

    }
}
