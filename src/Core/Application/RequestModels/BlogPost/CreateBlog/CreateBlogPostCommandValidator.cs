using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Repositories.Interfaces;
using FluentValidation;

namespace Application.RequestModels.BlogPost.CreateBlog
{
    public class CreateBlogPostCommandValidator : AbstractValidator<CreateBlogPostCommand>
    {
        public CreateBlogPostCommandValidator()
        {
            RuleFor(x => x.Title)
                .NotEmpty()
                .WithMessage("Başlık boş olamaz")
                .MaximumLength(200)
                .WithMessage("Başlık en fazla 200 karakter olabilir");

            RuleFor(x => x.Content)
                .NotEmpty()
                .WithMessage("İçerik boş olamaz");

         

            RuleFor(x => x.BlogTagId)
                .NotEmpty()
                .WithMessage("Blog Tag seçilmeli");

            RuleFor(x => x.UserId)
                .NotEmpty()
                .WithMessage("Kullanıcı bilgisi gereklidir");

            RuleFor(x => x.Keywords)
                .MaximumLength(500)
                .WithMessage("Anahtar kelimeler en fazla 500 karakter olabilir");

            RuleFor(x => x.CreatedAt)
                .NotEmpty()
                .WithMessage("Oluşturulma tarihi gereklidir");
        }
    }
}
