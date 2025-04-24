using System.ComponentModel.DataAnnotations;

namespace Asan_Campus.Model
{
    public class RevokedToken
    {
        public int Id { get; set; }
        public string Token { get; set; }
        [MaxLength(128)]
        public string UserId { get; set; } // Associate token with the user
        public DateTime RevocationDate { get; set; }
    }
}
