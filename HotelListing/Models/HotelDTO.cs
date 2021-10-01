using System;
using System.ComponentModel.DataAnnotations;

namespace HotelListing.Models
{
    //DTO - класс для взаимодействия с пользователем
    public class HotelDTO : UpsertHotelDTO
    {
        public int Id { get; set; }
        public CountryDTO Country { get; set; }
    }
    //public class UpdateHotelDTO : CreateHotelDTO {}

    public class UpsertHotelDTO //CreateHotelDTO
    {
        [Required]
        [StringLength(maximumLength: 100, ErrorMessage = "Name is too long")]
        public string Name { get; set; }
        [Required]
        [StringLength(maximumLength: 150, ErrorMessage = "Adress is too long")]
        public string Adress { get; set; }
        [Required]
        [Range(1, 5)]
        public double Rating { get; set; }
        [Required]
        public int CountryId { get; set; }
    }
}
