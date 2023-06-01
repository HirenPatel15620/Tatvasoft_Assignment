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
    [ApiController]
    public class UserReactController : Controller
    {
        private readonly IUser repo;
        private readonly swaggerContext _db;
        public UserReactController(IUser _repo, swaggerContext db)
        {
            _db = db;
            repo = _repo;
        }
        [HttpGet("/getall")]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        {
            if (_db.Users == null)
            {
                return NotFound();
            }
            return await _db.Users.ToListAsync();
        }

        [HttpGet("/get")]
        public async Task<ActionResult<User>> GetUser(long id)
        {
            if (_db.Users == null)
            {
                return NotFound();
            }
            var user = await _db.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();

            }

            return Ok(user);

        }

        [HttpPost("/add")]
        public async Task<ActionResult<User>> PostUser(User user)
        {
            _db.Users.Add(user);
            await _db.SaveChangesAsync();
            return CreatedAtAction(nameof(GetUser), new { id = user.UserId }, user);
        }


        [HttpPut("{id}")]
        public async Task<ActionResult> PutUser(long id, User user)
        {
            if (id != user.UserId)
            {
                return BadRequest();
            }
            _db.Entry(user).State = EntityState.Modified;
            try
            {
                await _db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteUser(long id)
        {
            if (_db.Users == null)
            {
                return NotFound();
            }
            var user = await _db.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            _db.Users.Remove(user);
            await _db.SaveChangesAsync();
            return Ok();
        }
    }
}

