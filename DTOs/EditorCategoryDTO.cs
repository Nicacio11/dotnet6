using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Blog.DTOs
{
    public class EditorCategoryDTO
    {
        [Required(ErrorMessage = "{0} é um campo obrigatório")]
        [MaxLength(40, ErrorMessage = "{0} não pode ter a quantidade de caracteres maior que 40")]
        [MinLength(3, ErrorMessage = "{0} não pode ter a quantidade de caracteres menor que 3")]
        public string Name { get; set; } = string.Empty;
        [Required(ErrorMessage = "{0} é um campo obrigatório")]
        public string Slug { get; set; } = string.Empty;
    }
}