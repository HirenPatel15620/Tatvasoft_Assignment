using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Model.Data;
using Model.Models;
using Repository.Interface;
using System.Data;

namespace WebApplication1.Controllers
{
    [Route("User")]
    [ApiController, Authorize]
    public class UserController : Controller
    {
        private readonly IUser repo;
        private readonly swaggerContext db;
        public UserController(IUser _repo, swaggerContext _db)
        {
            db = _db;
            repo = _repo;
        }
        [HttpGet]
        [Route("AllUsers")]
        public async Task<List<User>> GetAllUsers()
        {
            var model = new List<Model.Models.User>();  // Update the type here
            var users = await repo.GetAllUsers();

            if (users is not null)
            {
                model = users;
            }
            return model;
        }

        [HttpPost]
        [Route("AddUser")]
        public async Task<User> AddUser(User newuser)
        {
            var model = new User();
            //var user = await mediator.Send(new AddUserCommand(
            //    newuser.Name, newuser.Address, newuser.CompanyName, newuser.Designation));
            var user = await repo.AddUser(newuser);
            if (user is not null)
            {
                model = user;
            }
            return model;
        }
        [HttpGet]
        [Route("GetUser")]
        public async Task<User> GetUser(long id)
        {
            var model = new User();
            var user = await repo.GetUserById(id);
            if (user is not null)
            {
                model = user;
            }
            return model;
        }
        [HttpPut]
        [Route("UpdateUser")]
        public async Task<User> UpdateUser(User newuser)
        {
            var model = new User();
            //var user = await mediator.Send(new UpdateUserCommand(
            //    newuser.EmployeeId, newuser.Name, newuser.Address, newuser.CompanyName, newuser.Designation));
            var user = await repo.UpdateUser(newuser);

            if (user is not null)
            {
                model = user;
            }
            return model;
        }
        [HttpDelete]
        [Route("DeleteUser")]
        public async Task<bool> DeleteUser(int id)
        {
            bool result = await repo.DeleteUser(id);
            return result;
        }

    }
}
