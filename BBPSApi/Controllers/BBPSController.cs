using BBPSApi.Data;
using BBPSApi.Dto;
using BBPSApi.Interface;
using BBPSApi.Model;
using BBPSApi.Service;
using Dapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Diagnostics;
using NGLMoSy.Data;
using System.Data;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using static System.Runtime.InteropServices.JavaScript.JSType;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BBPSApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class BBPSApi : ControllerBase
    {
        private IDbConnection _dbConnection;
        private readonly ApplicationDbContext _dbContext;
        private readonly IDataService _dataService;

        public BBPSApi(IDbConnection dbConnection, ApplicationDbContext dbContext, IDataService dataService)
        {
            _dbConnection = dbConnection;
            _dbContext = dbContext;
            _dataService = dataService;
        }
        // GET: api/<TestController>
        [HttpPost("GetData")]
        public async Task<ActionResult> Get(InputParameter inputParameter)
        {
            try
            {
                var decriptLoanId = "";
                var key = Encoding.UTF8.GetBytes("kljsdkkdlo4454GG00155sajuklmbkdl");
                var iv = Encoding.UTF8.GetBytes("HR$2pIjHR$2pIj12");

                using (Aes myAes = Aes.Create())
                {
                    decriptLoanId = CommonEncryptDecrypt.DecryptStringFromBytes_Aes(Convert.FromBase64String(inputParameter.loan_number), key, iv);
                }

                if (inputParameter == null)
                {
                    return NotFound(inputParameter);
                }
                var data = await _dataService.GetLoanDetails("GetData", decriptLoanId);
                return Ok(data);
            }
            catch (Exception)
            {

                return Ok(new
                {
                    status = "FAILURE",
                    errorCode = "03"
                });
            }



        }


        [HttpPost("Payment")]
        public async Task<ActionResult> PostPayment(PostPayment postPayment)
        {
            try
            {

                var decriptLoanId = "";
                var decriptAmount = "";

                if (postPayment == null)
                {
                    return NotFound();
                }

                var key = Encoding.UTF8.GetBytes("kljsdkkdlo4454GG00155sajuklmbkdl");
                var iv = Encoding.UTF8.GetBytes("HR$2pIjHR$2pIj12");

                using (Aes myAes = Aes.Create())
                {
                    decriptLoanId = CommonEncryptDecrypt.DecryptStringFromBytes_Aes(Convert.FromBase64String(postPayment.loan_number), key, iv);
                    decriptAmount = CommonEncryptDecrypt.DecryptStringFromBytes_Aes(Convert.FromBase64String(postPayment.amountPaid), key, iv);
                }

                var payments = new List<PostPayment>();
                var datas = decriptAmount + "^" + postPayment.transactionId + "^" + postPayment.paymentMode + "^" + postPayment.billNumber;
                var data = await _dataService.PostPayment("PostPayment", decriptLoanId, datas);
                return Ok(data);
            }
            catch (Exception)
            {
                return Ok(new
                {
                    status = "FAILURE",
                    acknowledgementId = "03"
                });

            }
        }

    }

    public class InputParameter
    {
        public string? loan_number { get; set; }
    }
}
