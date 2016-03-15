using Ot_Sims_Givebox.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;
using Microsoft.AspNet.Identity;

namespace Ot_Sims_Givebox.helper
{
    public static class UserHelper
    {
        public static Utilisateur getUser(IPrincipal User, ModelContainer db){
            string userId = User.Identity.GetUserId();
            var utilisateurs = db.UtilisateurSet.Where(u => u.UserId.Equals(userId));
            if (utilisateurs.Any())
            {
                return utilisateurs.First();
            }
            else
            {
                return null;
            }
        }
    }
}