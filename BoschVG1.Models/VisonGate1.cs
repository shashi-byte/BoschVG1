using NPoco;

namespace BoschVG1.Models
{
    [TableName("vg1")]
    public class VisonGate1
    {
        public int? job_id { get; set; }
        public int box_id { get; set; }
        public int part_nu { get; set; }
        public int quantity { get; set; }
        public int status { get; set; }
        public int shift { get; set; }
        public DateTime timestamp { get; set; }

    }
}
