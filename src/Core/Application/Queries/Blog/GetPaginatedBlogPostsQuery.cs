using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Models;
using Application.Models.Page;
using Domain.Models;
using MediatR;

namespace Application.Queries.Blog
{
    public class GetPaginatedBlogPostsQuery : BasePagedQuery,IRequest<PagedViewModel<BlogPostDto>>
    {
        public GetPaginatedBlogPostsQuery(Guid? UserId, int page, int pageSize) : base(page, pageSize)
        {
            UserId = userId;
        }

        public Guid? userId { get; set; }
    }
}
