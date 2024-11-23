using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PaymentGateway.Helpers;
using PaymentGateway.Models;

namespace PaymentGateway.Controllers
{
   // [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class TransactionController : Controller
    {
        [HttpGet("encryption-key")]
        public IActionResult GetEncryptionKey()
        {
            var key = EncryptionHelper.GenerateKey();
            return Ok(new { EncryptionKey = key });
        }

        [HttpPost("process-transaction")]
        public IActionResult ProcessTransaction([FromBody] string encryptedData, [FromHeader] string encryptionKey)
        {
            try
            {
                var decryptedData = EncryptionHelper.Decrypt(encryptedData, encryptionKey);
                var transaction = JsonConvert.DeserializeObject<TransactionRequest>(decryptedData);

                var response = new TransactionResponse
                {
                    ResponseCode = "00",
                    Message = "Success",
                    ApprovalCode = new Random().Next(100000, 999999).ToString(),
                    DateTime = DateTime.Now.ToString("yyyyMMddHHmm")
                };

                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Error = ex.Message });
            }
        }
    }
}
