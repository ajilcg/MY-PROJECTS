namespace BBPSApi.Interface
{
    public interface IDataService
    {
        public Task<dynamic> GetLoanDetails(string flag, string param);
        //public Task<dynamic> PostPayment(string flag, string param,string param1,string param2,string param3,string param4,string param5);
        public Task<dynamic> PostPayment(string flag, string param, string param1);
    }
}
