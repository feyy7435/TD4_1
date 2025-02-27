using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using TD4.Models.EntityFramework;

namespace TD4.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UtilisateursController : ControllerBase
    {
        private readonly UtilisateurManager utilisateurManager;
        //private readonly FilmRatingsDBContext _context;

        //public UtilisateursController(FilmRatingsDBContext context)
        //{
        //    _context = context;
        //}
        public UtilisateursController(UtilisateurManager userManager)
        {
            utilisateurManager = userManager;
        }

        // GET: api/Utilisateurs
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Utilisateur>>> GetUtilisateurs()
        {
            //return await _context.Utilisateurs.ToListAsync();
            return utilisateurManager.GetAll();
        }

        // GET: api/Utilisateurs/5
        [HttpGet]
        [Route("[action]/{id}")]
        [ActionName("GetById")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Utilisateur>> GetUtilisateur(int id)
        {
            var utilisateur = utilisateurManager.GetById(id);
            //var utilisateur = await _context.Utilisateurs.FindAsync(id);

            if (utilisateur == null)
            {
                return NotFound();
            }

            return utilisateur;
        }

        // GET: api/Utilisateurs/toto@titi.fr
        [HttpGet]
        [Route("[action]/{email}")]
        [ActionName("GetByEmail")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Utilisateur>> GetUtilisateurByEmail(string email)

        {
            var utilisateur = utilisateurManager.GetByString(email);
            //var utilisateur = await _context.Utilisateurs.FirstOrDefaultAsync(e => e.Mail.ToUpper() == email.ToUpper());
            if (utilisateur == null)
            {
                return NotFound();
            }
            return utilisateur;
        }


        // PUT: api/Utilisateurs/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> PutUtilisateur(int id, Utilisateur utilisateur)
        {
            if (id != utilisateur.UtilisateurId)
            {
                return BadRequest();
            }
            var userToUpdate = utilisateurManager.GetById(id);
            if (userToUpdate == null)
            {
                return NotFound();
            }
            else
            {
                utilisateurManager.Update(userToUpdate.Value, utilisateur);
                return NoContent();
            }
        }

        // POST: api/Utilisateurs
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]

        public async Task<ActionResult<Utilisateur>> PostUtilisateur(Utilisateur utilisateur)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            utilisateurManager.Add(utilisateur);
            return CreatedAtAction("GetById", new { id = utilisateur.UtilisateurId }, utilisateur); // GetById : nom de l’action
        }

        // DELETE: api/Utilisateurs/5
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]

        public async Task<IActionResult> DeleteUtilisateur(int id)
        {
            var utilisateur = utilisateurManager.GetById(id);

            //var utilisateur = await _context.Utilisateurs.FindAsync(id);
            if (utilisateur == null)
            {
                return NotFound();
            }

            utilisateurManager.Delete(utilisateur.Value);
            // _context.Utilisateurs.Remove(utilisateur);
            //await _context.SaveChangesAsync();

            return NoContent();
        }

        //private bool UtilisateurExists(int id)
        //{
        //    return _context.Utilisateurs.Any(e => e.UtilisateurId == id);
        //}
    }

}