
namespace ProductAPI.Models
{
    public class PrimeResult
    {
        public int Id { get; set; }
        public int StartRange { get; set; }
        public int EndRange { get; set; }
        public string Primes { get; set; } = string.Empty;

        public DateTime ExecutedAt { get; set; } = DateTime.UtcNow;
    }
}
