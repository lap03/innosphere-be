using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Models.JobApplicationModels
{
    public class JobApplicationModel
    {
        public int Id { get; set; }
        public int JobPostingId { get; set; }
        public int? ResumeId { get; set; }
        public DateTime AppliedAt { get; set; }
        public string Status { get; set; }
        public string CoverNote { get; set; }

        // Các trường thông tin bổ sung có thể mở rộng nếu cần
        public string JobTitle { get; set; }
        public string WorkerName { get; set; }
        public string ResumeTitle { get; set; }
    }
}
