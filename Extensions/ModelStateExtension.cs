using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Blog.Extensions
{
    public  static class ModelStateExtension
    {

        public static List<string> GetErrors(this ModelStateDictionary dictionary)
        {
            return dictionary.Values.SelectMany(x => x.Errors.Select(x => x.ErrorMessage)).ToList();
        }
        
    }
}