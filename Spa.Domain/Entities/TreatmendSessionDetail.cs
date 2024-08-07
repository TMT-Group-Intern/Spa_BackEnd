namespace Spa.Domain.Entities
{
    public class TreatmendSessionDetail
    {
        public long? TreatmendDetailID {  get; set; }
        public long SessionID { get; set; }
        public long ServiceID { get; set; } // số dịch vụ chọn trong buổi
        public TreatmentSession? TreatmentSession { get; set; }

        public ServiceEntity? Service { get; set; }
    }
}