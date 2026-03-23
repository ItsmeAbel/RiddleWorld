using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DadJokes.Data;
using DadJokes.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace DadJokes.Controllers
{
    public class JokesController : Controller
    {
        private readonly ApplicationDbContext _context; //to access joke/riddle info
        private readonly UserManager<IdentityUser> _userManager; //to access user info
        public int _upvote;
        public int _downvote;

        public JokesController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        
        // GET: Jokes
        public async Task<IActionResult> Index()
        {
            var jokesWithVotes = await _context.Joke
                .Include(j => j.Votes)
                .ToListAsync();

            return _context.Joke != null ?
                        View(jokesWithVotes) : //returns list
                        Problem("Entity set 'ApplicationDbContext.Joke'  is null.");
        }

        // GET: Jokes/SearchJokes
        public async Task<IActionResult> SearchJokes()
        {
            return View(); //could put in name of view in paranthesis, alternatively if empty, it takes the name of the method auto
        }

        // PoST: Jokes/SearchJokesResults
        public async Task<IActionResult> SearchJokesResults(String SearchPhrase)
        {
            return View("Index", await _context.Joke.Where(J => J.Question.Contains(SearchPhrase)).ToListAsync()); //could put in name of view in paranthesis, alternatively if empty, it takes the name of the method auto
        }

        // GET: Jokes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Joke == null)
            {
                return NotFound();
            }

            var joke = await _context.Joke
                .FirstOrDefaultAsync(m => m.Id == id);
            if (joke == null)
            {
                return NotFound();
            }

            return View(joke);
        }

        // GET: Jokes/Create
        [Authorize] //Requires Login
        public IActionResult Create()
        {
            return View();
        }

        // POST: Jokes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id, Question,Answer")] Joke joke)
        {
            if (!ModelState.IsValid)
            {
                joke.UserId = _userManager.GetUserId(User); //gets user id and stores it in db
                _context.Add(joke);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));

            }

            return View(joke);
        }

        // GET: Jokes/Edit/5
        [Authorize]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Joke == null)
            {
                return NotFound();
            }

            var joke = await _context.Joke.FindAsync(id);
            if (joke == null)
            {
                return NotFound();
            }

            var currentUserId = _userManager.GetUserId(User);
            if (currentUserId != joke.UserId) //Forbids edits if not the creater
            {
                return Forbid(); 

            };
            return View(joke);
        }

        // POST: Jokes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Question,Answer")] Joke joke)
        {
            if (id != joke.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(joke);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!JokeExists(joke.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(joke);
        }

        // GET: Jokes/Delete/5
        [Authorize]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Joke == null)
            {
                return NotFound();
            }

            var joke = await _context.Joke
                .FirstOrDefaultAsync(m => m.Id == id);
            if (joke == null)
            {
                return NotFound();
                
                
            }

            var currentUserId = _userManager.GetUserId(User);
            if (currentUserId != joke.UserId) //Forbids deletions if not the creater
            {
                return Forbid();

            };
            return View(joke);
        }

        // POST: Jokes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Joke == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Joke'  is null.");
            }
            var joke = await _context.Joke.FindAsync(id);
            if (joke != null)
            {
                _context.Joke.Remove(joke);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        //handles riddle upvotes
        [Authorize]
        public async Task<IActionResult> Upvote(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var existingVote = await _context.Votes
                .FirstOrDefaultAsync(v => v.JokeId == id && v.UserId == userId);

            if (existingVote == null)
            {
                _context.Votes.Add(new Vote { JokeId = id, UserId = userId, Value = 1 });
            }
            else if (existingVote.Value == 1)
            {
               _context.Votes.Remove(existingVote);
            }
            else{
                existingVote.Value += 1;
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));

        }

        //handles riddle downvotes
        [Authorize]
        public async Task<IActionResult> Downvote(int id)
        {

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var existingVote = await _context.Votes
                .FirstOrDefaultAsync(v => v.JokeId == id && v.UserId == userId);

            if (existingVote == null)
            {
                _context.Votes.Add(new Vote { JokeId = id, UserId = userId, Value = -1 });
            }
            else if (existingVote.Value == -1)
            {
                _context.Votes.Remove(existingVote);
            }
            else
            {
                existingVote.Value -= 1;
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool JokeExists(int id)
        {
          return (_context.Joke?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
