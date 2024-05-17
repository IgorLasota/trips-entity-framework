using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TripsEntityFramework.Models;

[Table("Country_Trip")]
public class CountryTrip
{
    public int IdCounty { get; set; }
    public Country Country { get; set; }

    public int IdTrip { get; set; }
    public Trip Trip { get; set; }
}
