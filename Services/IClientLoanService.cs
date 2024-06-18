using HomeBanking.DTOs;
using HomeBanking.Models;

namespace HomeBanking.Services
{
    public interface IClientLoanService
    {
        void AsignLoanToClient(LoanApplicationDTO loanApplicationDTO, Client Client);
    }
}
