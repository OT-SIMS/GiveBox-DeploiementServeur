using Ot_Sims_Givebox.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Web.Http;

namespace Ot_Sims_Givebox.Controllers
{
    [AllowAnonymous]
    public class CategoriesController : ApiController
    {
        private ModelContainer db = new ModelContainer();

        // GET: api/Categories
        public IQueryable<Categorie> GetCategorieSet()
        {
            return db.CategorieSet;
        }

        //option handler
        public HttpResponseMessage Options()
        {
            return new HttpResponseMessage { StatusCode = HttpStatusCode.OK };
        }
    }
}
