using Ot_Sims_Givebox.helper;
using Ot_Sims_Givebox.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace Ot_Sims_Givebox.Controllers
{
    [Authorize]
    public class FichiersController : ApiController
    {
        private ModelContainer db = new ModelContainer();
        [AllowAnonymous]
        public async Task<HttpResponseMessage> GetFichier(int id)
        {
            Fichier fichier = await db.FichierSet.FindAsync(id);
            if (fichier == null)
            {
                return new HttpResponseMessage(HttpStatusCode.NoContent);
            }
            HttpResponseMessage ret = new HttpResponseMessage(HttpStatusCode.OK);
            ret.Content = new StreamContent(new FileStream(fichier.Chemin, FileMode.Open));
            ret.Content.Headers.ContentType = new MediaTypeHeaderValue(fichier.contentType);
            return ret;
        }

        // RECUPERATION DE L'IMAGE - POST
        public async Task<HttpResponseMessage> PostFichier(int id)
        {
            //vérifie que l'utilisteur soit le créateur de l'offre

            Offre o = await db.OffreSet.FindAsync(id);
            if (o.UtilisateurId != UserHelper.getUser(User, db).Id)
            {
                return new HttpResponseMessage(HttpStatusCode.Unauthorized);
            }

            string dataDirectory = "Images/";
            if (!Request.Content.IsMimeMultipartContent())
            {
                throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
            }

            string root = HttpContext.Current.Server.MapPath("~/" + dataDirectory);
            var provider = new MultipartFormDataStreamProvider(root);


            HttpResponseMessage ret = new HttpResponseMessage(HttpStatusCode.Created);
            try
            {
                // Read the form data.
                await Request.Content.ReadAsMultipartAsync(provider);

                // This illustrates how to get the file names.

                foreach (MultipartFileData file in provider.FileData)
                {
                    string type = file.Headers.ContentDisposition.Name;
                    string type2 = type.TrimStart('"');
                    string[] type3 = type2.Split('_');

                    string localName = file.LocalFileName.Substring(file.LocalFileName.LastIndexOf("\\") + 1);
                    string url = Request.RequestUri.AbsoluteUri;
                    Fichier fichier = new Fichier()
                    {
                        OffreId = id,
                        Titre = file.Headers.ContentDisposition.FileName,
                        Chemin = file.LocalFileName,
                        FichierTypeId = Int32.Parse(type3[0]),
                        contentType = file.Headers.ContentType.ToString(),
                        url = ""
                    };
                    //Trace.WriteLine(file.Headers.ContentDisposition.FileName);
                    //Trace.WriteLine("Server file path: " + file.LocalFileName);
                    db.FichierSet.Add(fichier);
                    await db.SaveChangesAsync();
                    fichier.url = url.Substring(0, url.IndexOf("api")) + "api/fichiers/" + fichier.Id;
                    await db.SaveChangesAsync();

                }
                return ret;
            }
            catch (System.Exception e)
            {
                HttpResponseMessage err = new HttpResponseMessage(HttpStatusCode.InternalServerError);
                err.Content = new StringContent(e.ToString());
                return err;
            }
        }

        //ajout de l'avatar
        public async Task<HttpResponseMessage> PostFichier() {
            Utilisateur user = UserHelper.getUser(User, db);

            string dataDirectory = "Images/";
            if (!Request.Content.IsMimeMultipartContent())
            {
                throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
            }

            string root = HttpContext.Current.Server.MapPath("~/" + dataDirectory);
            var provider = new MultipartFormDataStreamProvider(root);

            HttpResponseMessage ret = new HttpResponseMessage(HttpStatusCode.Created);
            try
            {
                // Read the form data.
                await Request.Content.ReadAsMultipartAsync(provider);
                if (provider.FileData.Count == 1)
                {
                    MultipartFileData file = provider.FileData.First();
                    string type = file.Headers.ContentDisposition.Name;
                    string type2 = type.TrimStart('"');
                    string[] type3 = type2.Split('_');

                    string localName = file.LocalFileName.Substring(file.LocalFileName.LastIndexOf("\\") + 1);
                    string url = Request.RequestUri.AbsoluteUri;
                    Fichier fichier = new Fichier()
                    {
                        Titre = file.Headers.ContentDisposition.FileName,
                        Chemin = file.LocalFileName,
                        FichierTypeId = Int32.Parse(type3[0]),
                        contentType = file.Headers.ContentType.ToString(),
                        url = ""
                    };
                    //Trace.WriteLine(file.Headers.ContentDisposition.FileName);
                    //Trace.WriteLine("Server file path: " + file.LocalFileName);
                    user.Fichier = fichier;
                    await db.SaveChangesAsync();
                    fichier.url = url.Substring(0, url.IndexOf("api")) + "api/fichiers/" + fichier.Id;
                    await db.SaveChangesAsync();
                    return ret;
                }
                else
                {
                    throw new HttpResponseException(HttpStatusCode.BadRequest);
                }
            }
            catch (Exception e)
            {
                HttpResponseMessage err = new HttpResponseMessage(HttpStatusCode.InternalServerError);
                err.Content = new StringContent(e.ToString());
                return err;
            }
        }

        

        //option handler
        [AllowAnonymous]
        public HttpResponseMessage Options()
        {
            return new HttpResponseMessage { StatusCode = HttpStatusCode.OK };
        }
    }
}
