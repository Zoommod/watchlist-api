using Microsoft.EntityFrameworkCore;
using Watchlist.models;

namespace Watchlist.Data;

public class WatchlistContext : DbContext{

    public WatchlistContext(DbContextOptions<WatchlistContext> options) : base(options) { }

    public DbSet<MovieModel> Movies { get; set; }
}