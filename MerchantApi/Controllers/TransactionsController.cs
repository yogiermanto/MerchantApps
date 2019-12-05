using MerchantApi.Dtos;
using MerchantApi.Models;
using MerchantApi.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MerchantApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionsController : Controller
    {
        private ITransactionRepository _transactionRepository;
        private IUserRepository _userRepository;
        private IMerchantRepository _merchantRepository;
        public TransactionsController(ITransactionRepository transactionRepository, IUserRepository userRepository,
                                    IMerchantRepository merchantRepository)
        {
            _transactionRepository = transactionRepository;
            _userRepository = userRepository;
            _merchantRepository = merchantRepository;
        }

        //api/transactions
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<TransactionDto>))]
        [ProducesResponseType(400)]
        public IActionResult GetMerchants()
        {
            var transactions = _transactionRepository.GetTransactions();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var transactionDto = new List<TransactionDto>();
            foreach (var transaction in transactions)
            {
                transactionDto.Add(new TransactionDto
                {
                    Id = transaction.Id,
                    Name = transaction.Name,
                    Price = transaction.Price,
                    DateTime = transaction.DateTime                    
                });
            }

            return Ok(transactionDto);
        }

        //api/transactions/transactionId
        [HttpGet("{transactionId}", Name = "GetTransaction")]
        [ProducesResponseType(200, Type = typeof(TransactionDto))]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public IActionResult GetTransactions(int transactionId)
        {
            if (!_transactionRepository.TransactionExist(transactionId))
                return NotFound();

            var transaction = _transactionRepository.GetTransaction(transactionId);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var transactionDto = new List<TransactionDto>();

            transactionDto.Add(new TransactionDto
            {
                Id = transaction.Id,
                Name = transaction.Name,
                Price = transaction.Price,
                DateTime = transaction.DateTime
            });

            return Ok(transactionDto);

        }

        //api/reviews
        [HttpPost]
        [ProducesResponseType(201, Type = typeof(Transaction))]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]//server error
        public IActionResult CreateReview([FromBody]Transaction transactionToCreate)
        {
            if (transactionToCreate == null)
                return BadRequest(ModelState);//400

            if (!_merchantRepository.MerchantExists(transactionToCreate.Merchant.Id))
                ModelState.AddModelError("", "Merchant doesn't exist");

            if (!_userRepository.UserExists(transactionToCreate.User.Id))
                ModelState.AddModelError("", "User doesn't exist");

            if (!ModelState.IsValid)
                return StatusCode(404, ModelState);

            transactionToCreate.Merchant = _merchantRepository.GetMerchant(transactionToCreate.Merchant.Id);
            transactionToCreate.User = _userRepository.GetUser(transactionToCreate.User.Id);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!_transactionRepository.CreateTransaction(transactionToCreate))
            {
                ModelState.AddModelError("", $"Something went wrong saving the review {transactionToCreate.Name}");
                return StatusCode(500, ModelState);
            }

            return CreatedAtRoute("GetTransaction", new { transactionId = transactionToCreate.Id }, transactionToCreate);

        }

        //api/transactions/transactionId
        [HttpPut("{transactionId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]//server error
        public IActionResult UpdateTransaction(int transactionId, [FromBody]Transaction transactionToUpdate)
        {
            if (transactionToUpdate == null)
                return BadRequest(ModelState);//400

            if (transactionId != transactionToUpdate.Id)
                return BadRequest(ModelState);

            if (!_transactionRepository.TransactionExist(transactionId))
                ModelState.AddModelError("", "Transaction doesn't exist");

            if (!_merchantRepository.MerchantExists(transactionToUpdate.Merchant.Id))
                ModelState.AddModelError("", "Merchant doesn't exist");

            if (!_userRepository.UserExists(transactionToUpdate.User.Id))
                ModelState.AddModelError("", "User doesn't exist");

            if (!ModelState.IsValid)
                return StatusCode(404, ModelState);

            transactionToUpdate.Merchant = _merchantRepository.GetMerchant(transactionToUpdate.Merchant.Id);
            transactionToUpdate.User = _userRepository.GetUser(transactionToUpdate.User.Id);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!_transactionRepository.UpdateTransaction(transactionToUpdate))
            {
                ModelState.AddModelError("", $"Something went wrong updating the review {transactionToUpdate.Name}");
                return StatusCode(500, ModelState);
            }

            return NoContent();

        }

        //api/transactions/transactionId
        [HttpDelete("{transactionId}")]
        [ProducesResponseType(204)]//No Context
        [ProducesResponseType(400)]//Bad Request
        [ProducesResponseType(404)]//Not Found
        [ProducesResponseType(500)]//internal server error
        public IActionResult DeleteTransaction(int transactionId)
        {
            if (!_transactionRepository.TransactionExist(transactionId))
                return NotFound(); //not found

            var transactionToDelete = _transactionRepository.GetTransaction(transactionId);

            if (!ModelState.IsValid)
                return BadRequest(ModelState); //bad request

            if (!_transactionRepository.DeleteTransaction(transactionToDelete))
            {
                ModelState.AddModelError("", $"Something went wrong deleting {transactionToDelete.Id}");
                return StatusCode(500, ModelState); //internal server error
            }

            return NoContent(); //204

        }
    }
}
