using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Web;

namespace FileSharing.Models
{
    public class File
    {
        //public File(string name, long sizeInBytes, Stream inputStream)
        //{
        //    Name = name;
        //    SizeInBytes = sizeInBytes;
        //    InputStream = inputStream;
        //}

        public int Id { get; set; }

        [DataType(DataType.Text)]
        [Display(Name = "Название")]
        public string Name { get; set; }
        public string OriginalName { get; set; }

        [DataType(DataType.Text)]
        [Display(Name = "Размер")]
        public long SizeInBytes { get; set; }
        //public Stream InputStream { get; set; }

        [DataType(DataType.DateTime)]
        [Display(Name = "Дата и время загрузки")]
        public DateTime Date { get; set; }

        public int? UserId { get; set; }
        public User User { get; set; }

        public int AccessId { get; set; }
        public FileAccess Access { get; set; }

        public int FileRententionPeriodId { get; set; }
        public FileRetentionPeriod FileRetentionPeriod { get; set; }

        public int? FileUniqueKeyId { get; set; }
        public FileUniqueKey FileUniqueKey { get; set; }

        public int CategoryId { get; set; }
        public Category Category { get; set; }
    }

    public class FileAccess
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class FileRetentionPeriod
    {
        public int Id { get; set; }
        public int Value { get; set; }
    }

    public class FileUniqueKey
    {
        public int Id { get; set; }
        public string UniqueKey { get; set; }
        public int FileId { get; set; }
    }
}