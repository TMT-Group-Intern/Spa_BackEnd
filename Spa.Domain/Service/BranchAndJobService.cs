using Microsoft.AspNetCore.Identity;
using Spa.Domain.Entities;
using Spa.Domain.IRepository;
using Spa.Domain.IService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spa.Domain.Service
{
    public class BranchAndJobService:IBranchAndJobService
    {
        private readonly IBranchAndJobRepository _bnjRepository;
        public BranchAndJobService(IBranchAndJobRepository bnjRepository)
        {
            _bnjRepository = bnjRepository;
        }

        public async Task<List<Branch>> GetAllBranches()
        {
            var branches = await _bnjRepository.GetAllBranches();
            return branches;
        }

        public async Task<Branch> GetBranchByID(long? branchID)
        {
            var branch = await _bnjRepository.GetBranchByID(branchID);
            return branch;
        }

        public async Task<Branch> CreateBranch(Branch branchDTO)
        {
            var newBranch = await _bnjRepository.CreateBranch(branchDTO);
            return newBranch;
        }
        public async Task UpdateBranch(Branch branchDTO)
        {
            await _bnjRepository.UpdateBranch(branchDTO);
        }

        public async Task DeleteBranch(long? id)
        {
            await _bnjRepository.DeleteBranch(id);
        }

        public async Task<List<JobType>> GetAllJobs()
        {
            var jobs = await _bnjRepository.GetAllJobs();
            return jobs;
        }

        public async Task<JobType> GetJobTypeByID(long? jobTypeID)
        {
            var job = await _bnjRepository.GetJobTypeByID(jobTypeID);
            return job;
        }

        public async Task<JobType> CreateJobType(JobType jobDTO)
        {
            var newJob = await _bnjRepository.CreateJobType(jobDTO);
            return newJob;
        }
        public async Task UpdateJob(JobType jobDTO)
        {
            await _bnjRepository.UpdateJob(jobDTO);
        }

        public async Task DeleteJob(long? id)
        {
            await _bnjRepository.DeleteJob(id);
        }
    }
}
