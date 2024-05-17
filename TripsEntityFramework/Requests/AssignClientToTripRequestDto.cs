using System.ComponentModel.DataAnnotations;

namespace TripsEntityFramework.Requests;

public class AssignClientToTripRequestDto
{
    [Required]
    [StringLength(100)]
    public string FirstName { get; set; }
    
    [Required]
    [StringLength(100)]
    public string LastName { get; set; }
    
    [Required]
    [EmailAddress]
    public string Email { get; set; }
    
    [Required]
    [Phone]
    public string Telephone { get; set; }
    
    [Required]
    [StringLength(11, MinimumLength = 11)]
    public string Pesel { get; set; }
    
    [Required]
    [Range(1, int.MaxValue)]
    public int IdTrip { get; set; }
    
    [Required]
    [StringLength(100)]
    public string TripName { get; set; }
    
    [Required]
    [Range(1, int.MaxValue)]
    public DateTime? PaymentDate { get; set; }
}