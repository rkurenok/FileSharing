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

        [DataType(DataType.Text)]
        [Display(Name = "Размер")]
        public long SizeInBytes { get; set; }
        //public Stream InputStream { get; set; }

        public int? UserId { get; set; }
        public User User { get; set; }
    }
}