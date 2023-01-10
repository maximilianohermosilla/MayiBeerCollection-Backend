using AutoMapper;
using MayiBeerCollection.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Numerics;
using System.Linq;
using System.Threading.Tasks;
using MayiBeerCollection.DTO;
using System.Text;

#nullable disable
namespace MayiBeerCollection.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class EstiloController : ControllerBase
    {
        private MayiBeerCollectionContext _contexto;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;

        public EstiloController(MayiBeerCollectionContext context, IConfiguration configuration, IMapper mapper)
        {
            _contexto = context;
            _configuration = configuration;
            _mapper = mapper;
        }

        [HttpGet]
        public ActionResult<IEnumerable<EstiloDTO>> Estilos()
        {
            var lst = (from tbl in _contexto.Estilos where tbl.Id > 0 select new Estilo() { Id = tbl.Id, Nombre = tbl.Nombre, IdArchivo = tbl.IdArchivo }).ToList();

            List<EstiloDTO> estilosDTO = _mapper.Map<List<EstiloDTO>>(lst);

            foreach (var item in estilosDTO)
            {
                Archivo _archivo = (from h in _contexto.Archivos where h.Id == item.IdArchivo select h).FirstOrDefault();
                if (_archivo != null)
                {
                    string stringArchivo = Encoding.ASCII.GetString(_archivo.Archivo1);
                    item.Imagen = stringArchivo;
                }
            }

            return Accepted(estilosDTO);
        }

        [HttpGet("buscar/{EstiloId}")]
        public ActionResult<EstiloDTO> Estilos(int EstiloId)
        {
            Estilo cl = (from tbl in _contexto.Estilos where tbl.Id == EstiloId select new Estilo() { Id = tbl.Id, Nombre = tbl.Nombre, IdArchivo = tbl.IdArchivo }).FirstOrDefault();
            if (cl == null)
            {
                return NotFound(EstiloId);
            }

            EstiloDTO item = _mapper.Map<EstiloDTO>(cl);

            Archivo _archivo = (from h in _contexto.Archivos where h.Id == item.IdArchivo select h).FirstOrDefault();

            if (_archivo != null)
            {
                string stringArchivo = Encoding.ASCII.GetString(_archivo.Archivo1);
                item.Imagen = stringArchivo;
            }

            return Accepted(item);
        }

        [HttpPost("nuevo")]
        public ActionResult nuevo(EstiloDTO nuevo)
        {
            try
            {
                Estilo _estilo = _mapper.Map<Estilo>(nuevo);

                if (nuevo.Imagen != null)
                {
                    byte[] bytes = Encoding.ASCII.GetBytes(nuevo.Imagen);
                    Archivo newArch = new Archivo() { Archivo1 = bytes };
                    _contexto.Archivos.Add(newArch);
                    _contexto.SaveChanges();                    
                    _estilo.IdArchivo = newArch.Id;
                }

                _contexto.Estilos.Add(_estilo);
                _contexto.SaveChanges();

                nuevo.Id = _estilo.Id;

                return Accepted(_estilo);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }            
        }

        [HttpPut("actualizar")]
        public ActionResult actualizar(EstiloDTO actualiza)
        {
            try
            {
                Estilo _estilo = (from h in _contexto.Estilos where h.Id == actualiza.Id select h).FirstOrDefault();

                if (_estilo == null)
                {
                    return NotFound(actualiza);
                }
                _estilo.Nombre = actualiza.Nombre;

                if (actualiza.Imagen != null)
                {
                    byte[] bytes = Encoding.ASCII.GetBytes(actualiza.Imagen);
                    Archivo arch = (from a in _contexto.Archivos where a.Id == _estilo.IdArchivo select a).FirstOrDefault();

                    if (arch == null)
                    {
                        Archivo newArch = new Archivo() { Archivo1 = bytes };
                        _contexto.Archivos.Add(newArch);
                        _contexto.SaveChanges();
                        _estilo.IdArchivo = newArch.Id;
                    }
                    else
                    {
                        arch.Archivo1 = bytes;
                        _contexto.Archivos.Update(arch);
                        _contexto.SaveChanges();
                    }
                }  

                _contexto.Estilos.Update(_estilo);
                _contexto.SaveChanges();

                return Accepted(actualiza);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }                
        }


        [HttpDelete("eliminar/{EstiloId}")]
        public ActionResult eliminar(int EstiloId)
        {
            Estilo _estilo = (from h in _contexto.Estilos where h.Id == EstiloId select h).FirstOrDefault();

            if (_estilo == null)
            {
                return NotFound(EstiloId);
            }

            Archivo arch = (from a in _contexto.Archivos where a.Id == _estilo.IdArchivo select a).FirstOrDefault();

            if (arch == null)
            {
                _contexto.Archivos.Remove(arch);
                _contexto.SaveChanges();
            }

            _contexto.Estilos.Remove(_estilo);
            _contexto.SaveChanges();

            return Accepted(EstiloId);
        }
    }
}