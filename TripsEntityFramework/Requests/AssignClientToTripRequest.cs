namespace TripsEntityFramework.Requests;

public class AssignClientToTripRequest
{
    public string Pesel { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string Telephone { get; set; }
}
