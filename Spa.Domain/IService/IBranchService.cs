using Spa.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spa.Domain.IService
{
    public interface IBranchService
    {
        Task<List<Branch>> GetAllBranches();
        Task<Branch>GetBranchByID(long? branchID);
        Task<string> GetBranchNameByID(long? branchID);
        Task<Branch> CreateBranch(Branch branchDTO);
        Task UpdateBranch(Branch branchDTO);
        Task DeleteBranch(long? id);
    }
}
