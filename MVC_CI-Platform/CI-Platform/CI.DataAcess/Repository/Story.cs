using CI.DataAcess.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CI.Models;
using System.Net.Mail;
using System.Net;

namespace CI.DataAcess.Repository
{
    public class Story:Repository<CI.Models.Story>,IStory
    {
        private readonly CiPlatformContext _db;
        List<MissionApplication> missionApplications = new List<MissionApplication>();
        List<CI.Models.StoryMedia> medias = new List<CI.Models.StoryMedia>();
        List<Models.Story> stories = new List<Models.Story>();
        List<User> users = new List<User>();
        List<StoryInvite> already_recommended_users= new List<StoryInvite>();
        List<StoryView> views = new List<StoryView>();
        public Story(CiPlatformContext db) : base(db)
        {
            _db = db;
            getdetails();
        }
        
        
        //for share story page
        public bool AddStory(long user_id,long id, long mission_id, string title, string published_date, string mystory, List<string> media,string type)
        {
            CI.Models.Story story = new CI.Models.Story();
            if (type == "PUBLISHED")
            {
                //if story is save in draft

                if (id != 0)
                {
                    CI.Models.Story edit_story = _db.Stories.FirstOrDefault(c => c.StoryId.Equals(id));
                    edit_story.Title = title;
                    edit_story.PublishedAt= DateTime.Parse(published_date);
                    edit_story.Description = mystory;
                    edit_story.Status = "PUBLISHED";
                    List<StoryMedia> storymedias = (from m in medias
                                       where m.StoryId == id
                                       select m).ToList();
                            _db.StoryMedia.RemoveRange(storymedias);
                    foreach (var item in media)
                    {
                        _db.StoryMedia.Add(new StoryMedia
                        {
                            StoryId = id,
                            Type = "images",
                            Path = item
                        });
                    }
                    _db.SaveChanges();
                }

                //direct published at first time

                else
                {
                    story.UserId = user_id;
                    story.MissionId = mission_id;
                    story.Title = title;
                    story.Description = mystory;
                    story.PublishedAt = DateTime.Parse(published_date);
                    story.Status = "PUBLISHED";
                    _db.Stories.Add(story);
                    _db.SaveChanges();
                    long story_id = story.StoryId;
                    foreach (var item in media)
                    {
                        _db.StoryMedia.Add(new StoryMedia
                        {
                            StoryId = story_id,
                            Type = "images",
                            Path = item
                        });
                    }
                }
            }

            //if user save story 
            else
            {
                story.UserId = user_id;
                story.MissionId = mission_id;
                story.Title = title;
                story.Description = mystory;
                story.PublishedAt = DateTime.Parse(published_date);
                _db.Stories.Add(story);
                _db.SaveChanges();
                long story_id = story.StoryId;
                foreach (var item in media)
                {
                    _db.StoryMedia.Add(new StoryMedia
                    {
                        StoryId = story_id,
                        Type = "images",
                        Path = item
                    });
                }

            }
            _db.SaveChanges();
            return true;
            
        }

        //access all needed data
        public void getdetails()
        {
            missionApplications = _db.MissionApplications.ToList();
            medias = _db.StoryMedia.ToList();
            stories = _db.Stories.ToList();
            users = _db.Users.ToList();
            already_recommended_users = _db.StoryInvites.ToList();
            views = _db.StoryViews.ToList();
        }


        //filter stories 
        public Models.ViewModels.Mission GetFileredStories( int page_index,long user_id)
        {
            //get stories as per page
            stories = (from s in stories
                       where s.Status == "PUBLISHED" || s.UserId == user_id
                       orderby s.Status ascending
                       select s).ToList();
            return new CI.Models.ViewModels.Mission {  Stories = stories.Skip(9*page_index).Take(9).ToList() };
        }


        //first time load 9-stories
        public CI.Models.ViewModels.Mission GetStories(long user_id)
        {
            stories = (from s in stories
                       where s.Status == "PUBLISHED" || s.UserId == user_id
                       orderby s.Status ascending
                       select s).ToList();
            return new CI.Models.ViewModels.Mission { Stories=stories.Take(9).ToList(),total_missions=stories.Count} ;
        }


        //all mission in which user apply 
        public List<CI.Models.Mission> Get_User_Missions(long user_id)
        {
            List<CI.Models.Mission> User_Missions = (from m in missionApplications
                                                     where m.UserId == user_id
                                                     select m.Mission).ToList();
            return User_Missions;
        }






        //Story-Details Page

        //get story
        public Models.ViewModels.StoryViewModel GetStory(long user_id, long id)
        {
            var story = _db.Stories.FirstOrDefault(c => c.StoryId == id);
            if (story is not null)
            {
                List<User> already_recommended = new List<User>();
                List<User> co_workers = new List<User>();
                already_recommended_users = (from a in already_recommended_users
                                             where a.StoryId == id && a.FromUserId == user_id
                                             select a).ToList();
                if (already_recommended_users.Count > 0)
                {
                    foreach (var item in already_recommended_users)
                    {
                        already_recommended.Add(item.ToUser);
                    }
                }
                users = (from u in users
                         where user_id != u.UserId
                         select u).ToList();

                if (users.Count > 0)
                {
                    foreach (var item in users)
                    {
                        if (!already_recommended.Contains(item))
                        {
                            co_workers.Add(item);
                        }
                    }
                }
                return new Models.ViewModels.StoryViewModel { story = story, co_workers = co_workers };
            }
            else
            {
                return null;
            }
        }

        public bool Recommend(long user_id, long story_id, List<long> co_workers)
        {
            foreach (var user in co_workers)
            {
                _db.StoryInvites.Add(new StoryInvite
                {
                    FromUserId = user_id,
                    ToUserId = user,
                    StoryId = story_id
                });
            }
            _db.SaveChanges();
            User from_user = _db.Users.FirstOrDefault(c => c.UserId.Equals(user_id));
            List<string> Email_users = (from u in users
                                        where co_workers.Contains(u.UserId)
                                        select u.Email).ToList();
            foreach (var email in Email_users)
            {
                var senderEmail = new MailAddress("dhruvikkothiya732002@gmail.com", "dhruvik");
                var receiverEmail = new MailAddress(email, "Receiver");
                var password = "clpd gojh borl hemp";
                var sub = "Recommendation";
                var body = "Recommend By " + from_user?.FirstName + " " + from_user?.LastName + "\n" + $"https://localhost:44334/stories/detail/{story_id}";
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
                    Body = body
                })
                {
                    smtp.Send(mess);
                }
            }
            return true;
        }

        public void Add_View(long user_id, long story_id)
        {
            var view_exist = _db.StoryViews.FirstOrDefault(c => c.UserId.Equals(user_id) && c.StoryId.Equals(story_id));
            if(view_exist is null)
            {
                _db.StoryViews.Add(new StoryView { StoryId = story_id, UserId = user_id });
                _db.SaveChanges();
            }
        }
    }
}
