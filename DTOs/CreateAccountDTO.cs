using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Blog.DTOs
{
    public class CreateAccountDTO
    {

        [Required(ErrorMessage = "{0} é um campo obrigatório")]
        public string Name { get; set; }

        [Required(ErrorMessage = "{0} é um campo obrigatório")]
        public string Email { get; set; }

        [Required(ErrorMessage = "{0} é um campo obrigatório")]
        public string Password { get; set; }
    }
}