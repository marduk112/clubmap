using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ClubMap.DbModels
{
    public class Image
    {
        public static readonly int MaxSize = 1024 * 1024;
        public int Id { get; set; }

        [Required]
        public byte[] Bytes { get; set; }

        [Required]
        public string ContentType { get; set; }

        [Required]
        public string FileName { get; set; }

        public bool IsValid()
        {
            return
                Bytes != null &&
                Bytes.Length > 0 &&
                !string.IsNullOrWhiteSpace(ContentType) &&
                !ExceedsSize();
        }

        public bool ExceedsSize()
        {
            return
                Bytes != null &&
                Bytes.Length > MaxSize;
        }
    }
}