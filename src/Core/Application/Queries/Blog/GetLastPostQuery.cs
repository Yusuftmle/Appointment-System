using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Extension;
using Application.Models;
using Application.Models.Page;
using Application.Queries.Blog;
using MediatR;

namespace Application.Queries.Blog
{
    public  class GetLastPostQuery : BasePagedQuery, IRequest<PagedViewModel<BlogPostListItemDto>>
    {
      
        public GetLastPostQuery(int page, int pageSize) : base(page, pageSize)
        {
            
        }
    }
}
