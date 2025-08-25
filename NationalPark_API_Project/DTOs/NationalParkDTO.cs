using System.ComponentModel.DataAnnotations;

namespace NationalPark_API_Project.DTOs
{
    public class NationalParkDTO
    {
        public int Id{ get; set; }
        [Required]
        public string Name{ get; set; }
        [Required]
        public string State { get; set; }
        public byte[]? Picture { get; set; }
        public DateTime Created { get; set; }
        public DateTime Established { get; set; }

    }
}
