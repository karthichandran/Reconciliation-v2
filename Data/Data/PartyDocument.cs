using System;
using System.ComponentModel.DataAnnotations;

namespace Data { 
    public class PartyDocuments
    {
        [Key]
        public int PartyBlobId { get; set; }
        public Guid PartyGuid { get; set; }
        public byte[] FileBlob { get; set; }
        public string FileName { get; set; }
        public int FileLength { get; set; }
        public string FileType { get; set; }
        public int FileCategoryId { get; set; }
        public DateTime UploadTime { get; set; }
      
    }
}
