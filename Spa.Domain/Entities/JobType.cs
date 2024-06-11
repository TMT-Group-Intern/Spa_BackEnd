namespace Spa.Domain.Entities
{
    public class JobType
    {
        public long JobTypeID { get; set; }
        public string JobTypeName { get; set; }
        public ICollection<Employee> Employees { get; set; }
    }
}