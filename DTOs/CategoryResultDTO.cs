using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blog.Models;

namespace Blog.DTOs
{
    public class CategoryResultDTO : ResultDTO<Category>
    {
        public CategoryResultDTO(Category data) : base(data)
        {
        }

        public CategoryResultDTO(List<string> errors) : base(errors)
        {
        }

        public CategoryResultDTO(string error) : base(error)
        {
        }

        public CategoryResultDTO(Category data, List<string> errors) : base(data, errors)
        {
        }
    }

    public class ListCategoryResultDTO : ResultDTO<List<Category>>
    {
        public ListCategoryResultDTO(List<Category> data) : base(data)
        {
        }

        public ListCategoryResultDTO(List<string> errors) : base(errors)
        {
        }

        public ListCategoryResultDTO(string error) : base(error)
        {
        }

        public ListCategoryResultDTO(List<Category> data, List<string> errors) : base(data, errors)
        {
        }
    }
}