using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using P225FirstApi.Data;
using P225FirstApi.Data.Entities;
using P225FirstApi.DTOs.BrandDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace P225FirstApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BrandsController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public BrandsController(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet("")]
        public async Task<IActionResult> Get()
        {

            List<BrandListDto> brandListDtos = _mapper.Map<List<BrandListDto>>(await _context.Brands.Where(b => !b.IsDeleted).ToListAsync());
            return Ok(brandListDtos);
        }


        [HttpPost("")]
        public async Task<IActionResult> Create(BrandPostDto brandPostDto)
        {
            Brand brand = _mapper.Map<Brand>(brandPostDto);

            await _context.Brands.AddAsync(brand);
            await _context.SaveChangesAsync();

            BrandListDto brandListDto = _mapper.Map<BrandListDto>(brand);

            return StatusCode(201, brandListDto);

        }

        [HttpPut("{id?}")]
        public async Task<IActionResult> Update(int? id, BrandPutDto brandPutDto)
        {
            if (id==null)
            {
                return BadRequest("Id is Required");
            }

            if (brandPutDto.Id!=id)
            {
                return BadRequest("id is not matched");
            }

            Brand dbbrand = await _context.Brands.FirstOrDefaultAsync(b => !b.IsDeleted && b.Id == id);
            if (dbbrand==null)
            {
                return NotFound("Id is incorrect");
            }

            dbbrand.Name = brandPutDto.Name.Trim();
            dbbrand.UpdatedAt = DateTime.UtcNow.AddHours(4);
            await _context.SaveChangesAsync();

            return NoContent();
        }


        [HttpDelete("{id?}")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id==null)
            {
                return BadRequest("Id Is Requeired");
            }
            Brand brand = await _context.Brands.FirstOrDefaultAsync(b => !b.IsDeleted && b.Id == id);

            if (brand==null)
            {
                return BadRequest("Id is Incorrect");
            }

            brand.IsDeleted = true;
            brand.DeletedAt = DateTime.UtcNow.AddHours(4);

            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
