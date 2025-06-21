using BBPSApi.Dto;
using BBPSApi.Interface;
using Dapper;
using Dapper.Oracle;
using Microsoft.AspNetCore.Connections.Features;
using Microsoft.AspNetCore.Http.HttpResults;
using Newtonsoft.Json;
using Oracle.ManagedDataAccess.Client;
using Oracle.ManagedDataAccess.Types;
using System.Data;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace BBPSApi.Service
{
    public class DataService : IDataService
    {
        private readonly IDbConnection _dbConnection;

        public DataService(IDbConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }
        public async Task<dynamic> GetLoanDetails(string flag, string param)
        {
            string[] emptyArray = new string[0];
            OracleRefCursor result = null;
            var dataList = new List<FetchLoanDetails>();
            var respList = new List<Response>();
            var procedureName = "proc_axis_bbps_api";
            var parameters = new OracleDynamicParameters();
            parameters.Add("p_flag", flag, OracleMappingType.NVarchar2, ParameterDirection.Input);
            parameters.Add("p_pageval", param, OracleMappingType.NVarchar2, ParameterDirection.Input);
            parameters.Add("p_parval1", "", OracleMappingType.NVarchar2, ParameterDirection.Input);
            parameters.Add("qry_result", result, OracleMappingType.RefCursor, direction: ParameterDirection.Output);
            parameters.BindByName = true;
            var res = await _dbConnection.QueryAsync(procedureName, parameters, commandType: CommandType.StoredProcedure);


            if (res != null && res.Any())
            {
                //Response resp = new Response { Status = "SUCCESS", ErrorCode = "00" };
                //respList.Add(resp);
                foreach (var item in res)
                {
                    var data = new FetchLoanDetails
                    {
                        status = "SUCCESS",
                        errorCode = "00",
                        customerName = item.CUSTOMERNAME,
                        amountDue = item.AMOUNTDUE,
                        billDate = item.BILLDATE,
                        dueDate = item.DUEDATE,
                        billNumber = item.BILLNUMBER,
                        billPeriod = item.BILLPERIOD,
                        additionalInfo = new AdditionalInfo
                        {
                            dueAmount = item.DUEAMOUNT,
                            penalInterest = item.PENALINTEREST,
                            otherCharges = item.OTHERCHARGES,
                            EmiAmount = item.EMIAMOUNT
                        }



                    };

                    dataList.Add(data);
                }
                //var splitData = new List<FetchLoanDetails>(dataList);
                return (dataList);
            }
            else
            {
                var data = new Response
                {
                    Status = "FAILURE",
                    ErrorCode = "01"
                };
                respList.Add(data);

                return respList;
            }
        }
        public async Task<dynamic> PostPayment(string flag, string param, string param1)
        {
            var dataList = new List<PostResponse>();

            OracleRefCursor result = null;
            var procedureName = "proc_axis_bbps_api";
            var parameters = new OracleDynamicParameters();
            try
            {
                var message = ""; var ackno = "";
                parameters.Add("p_flag", flag, OracleMappingType.NVarchar2, ParameterDirection.Input);
                parameters.Add("p_pageval", param, OracleMappingType.NVarchar2, ParameterDirection.Input);
                parameters.Add("p_parval1", param1, OracleMappingType.NVarchar2, ParameterDirection.Input);
                parameters.Add("qry_result", result, OracleMappingType.RefCursor, direction: ParameterDirection.Output);
                parameters.BindByName = true;
                var res = await _dbConnection.QueryAsync(procedureName, parameters, commandType: CommandType.StoredProcedure);
                if (res != null && res.Any())
                {
                    foreach (var item in res)
                    {
                        message = item.MSG;
                        ackno = item.ACK_NUMBER;
                    }
                    if (message == "111")
                    {
                        var data = new PostResponse
                        {
                            status = "SUCCESS",
                            acknowledgementId = ackno
                        };
                        dataList.Add(data);
                    }
                    else if (message == "777")
                    {
                        var data = new PostResponse
                        {
                            status = "DUPLICATE",
                            acknowledgementId = ""
                        };
                        dataList.Add(data);
                    }
                    else
                    {

                        var data = new PostResponse
                        {
                            status = "FAILURE",
                            acknowledgementId = ""
                        };
                        dataList.Add(data);
                    }
                }
                else
                {

                    var data = new PostResponse
                    {
                        status = "FAILURE",
                        acknowledgementId = ""
                    };
                    dataList.Add(data);
                }
            }
            catch (Exception)
            {
                var data = new PostResponse
                {
                    status = "FAILURE",
                    acknowledgementId = ""
                };
                dataList.Add(data);
            }


            return (dataList);
        }


    }
}
