using Ot_Sims_Givebox.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using Microsoft.AspNet.Identity;
using System.Threading.Tasks;
using Ot_Sims_Givebox.helper;
using System.Net.Http;
using System.Net;

namespace Ot_Sims_Givebox.Controllers
{
    [Authorize]
    [RoutePrefix("api/utilisateur")]
    public class UtilisateurController : ApiController
    {
        public class UserInfo
        {
            public string nom;
            public string prenom;
            public string telephone;
            public string dateNaissance;

            public void assign(Utilisateur u)
            {
                u.Nom = nom;
                u.Prenom = prenom;
                u.Telephone = telephone;
                u.DateNaissance = System.DateTime.Parse(dateNaissance);
            }
        }
        private ModelContainer db = new ModelContainer();
        //POST: Offres que l'user met en fav : api/utilisateur/favori/{idOffre}
        [Route("favori/{id}")]
        public async Task<IHttpActionResult> MettreFav(int id)
        {
            var utilisateur = UserHelper.getUser(User, db);
            db.FavoriSet.Add(new Favori()
            {
                UtilisateurId = utilisateur.Id,
                OffreId = id
            });
            await db.SaveChangesAsync();
            return Ok(); 
        }
        // DEL: Enlever une offre des favoris
        [Route("favori/{id}")]
        public async Task<IHttpActionResult> DeleteFav(int id)
        {
            var utilisateur = UserHelper.getUser(User, db);
            Favori favori = db.FavoriSet.Find(id);
            db.FavoriSet.Remove(favori);
            await db.SaveChangesAsync();
            return Ok();
        }
        // GET: Offres mises en fav par l'user : api/utilisateur/favori
        [Route("favori")]
        public IHttpActionResult getFav()
        {
            var utilisateur = UserHelper.getUser(User, db);
            if (utilisateur == null)
            {
                return ResponseMessage(new HttpResponseMessage(HttpStatusCode.NoContent));
            }
            else
            {
                IQueryable<Offre> request = null;
                request = from fav in db.FavoriSet where fav.UtilisateurId.Equals(utilisateur.Id) select fav.Offre; // sélectionne toutes les offres mises en fav par l'user
                return Ok(request);
            }
        }
        // GET: Offres Postées par l'user : api/utilisateur/offres
        [Route("Offres")]
        public IHttpActionResult getOffre()
        {
            var utilisateur = UserHelper.getUser(User, db);
            if (utilisateur == null)
            {
                return ResponseMessage(new HttpResponseMessage(HttpStatusCode.NoContent));
            }
            else
            {
                IQueryable<Offre> request = null;
                request = from offres in db.OffreSet where offres.UtilisateurId.Equals(utilisateur.Id) select offres; // sélectionne toutes les offres de l'user
                return Ok(request);
            }
        }



        public IHttpActionResult getUtilisateur()
        {
            var utilisateur = UserHelper.getUser(User, db);
            if (utilisateur == null)
            {
                return ResponseMessage(new HttpResponseMessage(HttpStatusCode.NoContent));
            }
            else
            {
                return Ok(utilisateur);
            }
        }

        public IHttpActionResult postUtilisateur([FromBody] UserInfo userInfo)
        {
            try
            {

                Utilisateur utilisateur = UserHelper.getUser(User, db);
                if (utilisateur == null)
                {
                    utilisateur = new Utilisateur() { UserId = User.Identity.GetUserId() };
                    userInfo.assign(utilisateur);
                    db.UtilisateurSet.Add(utilisateur);
                }
                else
                {
                    userInfo.assign(utilisateur);
                }
                db.SaveChanges();
                return Ok(utilisateur);
            }
            catch (Exception e)
            {
                return InternalServerError(e);
            }

        }

        [AllowAnonymous]
        //option handler
        public IHttpActionResult Options()
        {
            return Ok();
        }
    }
}