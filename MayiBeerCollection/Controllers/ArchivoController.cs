using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace MayiBeerCollection.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ArchivoController : ControllerBase
    {
        private readonly IWebHostEnvironment _env;

        public ArchivoController(IWebHostEnvironment env)
        {
            _env = env;
        }

        [HttpPost, Route("cargar-archivo")]
        public Task<byte[]> UploadFile()
        {
            bool resultado = false;
            byte[] imagen = new byte[0];
            //La variable "file" recibe el archivo en el objeto Request.Form
            //Del POST que realiza la aplicacion a este servicio.
            //Se envia un formulario completo donde uno de los valores es el archivo
            var file = Request.Form.Files[0];

            string RutaRaiz = _env.ContentRootPath;
            string NombreCarpeta = "/database/";
            string RutaCompleta = RutaRaiz + NombreCarpeta;

            if (!Directory.Exists(RutaCompleta))
            {
                Directory.CreateDirectory(RutaCompleta);
            }

            if (file.Length > 0)
            {
                string NombreArchivo = file.FileName;
                string RutaFullCompleta = Path.Combine(RutaCompleta, NombreArchivo);
                

                using (var stream = new MemoryStream())
                {
                    file.CopyTo(stream);
                    imagen = stream.ToArray();
                    resultado = true;
                }

                using (var stream = new FileStream(RutaFullCompleta, FileMode.Create))
                {
                    file.CopyTo(stream);
                    resultado = true;
                }

            }
            return Task.FromResult(imagen);
        }



    }
}
