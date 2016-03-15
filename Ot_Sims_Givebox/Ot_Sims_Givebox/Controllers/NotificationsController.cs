using Ot_Sims_Givebox.helper;
using Ot_Sims_Givebox.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace Ot_Sims_Givebox.Controllers
{
    [Authorize]
    public class NotificationsController : ApiController
    {
        private ModelContainer db = new ModelContainer();

        //Crée une notification sur l'offre donnée
        [HttpPost]
        public async Task<IHttpActionResult> PostNotification(int id)
        {
            Utilisateur u = UserHelper.getUser(User, db);
            var request = from notifications in db.NotificationSet where notifications.OffreId.Equals(id) where notifications.UtilisateurId.Equals(u.Id) select notifications;
            if (request != null)
            {
                return BadRequest("Une notification a déjà été envoyé pour cette offre");
            }
            Notification notif = new Notification() { OffreId = id, Date = DateTime.Now, UtilisateurId = u.Id };

            db.NotificationSet.Add(notif);
            try
            {
                await db.SaveChangesAsync();
                return Created("DefaultApi", notif);
            }
            catch (Exception e)
            {
                return InternalServerError(e);
            }
        }

        //valider la demande et archiver l'offre. 
        [HttpPut]
        public async Task<IHttpActionResult> PutNotification(int id)
        {
            Utilisateur u = UserHelper.getUser(User, db);
            Notification notif = await db.NotificationSet.FindAsync(id);
            Offre o = await db.OffreSet.FindAsync(notif.OffreId);
            if (u.Id != o.UtilisateurId)
            {
                return Unauthorized();
            }
            notif.EstAccepte = true;
            o.EstArchivee = true;
            try
            {
                await db.SaveChangesAsync();
                return Ok();
            }
            catch (Exception e)
            {
                return InternalServerError(e);
            }
        }
    }
}
