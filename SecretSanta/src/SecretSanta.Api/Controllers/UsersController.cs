using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using SecretSanta.Api.Dto;
using SecretSanta.Business;
using SecretSanta.Data;

namespace SecretSanta.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private IUserRepository UserManager { get; }

        public UsersController(IUserRepository userManager)
        {
            UserManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
        }

        // /api/users
        [HttpGet]
        public IEnumerable<User> Get()
        {
            return UserManager.List();
        }

        // /api/users/<index>
        [HttpGet("{id}")]
        public ActionResult<User?> Get(int id)
        {
            if (id < 0)
            {
                return NotFound();
            }
            User? returnedUser = UserManager.GetItem(id);
            return returnedUser;
        }

        //DELETE /api/users/<index>
        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            if (id < 0)
            {
                return NotFound();
            }
            if (UserManager.Remove(id))
            {
                return Ok();
            }
            return NotFound();
        }

        // POST /api/users
        [HttpPost]
        public ActionResult<User?> Post([FromBody] User? myUser)
        {
            if (myUser is null)
            {
                return BadRequest();
            }
            return UserManager.Create(myUser);
        }

        // PUT /api/users/<id>
        [HttpPut("{id}")]
        public ActionResult Put(int id, [FromBody]UpdateUser? updatedUser)
        {
            if (updatedUser is null)
            {
                return BadRequest();
            }
            User? foundUser = UserManager.GetItem(id);
            if (foundUser is not null)
            {
                if (!string.IsNullOrWhiteSpace(updatedUser.FirstName) && !string.IsNullOrWhiteSpace(updatedUser.LastName))
                {
                    foundUser.FirstName = updatedUser.FirstName;
                    foundUser.LastName = updatedUser.LastName;
                }

                UserManager.Save(foundUser);
                return Ok();
            }
            return NotFound();
        }
    }
}