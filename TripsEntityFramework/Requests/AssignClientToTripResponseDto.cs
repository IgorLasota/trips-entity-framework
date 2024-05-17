namespace TripsEntityFramework.Requests
{
    public class AssignClientToTripResponseDto
    {
        public int ClientId { get; set; }
        public int TripId { get; set; }
        public string ClientName { get; set; }
        public string TripName { get; set; }
        public DateTime RegisteredAt { get; set; }
        public DateTime? PaymentDate { get; set; }
        public string Message { get; set; }
    }
}