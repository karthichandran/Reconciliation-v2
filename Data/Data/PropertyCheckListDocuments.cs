using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Data
{
    public class PropertyCheckListDocuments
    {
        [Key]
        public int PropertyCheckListBlobId { get; set; }
        public int CheckListPropertyId { get; set; }
        public Guid PropertyGuid { get; set; }
        public byte[] FileBlob { get; set; }
        public string FileName { get; set; }
        public int FileLenght { get; set; }
        public string FileType { get; set; }
        public int FileCategoryId { get; set; }
        public DateTime UploadTime { get; set; }
        public DateTime DueDate {get;set;}
        public DateTime ActualCompletionDate { get; set; }
        public string Remarks { get; set; }
        [NotMapped]
        public int CheckListId { get; set; }
    }
}
