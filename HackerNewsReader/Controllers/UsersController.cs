using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HackerNewsReader.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class UsersController : ControllerBase
  {
    private readonly HnReaderContext _db;

    public UsersController(HnReaderContext db)
    {
      _db = db;
    }

    [HttpGet]
    public async Task<List<User>> GetAllUsers()
    {
      var result = await _db.Users.ToListAsync();

      return result;
		}

    [HttpGet("{id}")]
    public async Task<User?> GetUserById(int id)
    {
      var result = await _db.FindAsync<User>(id);
      if (result == null)
      {
        return null;
		  }
      return result;
		}
  }
}

