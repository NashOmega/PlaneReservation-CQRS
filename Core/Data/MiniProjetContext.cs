﻿using Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace Core.Data
{
    public class MiniProjetContext : DbContext
    {
        public MiniProjetContext(DbContextOptions<MiniProjetContext> options) : base(options)
        {

        }
        public DbSet<PlaneEntity> Planes { get; set; }
        public DbSet<PassengerEntity> Passengers { get; set; }
        public DbSet<ReservationEntity> Reservations { get; set; }
        public DbSet<SeatArrangementEntity> SeatArrangements { get; set; }
    }

}
