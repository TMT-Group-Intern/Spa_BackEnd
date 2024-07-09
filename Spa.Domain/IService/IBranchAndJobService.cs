using Spa.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spa.Domain.IService
{
    public interface IBranchAndJobService
    {
        Task<List<Branch>> GetAllBranches();
        Task<Branch>GetBranchByID(long? branchID);
        Task<Branch> CreateBranch(Branch branchDTO);
        Task UpdateBranch(Branch branchDTO);
        Task DeleteBranch(long? id);
        Task<List<JobType>> GetAllJobs();
        Task<JobType> GetJobTypeByID(long? jobTypeID);
        Task<JobType> CreateJobType(JobType jobDTO);
        Task UpdateJob(JobType jobDTO);
        Task DeleteJob(long? id);
    }
}
