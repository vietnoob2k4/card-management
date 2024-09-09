using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QLCapThe.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QLCapThe.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CardController : ControllerBase
    {
        private readonly QLCapTheV2Context _context;

        public CardController(QLCapTheV2Context context)
        {
            _context = context;
        }

        // GET: api/Card
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Card>>> GetCards()
        {
            try
            {
                var cards = await _context.Cards.ToListAsync();

                // Update status for each card
                foreach (var card in cards)
                {
                    card.CheckAndUpdateStatus();
                    _context.Entry(card).State = EntityState.Modified;
                }

                await _context.SaveChangesAsync();

                return Ok(cards);
            }
            catch (Exception ex)
            {
                // Log exception
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }

        // GET: api/Card/user/5
        [HttpGet("user/{userId}")]
        public async Task<ActionResult<IEnumerable<Card>>> GetCardsByUserId(int userId)
        {
            try
            {
                var cards = await _context.Cards
                    .Where(c => c.UserId == userId)
                    .ToListAsync();

                if (cards == null || !cards.Any())
                {
                    return NotFound("No cards found for the given user ID.");
                }

                // Update status for each card
                foreach (var card in cards)
                {
                    card.CheckAndUpdateStatus();
                    _context.Entry(card).State = EntityState.Modified;
                }

                await _context.SaveChangesAsync();

                return Ok(cards);
            }
            catch (Exception ex)
            {
                // Log exception
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }

        // GET: api/Card/cardId/5
        [HttpGet("cardId/{cardId}")]
        public async Task<ActionResult<Card>> GetCardById(Guid cardId)
        {
            try
            {
                var card = await _context.Cards.FindAsync(cardId);

                if (card == null)
                {
                    return NotFound("Card not found with the given ID.");
                }

                // Update status if needed
                card.CheckAndUpdateStatus();
                _context.Entry(card).State = EntityState.Modified;
                await _context.SaveChangesAsync();

                return Ok(card);
            }
            catch (Exception ex)
            {
                // Log exception
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }

        // PUT: api/Card/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCard(Guid id, Card card)
        {
            if (id != card.CardId)
            {
                return BadRequest("Card ID mismatch");
            }

            // Check and update status
            card.CheckAndUpdateStatus();
            _context.Entry(card).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CardExists(id))
                {
                    return NotFound("Card not found.");
                }
                else
                {
                    return StatusCode(500, "Concurrency error occurred.");
                }
            }
            catch (Exception ex)
            {
                // Log exception
                return StatusCode(500, "Internal server error: " + ex.Message);
            }

            return NoContent();
        }

        // POST: api/Card
        [HttpPost]
        public async Task<ActionResult<Card>> PostCard(Card card)
        {
            try
            {
                card.CreatedAt = DateTime.Now;
                // Kiểm tra nếu CreatedAt không phải là null rồi mới thêm số năm
                if (card.CreatedAt.HasValue)
                {
                    card.ExpiryDate = card.CreatedAt.Value.AddYears(3); // Set expiry date to 3 years from creation
                }
                else
                {
                    // Xử lý khi CreatedAt là null nếu cần thiết
                    return BadRequest("Creation date cannot be null.");
                }

                _context.Cards.Add(card);
                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(GetCardById), new { cardId = card.CardId }, card);
            }
            catch (Exception ex)
            {
                // Log exception
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }

        // DELETE: api/Card/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCard(Guid id)
        {
            try
            {
                var card = await _context.Cards.FindAsync(id);
                if (card == null)
                {
                    return NotFound("Card not found.");
                }

                _context.Cards.Remove(card);
                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                // Log exception
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }

        private bool CardExists(Guid id)
        {
            return _context.Cards.Any(e => e.CardId == id);
        }

        // GET: api/Card/filter
        [HttpGet("filter")]
        public async Task<ActionResult<IEnumerable<Card>>> GetCardsByDateRange(DateTime startDate, DateTime endDate)
        {
            try
            {
                var cards = await _context.Cards
                    .Where(c => c.CreatedAt >= startDate && c.CreatedAt <= endDate)
                    .ToListAsync();

                return Ok(cards);
            }
            catch (Exception ex)
            {
                // Log exception
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }

        // GET: api/Card/count
        [HttpGet("count")]
        public async Task<ActionResult<int>> GetCardCount()
        {
            try
            {
                var count = await _context.Cards.CountAsync();
                return Ok(count);
            }
            catch (Exception ex)
            {
                // Log exception
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }
    }
}
