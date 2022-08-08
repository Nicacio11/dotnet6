using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Blog.DTOs.Accounts;
public class UploadImageDTO
{
    [Required(ErrorMessage = "{0} é um campo obrigatório")]
    public string Base64Image { get; set; }
    
}