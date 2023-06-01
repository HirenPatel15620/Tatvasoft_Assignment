using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Model.Data;
using Model.Models;
using Model.ViewModel;
using Repository.Interface;
using System.Data;

namespace WebApplication1.Controllers
{
    [Route("StoreProcedure")]
    [ApiController, Authorize]
    public class SPUserController : Controller
    {
        private readonly IUser repo;
        private readonly swaggerContext db;
        public SPUserController(IUser _repo,swaggerContext _db)
        {
            db = _db;
            repo = _repo;
        }


        [HttpGet]
        [Route("SpAllUser")]
        public async Task<ActionResult<User>> GetSpAllUser()
        {
            string Sqlstr = "EXEC CRUDUserData " +
                            "@Operation = 'AllUser'," +
                            //"@UserId =  'NULL'," +
                            "@FirstName = 'NULL'," +
                            "@LastName = 'NULL'";

            IQueryable<User> results = db.Users.FromSqlRaw(Sqlstr);
            return Ok(results.ToList());
        }





        [HttpGet]
        [Route("SpUSER")]
        public async Task<ActionResult<User>> GetSpUser(long id)
        {
            string Sqlstr = "EXEC CRUDUserData " +
                            "@Operation = 'READ'," +
                            "@UserId = " + id + "," +
                            "@FirstName = 'NULL'," +
                            "@LastName = 'NULL'";
            IQueryable<User> results = db.Users.FromSqlRaw(Sqlstr);
            return Ok(results.ToList());
        }



        [HttpPost]
        [Route("SpAddUser")]

        public async Task<ActionResult<User>> CreateUser(User user)
        {
            SqlParameter firstNameParam = new SqlParameter("@FirstName", SqlDbType.NVarChar, 50);
            firstNameParam.Value = user.FirstName;

            SqlParameter lastNameParam = new SqlParameter("@LastName", SqlDbType.NVarChar, 50);
            lastNameParam.Value = user.LastName;



            SqlParameter operationParam = new SqlParameter("@Operation", SqlDbType.NVarChar, 10);
            operationParam.Value = "CREATE";

            var userIdParam = new SqlParameter("@UserId", SqlDbType.Int);
            userIdParam.Direction = ParameterDirection.Output;

            await db.Database.ExecuteSqlRawAsync("EXEC @UserId = CRUDUserData " +
                "@FirstName = @FirstName, " +
                "@LastName = @LastName, " +
                "@Operation = @Operation",
                userIdParam, firstNameParam, lastNameParam, operationParam);


            // Fetch the newly created user based on the UserId
            //long userId = (int)userIdParam.Value;
            //var newUser = await db.Users.FindAsync(userId);
            //return Ok(newUser);
            return Ok("User successfully added. ;)");
        }





        [HttpPut]
        [Route("SpUpdateUser")]
        public async Task<ActionResult<User>> UpdateUser(long id, User user)
        {
            SqlParameter userIdParam = new SqlParameter("@UserId", SqlDbType.BigInt);
            userIdParam.Value = user.UserId;

            SqlParameter firstNameParam = new SqlParameter("@FirstName", SqlDbType.NVarChar, 50);
            firstNameParam.Value = user.FirstName;

            SqlParameter lastNameParam = new SqlParameter("@LastName", SqlDbType.NVarChar, 50);
            lastNameParam.Value = user.LastName;



            SqlParameter operationParam = new SqlParameter("@Operation", SqlDbType.NVarChar, 10);
            operationParam.Value = "UPDATE";

            var result = await db.Users.FromSqlRaw("EXEC CRUDUserData @UserId, @FirstName, @LastName, @Operation",
                userIdParam, firstNameParam, lastNameParam, operationParam).ToListAsync();

            return Ok(result);
        }

        [HttpDelete]
        [Route("SpDeleteUser")]
        public async Task<ActionResult<User>> DeleteUser(long id)
        {
            SqlParameter userIdParam = new SqlParameter("@UserId", SqlDbType.BigInt);
            userIdParam.Value = id;

            SqlParameter operationParam = new SqlParameter("@Operation", SqlDbType.NVarChar, 10);
            operationParam.Value = "DELETE";

            var result = await db.Users.FromSqlRaw("EXEC CRUDUserData @UserId, @FirstName = NULL, @LastName = NULL, @Operation = @Operation",
                userIdParam, operationParam).ToListAsync();

            return Ok(result);
        }



    }
}
