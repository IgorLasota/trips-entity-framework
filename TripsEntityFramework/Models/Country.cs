using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TripsEntityFramework.Models;

[Table("Country")]
public class Country
{
    [Key]
    [Column("IdCountry")]
    public int Id { get; set; }

    [Required]
    [Column("Name")]
    public string Name { get; set; }

    public ICollection<CountryTrip> CountryTrips { get; set; } = new List<CountryTrip>();
}
