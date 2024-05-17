using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TripsEntityFramework.Models
{
    [Table("Trip")]
    public class Trip
    {
        [Key]
        [Column("IdTrip")]
        public int Id { get; set; }

        [Required]
        [Column("Name")]
        public string Name { get; set; }

        [Required]
        [Column("Description")]
        public string Description { get; set; }

        [Required]
        [Column("DateFrom")]
        public DateTime StartDate { get; set; }

        [Required]
        [Column("DateTo")]
        public DateTime EndDate { get; set; }

        [Required]
        [Column("MaxPeople")]
        public int MaxPeople { get; set; }

        public ICollection<ClientTrip> ClientTrips { get; set; } = new List<ClientTrip>();
        public ICollection<CountryTrip> CountryTrips { get; set; } = new List<CountryTrip>();
    }
}