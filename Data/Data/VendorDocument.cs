using System;
using System.ComponentModel.DataAnnotations;

namespace Data
{
   public class VendorDocuments
    {
        [Key]
        public int VendorBlobId { get; set; }
        public Guid VendorGuid { get; set; }
        public byte[] FileBlob { get; set; }
        public string FileName { get; set; }
        public int FileLength { get; set; }
        public string FileType { get; set; }
        public int FileCategoryId { get; set; }
        public DateTime? UploadTime { get; set; }
    }
}
