using System;
using System.ComponentModel.DataAnnotations;

namespace Data
{
    public class PropertyDocuments
    {
        [Key]
        public int PropertyBlobId { get; set; }
        public int PropertyDocumentTypeId { get; set; }
        public Guid PropertyGuid { get; set; }
        public byte[] FileBlob { get; set; }
        public string FileName { get; set; }
        public int FileLenght { get; set; }
        public string FileType { get; set; }
        public int FileCategoryId { get; set; }
        public DateTime UploadTime { get; set; }

    }
}
