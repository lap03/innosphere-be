using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Models.ResumeModels
{
    public class ResumeModel
    {
        public int Id { get; set; }
        public int WorkerId { get; set; }
        public string Title { get; set; }
        public string UrlCvs { get; set; }
        public string? FileType { get; set; }
        public int? FileSize { get; set; }
    }
}
