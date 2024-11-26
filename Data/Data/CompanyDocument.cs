using System;
using System.ComponentModel.DataAnnotations;

namespace Data
{
    public class CompanyDocuments
    {
        [Key]
        public int CompanyBlobId { get; set; }
        public Guid CompanyGuid { get; set; }
        public byte[] FileBlob { get; set; }
        public string FileName { get; set; }
        public int FileLength { get; set; }
        public string FileType { get; set; }
        public int FileCategoryId { get; set; }
        public DateTime? UploadTime { get; set; }
    }
}
