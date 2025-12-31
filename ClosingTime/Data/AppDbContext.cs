using System;
using ClosingTime.Models;
using Microsoft.EntityFrameworkCore;

namespace ClosingTime.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<ApplicationUser> Users { get; set; }
}
