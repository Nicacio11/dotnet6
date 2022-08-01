using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blog.DTOs
{
    public class CreateAccountResultDTO : ResultDTO<CreateAccountDTO>
    {
        public CreateAccountResultDTO(CreateAccountDTO data) : base(data)
        {
        }

        public CreateAccountResultDTO(List<string> errors) : base(errors)
        {
        }

        public CreateAccountResultDTO(string error) : base(error)
        {
        }

        public CreateAccountResultDTO(CreateAccountDTO data, List<string> errors) : base(data, errors)
        {
        }
    }

        public class LoginResultDTO : ResultDTO<LoginDTO>
    {
        public LoginResultDTO(LoginDTO data) : base(data)
        {
        }

        public LoginResultDTO(List<string> errors) : base(errors)
        {
        }

        public LoginResultDTO(string error) : base(error)
        {
        }

        public LoginResultDTO(LoginDTO data, List<string> errors) : base(data, errors)
        {
        }
    }
}