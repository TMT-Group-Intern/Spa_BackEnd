using Spa.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spa.Domain.IRepository
{
    public interface IBranchAndJobRepository
    {
        Task<List<Branch>> GetAllBranches();
        Task<Branch> GetBranchByID(long? branchID);
        Task<Branch> CreateBranch(Branch branchDTO);
        Task<bool> UpdateBranch(Branch branchDTO);
        Task<bool> DeleteBranch(long? id);
        Task<List<JobType>> GetAllJobs();
        Task<JobType> GetJobTypeByID(long? jobTypeID);
        Task<JobType> CreateJobType(JobType jobDTO);
        Task<bool> UpdateJob(JobType jobDTO);
        Task<bool> DeleteJob(long? id);
    }
}
