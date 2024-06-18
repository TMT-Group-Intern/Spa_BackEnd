namespace Spa.Domain.Entities
{
    public class CustomerPhoto
    {
        public long PhotoID { get; set; }
        public long CustomerID { get; set; }
        public string PhotoPath { get; set; }
        public DateTime UploadedAt { get; set; } = DateTime.Now;

        public Customer Customer { get; set; }
    }
}