
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProductAPI.Data;
using ProductAPI.Models;

namespace ProductAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PrimeController : ControllerBase
    {
        private readonly AppDbContext _context;
        public PrimeController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet("generate")]
        public async Task<IActionResult> GeneratePrimes(int start, int end)
        {
            //  start and end range as non-negative integers
            if(start < 0 || end < 0)
            {
                return BadRequest(new 
                {
                    Message = "Start and End range must be non-negative integers."
                });
            }
           
            // incorrect parameters check
           if(start > end)
            {
                return BadRequest(new 
                {
                    Message = "Start range must be less than the End range."
                });
            }
           

            var primes = new List<int>();

            for (int i = start; i <= end; i++)
            {
                if (IsPrime(i))
                    primes.Add(i);
            }
            
           if (!primes.Any())
    {
        return Ok(new
        {
            range = $"{start}-{end}",
            message = "No primes found in the given range.",
            primes = new List<int>()
        });
    }

           // Save result to DB
            var result = new PrimeResult
            {
                StartRange = start,
                EndRange = end,
                Primes = string.Join(",", primes)
            };
            
        try
     {
             _context.PrimeResults.Add(result);
               
             await _context.SaveChangesAsync();


        }
         catch (Exception ex)
        {
            return StatusCode(500, new { Message = "An error occurred while saving the result.", Error = ex.Message });
        }

 return Ok(new { range = $"{start}-{end}", primes});
        }
           [HttpGet("history")]
        public async Task<IActionResult> GetHistory()
        {
            var results = await _context.PrimeResults
                .OrderByDescending(r => r.ExecutedAt)
                .ToListAsync();
            return Ok(results);
        }
        private bool IsPrime(int number)
        {
            if (number < 2) return false;

            for (int i = 2; i <= Math.Sqrt(number); i++)
            {
                if (number % i == 0) return false;
            }

            return true;
        }
    }
}
