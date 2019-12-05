using MerchantApi.Dtos;
using MerchantApi.Models;
using MerchantApi.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace MerchantApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : Controller
    {
        private IUserRepository _userRepository;

        public UsersController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
            
        //api/users
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<UserDto>))]
        [ProducesResponseType(400)]
        public IActionResult GetUsers()
        {
            var users = _userRepository.GetUsers();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var userDto = new List<UserDto>();
            foreach (var user in users)
            {
                userDto.Add(new UserDto
                {
                    Id = user.Id,
                    Name = user.Name
                });
            }

            return Ok(userDto);
        }
            
        //api/users/userId
        [HttpGet("{userId}", Name = "GetUser")]
        [ProducesResponseType(200, Type = typeof(UserDto))]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public IActionResult GetUser(int userId)
        {
            if (!_userRepository.UserExists(userId))
                return NotFound();

            var user = _userRepository.GetUser(userId);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var userDto = new List<UserDto>();

            userDto.Add(new UserDto
            {
                Id = user.Id,
                Name = user.Name
            });

            return Ok(userDto);
        }
               
        //api/users/transactionId/user
        [HttpGet("{transactionId}/user")]
        [ProducesResponseType(200, Type = typeof(UserDto))]
        [ProducesResponseType(400)]
        public IActionResult GetUserByTransaction(int transactionId)
        {
            //TO DO - Validate transactionId
            var user = _userRepository.GetUserOfATransaction(transactionId);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var userDto = new UserDto()
            {
                Id = user.Id,
                Name = user.Name
            };

            return Ok(userDto);
        }

        //api/users/userId/transactions
        [HttpGet("{userId}/transactions")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<TransactionDto>))]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public IActionResult GetTransactionsByUser(int userId)
        {
            if (!_userRepository.UserExists(userId))
                return NotFound();

            var transactions = _userRepository.GetTransactionsByUser(userId);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var transactionsDto = new List<TransactionDto>();

            foreach (var transaction in transactions)
            {
                transactionsDto.Add(new TransactionDto()
                {
                    Id = transaction.Id,
                    Name = transaction.Name,
                    Price = transaction.Price,
                    DateTime = transaction.DateTime
                });
            }

            return Ok(transactionsDto);
        }

        //api/users
        [HttpPost]
        [ProducesResponseType(201, Type = typeof(User))]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public IActionResult CreateUser([FromBody]User userToCreate)
        {
            if (userToCreate == null)
                return BadRequest(ModelState);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!_userRepository.CreateUser(userToCreate))
            {
                ModelState.AddModelError("", $"Something went wrong saving {userToCreate.Name}");
                return StatusCode(500, ModelState);
            }

            return CreatedAtRoute("GetUser", new { userId = userToCreate.Id }, userToCreate);
        }

        //api/users/userId
        [HttpPut("{userId}")]
        [ProducesResponseType(204)]//No Context
        [ProducesResponseType(400)]//Bad Request
        [ProducesResponseType(404)]//Not Found
        [ProducesResponseType(422)]//entity error
        [ProducesResponseType(500)]//internal server error
        public IActionResult UpdateUser(int userId, [FromBody]User updatedUserInfo)
        {
            if (updatedUserInfo == null)
                return BadRequest(ModelState);

            if (userId != updatedUserInfo.Id)
                return BadRequest(ModelState);

            if (!_userRepository.UserExists(userId))
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!_userRepository.UpdateUser(updatedUserInfo))
            {
                ModelState.AddModelError("", $"Something went wrong updating {updatedUserInfo.Name}");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }

        //api/users/userId
        [HttpDelete("{userId}")]
        [ProducesResponseType(204)]//No Context
        [ProducesResponseType(400)]//Bad Request
        [ProducesResponseType(404)]//Not Found
        [ProducesResponseType(500)]//internal server error
        public IActionResult DeleteUser(int userId)
        {
            if (!_userRepository.UserExists(userId))
                return NotFound(); //not found

            var userToDelete = _userRepository.GetUser(userId);
            //var transactionsToDelete = _userRepository.GetTransactionsByUser(userId);

            if (!ModelState.IsValid)
                return BadRequest(ModelState); //bad request

            if (!_userRepository.DeleteUser(userToDelete))
            {
                ModelState.AddModelError("", $"Something went wrong deleting {userToDelete.Name}");
                return StatusCode(500, ModelState); //internal server error
            }

            // TO DO - Delete Transaction
            //if (!_reviewRepository.DeleteReviews(reviewsToDelete.ToList()))
            //{
            //    ModelState.AddModelError("", $"Something went wrong deleting {reviewerToDelete.FirstName} {reviewerToDelete.LastName}");
            //    return StatusCode(500, ModelState); //internal server error
            //}

            return NoContent(); //204

        }

    }
}
