using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Microsoft.AspNetCore.Http;

namespace BLL.DTO
{
    public class FileUploadDto
    {
        [Required]
        public IFormFile FormFile { get; set; }
    }
}
