﻿using LittleLeagueUParser.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LittleLeagueUParser.Data
{
    public class LittleLeagueDbContext : DbContext
    {
        public LittleLeagueDbContext(DbContextOptions<LittleLeagueDbContext> options) : base(options)
        {
        }

        public DbSet<ExeternalNewsItem> ExternalNewsItems { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var tablePrefix = "LittleLeague_";

            modelBuilder.Entity<ExeternalNewsItem>().ToTable(tablePrefix + "ExeternalNewsItem");
        }
    }
}