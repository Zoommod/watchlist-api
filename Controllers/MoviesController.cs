using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Watchlist.Data;
using Watchlist.models;

namespace Watchlist.Controllers;


[ApiController]
[Route("api/[controller]")]

public class MoviesController : ControllerBase{
    
    private readonly WatchlistContext _context;

    public MoviesController(WatchlistContext context){
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<MovieModel>>> GetAllMovies(){
        return await _context.Movies.ToListAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<MovieModel>> GetMovieById(Guid id){
        var movie = await _context.Movies.FindAsync(id);
        if (movie == null){
            return NotFound();
        }

        return movie;
    }

    [HttpPost]
    public async Task<ActionResult<MovieModel>> PostMovie(MovieModel movie){
        _context.Movies.Add(movie);
        await _context.SaveChangesAsync();

        return CreatedAtAction("GetMovie", new {id = movie.Id, movie});
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> PutMovie(Guid id, MovieModel movie){
        if(id != movie.Id){
            return BadRequest();
        }

    _context.Entry(movie).State = EntityState.Modified;

    try{
        await _context.SaveChangesAsync();
    }
    catch (DbUpdateConcurrencyException){
        if (!_context.Movies.Any(e => e.Id == id)){
            return NotFound();
        }else{
            throw;
        }
    }

    return NoContent();
}

[HttpDelete("{id}")]
    public async Task<IActionResult> DeleteMovie(Guid id)
    {
        var movie = await _context.Movies.FindAsync(id);
        if (movie == null)
        {
            return NotFound();
        }

        _context.Movies.Remove(movie);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}