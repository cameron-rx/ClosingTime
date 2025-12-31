using System;
using ClosingTime.Models;
using Microsoft.EntityFrameworkCore;

namespace ClosingTime.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<ApplicationUser> Users { get; set; }
    public DbSet<Template> Templates { get; set; }
    public DbSet<TemplateSection> TemplateSections { get; set; }
    public DbSet<TemplateItem> TemplateItems { get; set; }
}
