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
    public async Task<ActionResult<IEnumerable<MovieModel>>> GetAllMovies(string genre = null){
        var query = _context.Movies.AsQueryable();
        if (!string.IsNullOrEmpty(genre)){
            query = query.Where(m => m.Genre == genre);
        }

        var movies = await query.ToListAsync();

        if (!movies.Any()){
            return NotFound(new { Message = "Nenhum filme com esse gênero foi encontrado."});
        }
        return Ok(movies);
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
        movie.Id = Guid.NewGuid();

        _context.Movies.Add(movie);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetMovieById), new { id = movie.Id }, movie);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<MovieModel>> PutMovie(Guid id, MovieModel movie){
        if (id != movie.Id)
        {
            return BadRequest("O ID na rota não corresponde ao ID do filme.");
        }

        // Verifica se o filme existe no banco de dados
        var existingMovie = await _context.Movies.FindAsync(id);
        if (existingMovie == null)
        {
            return NotFound("Filme não encontrado.");
        }

        // Atualiza apenas os campos fornecidos
        existingMovie.Title = movie.Title;
        existingMovie.Genre = movie.Genre;
        existingMovie.ReleaseYear = movie.ReleaseYear;
        existingMovie.Watched = movie.Watched;
        existingMovie.Rating = movie.Rating;
        existingMovie.Comment = movie.Comment;

        try{
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException){
            if (!_context.Movies.Any(e => e.Id == id)){
                return NotFound("Filme não encontrado.");
            }
            else{
                throw;
            }
        }

        return Ok(existingMovie); // Retorna o filme atualizado
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