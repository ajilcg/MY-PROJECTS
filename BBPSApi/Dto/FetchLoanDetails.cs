namespace BBPSApi.Dto
{
    public class FetchLoanDetails
    {
        public string? status { get; set; }
        public string? errorCode { get; set; }
        public string? customerName { get; set; }
        public decimal? amountDue { get; set; }
        public string? billDate { get; set; }
        public string? dueDate { get; set; }
        public string? billNumber { get; set; }
        public string? billPeriod { get; set; }
        public AdditionalInfo? additionalInfo { get; set; }

    }
    public class AdditionalInfo
    {
        public decimal? dueAmount { get; set; }//Overdue Amount
        public decimal? penalInterest { get; set; }//Overdue Interest
        public decimal? otherCharges { get; set; }//ChkRtn + legal
        public decimal? EmiAmount { get; set; }
    }
}
