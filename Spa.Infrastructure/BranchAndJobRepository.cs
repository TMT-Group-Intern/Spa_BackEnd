using Microsoft.Extensions.Configuration;
using Spa.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Spa.Domain.IRepository;

namespace Spa.Infrastructure
{
    public class BranchAndJobRepository : IBranchAndJobRepository
    {
        private static readonly List<Branch> _branch = new();
        private static readonly List<JobType> _job = new();
        private readonly IConfiguration _config;
        private readonly SpaDbContext _spaDbContext;
        
        public BranchAndJobRepository(SpaDbContext spaDbContext, IConfiguration config)
        {
            _config = config;
            _spaDbContext = spaDbContext;
        }
        
        public async Task<List<Branch>> GetAllBranches()
        {
            var brans = await _spaDbContext.Branches.ToListAsync();
            if (brans is null)
            {
                return null;
            }

            var branDTOs = brans.Select(bran => new Branch
            {
                BranchID = bran.BranchID,
                BranchName = bran.BranchName,
                BranchPhone = bran.BranchPhone,
                BranchAddress = bran.BranchAddress,
            }).OrderBy(b => b.BranchID).ToList();

            return branDTOs;
        }
        
        public async Task<string> GetBranchNameByID(long? branchID)
        {
            var Branch = await _spaDbContext.Branches.FindAsync(branchID);
            return Branch.BranchName;
        }

        public async Task<Branch> GetBranchByID(long? branchID)
        {
            var Branch = await _spaDbContext.Branches.FindAsync(branchID);
            return Branch;
        }

            public async Task<Branch> CreateBranch(Branch branchDTO)
        {
            var newBranch = new Branch()
            {
                BranchName = branchDTO.BranchName,
                BranchAddress = branchDTO.BranchAddress,
                BranchPhone = branchDTO.BranchPhone,
            };
            await _spaDbContext.Branches.AddAsync(newBranch);
            await _spaDbContext.SaveChangesAsync();
            return newBranch;
        }

        public async Task<bool> UpdateBranch(Branch branchDTO)
        {
            var newUpdate = new Branch
            {
                BranchID= branchDTO.BranchID,
                BranchName = branchDTO.BranchName,
                BranchAddress= branchDTO.BranchAddress,
                BranchPhone = branchDTO.BranchPhone,
            };
            var branchUpdate = await _spaDbContext.Branches.FirstOrDefaultAsync(b => b.BranchID == newUpdate.BranchID);
            if (branchUpdate is null) return false;
            {
                branchUpdate.BranchName = newUpdate.BranchName;
                branchUpdate.BranchAddress = newUpdate.BranchAddress;
                branchUpdate.BranchPhone = newUpdate.BranchPhone;
            }
            _spaDbContext.Branches.Update(branchUpdate);
            _spaDbContext.SaveChanges();
            return true;
        }

        public async Task<bool> DeleteBranch(long? BranchID)
        {
                var branch = await _spaDbContext.Branches.FirstOrDefaultAsync(a => a.BranchID == BranchID);
                if (branch is null)
                {
                    return false;
                }
                _spaDbContext.Branches.Remove(branch);
                _spaDbContext.SaveChanges();
                return true;
        }

        public async Task<List<JobType>> GetAllJobs()
        {
            var jobs = await _spaDbContext.JobTypes.ToListAsync();
            if (jobs is null)
            {
                return null;
            }

            var jobDTOs = jobs.Select(job => new JobType
            {
                JobTypeID = job.JobTypeID,
                JobTypeName = job.JobTypeName
            }).OrderBy(j => j.JobTypeID).ToList();

            return jobDTOs;
        }
        
        public async Task<string> GetJobTypeNameByID(long? JobTypeId)
        {
            var Role = await _spaDbContext.JobTypes.FindAsync(JobTypeId);
            return Role.JobTypeName;
        }
        public async Task<JobType> GetJobTypeByID(long? JobTypeId)
        {
            var job = await _spaDbContext.JobTypes.FindAsync(JobTypeId);
            return job;
        }
        public async Task<JobType> CreateJobType(JobType jobDTO)
        {
            var newJob = new JobType()
            {
                JobTypeName = jobDTO.JobTypeName
            };
            await _spaDbContext.JobTypes.AddAsync(newJob);
            await _spaDbContext.SaveChangesAsync();
            return newJob;
        }

        public async Task<bool> UpdateJob(JobType jobDTO)
        {
            var newUpdate = new JobType
            {
                JobTypeID = jobDTO.JobTypeID,
                JobTypeName = jobDTO.JobTypeName,
            };
            var jobUpdate = await _spaDbContext.JobTypes.FirstOrDefaultAsync(b => b.JobTypeID == newUpdate.JobTypeID);
            if (jobUpdate is null) return false;
            {
                jobUpdate.JobTypeName = newUpdate.JobTypeName;
            }
            _spaDbContext.JobTypes.Update(jobUpdate);
            _spaDbContext.SaveChanges();
            return true;
        }

        public async Task<bool> DeleteJob(long? JobTypeID)
        {
            var job = await _spaDbContext.JobTypes.FirstOrDefaultAsync(a => a.JobTypeID == JobTypeID);
            if (job is null)
            {
                return false;
            }
            _spaDbContext.JobTypes.Remove(job);
            _spaDbContext.SaveChanges();
            return true;
        }
    }
}
