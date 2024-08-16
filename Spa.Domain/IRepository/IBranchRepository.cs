using Spa.Domain.Entities;

namespace Spa.Domain.IRepository
{
    public interface IBranchRepository
    {
        Task<List<Branch>> GetAllBranches();
        Task<Branch> GetBranchByID(long? branchID);
        Task<string> GetBranchNameByID(long? branchID);
        Task<Branch> CreateBranch(Branch branchDTO);
        Task<bool> UpdateBranch(Branch branchDTO);
        Task<bool> DeleteBranch(long? id);
        Task<IEnumerable<Branch>> GetAllBranchByPages(int pageNumber, int pageSize);
        Task<IEnumerable<Branch>> GetAllBranchActiveByPages(int pageNumber, int pageSize);
        Task<IEnumerable<Branch>> GetAllBranchNotActiveByPages(int pageNumber, int pageSize);
        Task<int> GetAllItemBranch();
        Task<bool> ChangeStatusBranch(long id);
    }
}
