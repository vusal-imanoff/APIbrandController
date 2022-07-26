using FluentValidation;
using P225FirstApi.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace P225FirstApi.DTOs.BrandDTOs
{
    public class BrandPostDto
    {
        public string Name { get; set; }
    }

    public class BrandPostDtoValidator : AbstractValidator<BrandPostDto>
    {
        private readonly AppDbContext _context;

        public BrandPostDtoValidator(AppDbContext context)
        {
            _context = context;

            RuleFor(b=>b.Name).MaximumLength(255).MinimumLength(4).NotEmpty().WithMessage("Required");

            RuleFor(x => x).Custom((b, ctx) =>
            {
               

                if (b.Name != null && _context.Brands.Any(x => x.Name.ToLower() == b.Name.Trim().ToLower()))
                {
                    ctx.AddFailure(nameof(b.Name), "Name Alreade Exists");
                }
            });
        }
    }
}
