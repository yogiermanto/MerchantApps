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
    public class MerchantsController : Controller
    {
        private IMerchantRepository _merchantRepository;

        public MerchantsController(IMerchantRepository merchantRepository)
        {
            _merchantRepository = merchantRepository;
        }

        //api/merchants
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<MerchantDto>))]
        [ProducesResponseType(400)]
        public IActionResult GetMerchants()
        {
            var merchants = _merchantRepository.GetMerchants();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var merchantDto = new List<MerchantDto>();
            foreach (var merchant in merchants)
            {
                merchantDto.Add(new MerchantDto
                {
                    Id = merchant.Id,
                    Name = merchant.Name
                });
            }

            return Ok(merchantDto);
        }
                
        //api/merchants/merchantId
        [HttpGet("{merchantId}" , Name = "GetMerchant")]
        [ProducesResponseType(200, Type = typeof(MerchantDto))]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public IActionResult GetMerchant(int merchantId)
        {
            if (!_merchantRepository.MerchantExists(merchantId))
                return NotFound();

            var merchant = _merchantRepository.GetMerchant(merchantId);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var merchantDto = new List<MerchantDto>();

            merchantDto.Add(new MerchantDto
            {
                Id = merchant.Id,
                Name = merchant.Name
            });

            return Ok(merchantDto);

        }

        //api/merchants/transactionId/merchant
        [HttpGet("{transactionId}/merchant")]
        [ProducesResponseType(200, Type = typeof(MerchantDto))]
        [ProducesResponseType(400)]
        public IActionResult GetMerchantOfATransaction(int transactionId)
        {
            //TO DO - Validate transactionId
            var merchant = _merchantRepository.GetMerchantOfATransaction(transactionId);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var merchantDto = new MerchantDto()
            {
                Id = merchant.Id,
                Name = merchant.Name
            };

            return Ok(merchantDto);
        }

        //api/merchants/merchantId/transactions
        [HttpGet("{merchantId}/transactions")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<TransactionDto>))]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public IActionResult GetTransactionsByMerchant(int merchantId)
        {
            if (!_merchantRepository.MerchantExists(merchantId))
                return NotFound();

            var transactions = _merchantRepository.GeTransactionsByMerchant(merchantId);

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

        //api/merchants
        [HttpPost]
        [ProducesResponseType(201, Type = typeof(Merchant))]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public IActionResult CreateUser([FromBody]Merchant merchantToCreate)
        {
            if (merchantToCreate == null)
                return BadRequest(ModelState);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!_merchantRepository.CreateMerchant(merchantToCreate))
            {
                ModelState.AddModelError("", $"Something went wrong saving {merchantToCreate.Name}");
                return StatusCode(500, ModelState);
            }

            return CreatedAtRoute("GetMerchant", new { merchantId = merchantToCreate.Id }, merchantToCreate);
        }

        //api/merchants/merchantId
        [HttpPut("{merchantId}")]
        [ProducesResponseType(204)]//No Context
        [ProducesResponseType(400)]//Bad Request
        [ProducesResponseType(404)]//Not Found
        [ProducesResponseType(422)]//entity error
        [ProducesResponseType(500)]//internal server error
        public IActionResult UpdateUser(int merchantId, [FromBody]Merchant updatedMerchantInfo)
        {
            if (updatedMerchantInfo == null)
                return BadRequest(ModelState);

            if (merchantId != updatedMerchantInfo.Id)
                return BadRequest(ModelState);

            if (!_merchantRepository.MerchantExists(merchantId))
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!_merchantRepository.UpdateMerchant(updatedMerchantInfo))
            {
                ModelState.AddModelError("", $"Something went wrong updating {updatedMerchantInfo.Name}");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }

        //api/merchants/merchantId
        [HttpDelete("{merchantId}")]
        [ProducesResponseType(204)]//No Context
        [ProducesResponseType(400)]//Bad Request
        [ProducesResponseType(404)]//Not Found
        [ProducesResponseType(500)]//internal server error
        public IActionResult DeleteMerchant(int merchantId)
        {
            if (!_merchantRepository.MerchantExists(merchantId))
                return NotFound(); //not found

            var merchantToDelete = _merchantRepository.GetMerchant(merchantId);
            //var transactionsToDelete = _userRepository.GetTransactionsByUser(userId);

            if (!ModelState.IsValid)
                return BadRequest(ModelState); //bad request

            if (!_merchantRepository.DeleteMerchant(merchantToDelete))
            {
                ModelState.AddModelError("", $"Something went wrong deleting {merchantToDelete.Name}");
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
