using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TripsEntityFramework.Models;

[Table("Client_Trip")]
public class ClientTrip
{
    public int IdClient { get; set; }
    public Client Client { get; set; }

    public int IdTrip { get; set; }
    public Trip Trip { get; set; }

    public DateTime RegisteredAt { get; set; }
    public DateTime? PaymentDate { get; set; }
}