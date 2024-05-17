using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TripsEntityFramework.Models;

[Table("Client")]
public class Client
{
    [Key]
    [Column("IdClient")]
    public int Id { get; set; }

    [Required]
    [Column("FirstName")]
    public string FirstName { get; set; }

    [Required]
    [Column("LastName")]
    public string LastName { get; set; }

    [Required]
    [Column("Email")]
    public string Email { get; set; }

    [Required]
    [Column("Telephone")]
    public string Telephone { get; set; }

    [Required]
    [Column("Pesel")]
    public string Pesel { get; set; }

    public ICollection<ClientTrip> ClientTrips { get; set; } = new List<ClientTrip>();
}

