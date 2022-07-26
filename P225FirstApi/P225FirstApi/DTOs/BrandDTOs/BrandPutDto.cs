using FluentValidation;
using P225FirstApi.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace P225FirstApi.DTOs.BrandDTOs
{
    public class BrandPutDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class BrandPutDtoValidator:AbstractValidator<BrandPutDto>
    {
        private readonly AppDbContext _context;

        public BrandPutDtoValidator(AppDbContext context)
        {
            _context = context;

            RuleFor(b=>b.Name).MaximumLength(255).MinimumLength(4).NotEmpty().WithMessage("Required");

            RuleFor(x => x).Custom((x, ctx) =>
            {
                if (!_context.Brands.Any(c => c.Id == x.Id))
                {
                    ctx.AddFailure(nameof(x.Id), "Id Is InCorrect");
                }

                if (x.Name != null && _context.Brands.Any(c => c.Id != x.Id && c.Name.ToLower() == x.Name.Trim().ToLower()))
                {
                    ctx.AddFailure(nameof(x.Name), "Name Alreade Exists");
                }
            });
        }
    }
}
