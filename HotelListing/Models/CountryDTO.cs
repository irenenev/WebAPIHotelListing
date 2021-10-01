using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HotelListing.Models
{
    //DTO - класс для взаимодействия с пользователем
    public class CountryDTO : UpsertCountryDTO
    {
        public int Id { get; set; }
        public IList<HotelDTO> Hotels {get; set; }
    }
    
    public class UpsertCountryDTO
    {
        [Required]
        [StringLength(maximumLength: 50, ErrorMessage = "Name is too long")]
        public string Name { get; set; }
        [Required]
        [StringLength(maximumLength: 2, ErrorMessage = "Need only 2 symbols")]
        public string ShortName { get; set; }
    }
}
