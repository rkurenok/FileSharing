using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FileSharing.Models
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string FilesExtension { get; set; }

        public ICollection<File> Files { get; set; }

        public Category()
        {
            Files = new List<File>();
        }
    }
}