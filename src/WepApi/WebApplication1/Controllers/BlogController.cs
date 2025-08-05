using Application.Queries.Blog;
using Application.RequestModels.BlogPost.CreateBlog;
using Application.RequestModels.BlogPost.DeleteBlog;
using Application.RequestModels.BlogPost.UpdateBlog;
using Application.RequestModels.BlogTag.Create;
using Application.RequestModels.BlogTag.Delete;
using Application.RequestModels.BlogTag.Update;
using Application.RequestModels.TimeSlotCommand.Delete;
using Application.RequestModels.User.CreateUser;
using Domain.Models;
using HotelApi.Infrastructure;
using HotelVR.Common.Infrastructure.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HotelApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlogController : BaseController
    {
        private readonly IMediator mediator;
        private readonly IBlogStaticHtmlGenerator staticHtmlGenerator;

        public BlogController(IMediator mediator, IBlogStaticHtmlGenerator staticHtmlGenerator)
        {
            this.mediator = mediator;
            this.staticHtmlGenerator = staticHtmlGenerator;
        }



        [Authorize(Roles = "Admin")]
        [HttpPost]
        [Route("Create-Blog")]
        public async Task<IActionResult> Create([FromBody] CreateBlogPostCommand command)
        {
          
            var guid = await mediator.Send(command);
            await staticHtmlGenerator.GenerateAsync(command);
            return Ok(guid);
        }
        [Authorize(Roles = "Admin")]
        [HttpDelete("delete-Blog/{id}")]
        public async Task<IActionResult> DeleteTimeSlot(Guid id)
        {
            try
            {
                var result = await mediator.Send(new DeleteBlogPostCommand { Id = id });
                return Ok(result);
            }
            catch (DataBaseValidationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [Route("update-blogpost")]
        public async Task<IActionResult> UpdateBlogPost([FromBody] UpdateBlogPostComamnd command)
        {
            try
            {
                if (command.Id == Guid.Empty)
                    return BadRequest("Geçersiz blog ID");

                var updatedId = await mediator.Send(command);
                return Ok(new
                {
                    Message = "Blog yazısı başarıyla güncellendi",
                    BlogPostId = updatedId
                });
            }
            catch (DataBaseValidationException ex)
            {
                return BadRequest(new { Error = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Error = ex.Message });
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [Route("Update-BlogTag")]
        public async Task<IActionResult> UpdateTag([FromBody] UpdateBlogTagCommand command)
        {
            try
            {
                if (command.Id == Guid.Empty)
                    return BadRequest("Geçersiz tag ID");

                var updatedId = await mediator.Send(command);
                return Ok(new
                {
                    Message = "Blog tag başarıyla güncellendi",
                    BlogTagId = updatedId
                });
            }
            catch (DataBaseValidationException ex)
            {
                return BadRequest(new { Error = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Error = ex.Message });
            }
        }


        [Authorize(Roles = "Admin")]
        [HttpPost]
        [Route("Create-BlogTag")]
        public async Task<IActionResult> CreateTag([FromBody] CreateBlogTagCommand command)
        {
           
            var guid = await mediator.Send(command);
            return Ok(guid);
        }
        [Authorize(Roles = "Admin")]
        [HttpDelete("delete-Tag/{id}")]
        public async Task<IActionResult> DeleteBlogTag(Guid id)
        {
            try
            {
                var result = await mediator.Send(new BlogTagDeleteCommand { Id = id });
                return Ok(result);
            }
            catch (DataBaseValidationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
        
        [HttpGet]
        [Route("MainPageBlog")]
        public async Task<IActionResult> GetMainPageEntries(int page, int pageSize)
        {
            var entries = await mediator.Send(new GetPaginatedBlogPostsQuery(UserId, page, pageSize));

            return Ok(entries);

        }
        
        [HttpGet("{slug}")]
        public async Task<IActionResult> GetBySlug(string slug)
        {
            var result = await mediator.Send(new GetBlogPostDetailBySlugQuery(slug));
            return Ok(result);
        }
        [HttpGet]
        [Route("GetAllBlogTag")]
        public async Task<IActionResult> GetAllBlogTag()
        {
            var result = await mediator.Send(new GetAllBlogPostTaqQuery());
            return Ok(result);
        }

        [HttpGet]
        [Route("GetLastPost")]
        public async Task<IActionResult> getLastPost(int page,int pageSize)
        {
            
                var result = await mediator.Send(new GetLastPostQuery(page, pageSize));
                return Ok(result);
            
        }

        [HttpGet]
        [Route("GetBlogPostByTagId/{Id}")]
        public async Task<IActionResult> GetBlogPostByTagId(Guid Id)
        {
            var result = await mediator.Send(new GetBlogPostTagIdQuery(Id));
            return Ok(result);
        }


    }
}
