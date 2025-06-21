using System.ComponentModel.DataAnnotations;

namespace BBPSApi.Dto
{
    public class PostPayment
    {
   
        [Required]
        public string? loan_number { get; set; }
        [Required]

        public string? amountPaid { get; set; }
        [Required]

        public string? transactionId { get; set; }
        public string? paymentMode { get; set; }
        //public DateTime? paymentDate { get; set; }
        [Required]
        public string? billNumber { get; set; }

    }
}
