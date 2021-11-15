using in_api.Models;
using in_api.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace in_api.Services
{
    public interface IFinancialDataService
    {
        Task<ResponseObj<FinancialData>> AddData(FinancialData data);

        Task<ResponseObj<List<FinancialData>>> GetAllUserData();

        Task<ResponseObj<FinancialData>> GetData(int id);

        Task<ResponseObj<FinancialData>> EditData(int id, FinancialData data);

        Task<ResponseObj<FinancialData>> DeleteData(int id);

        ResponseObj<FinancialData> MarkDataForEditing(IEnumerable<FinancialData> data);

        ResponseObj<FinancialData> MarkDataForDeletion(IEnumerable<FinancialData> data);

        Task<ResponseObj<FinancialData>> SaveData();

    }
}
