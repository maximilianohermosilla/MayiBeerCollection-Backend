using AutoMapper;
using MayiBeerCollection.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Numerics;
using System.Linq;
using System.Threading.Tasks;
using MayiBeerCollection.DTO;
using static System.Collections.Specialized.BitVector32;
using static System.Net.Mime.MediaTypeNames;

#nullable disable
namespace MayiBeerCollection.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class CervezaController : ControllerBase
    {
        private MayiBeerCollectionContext _contexto;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;

        public CervezaController(MayiBeerCollectionContext context, IConfiguration configuration, IMapper mapper)
        {
            _contexto = context;
            _configuration = configuration;
            _mapper = mapper;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Cerveza>> Cervezas()
        {
            List<Cerveza> lst = (from tbl in _contexto.Cervezas where tbl.Id > 0 select tbl).ToList();

            List<CervezaDTO> cervezasDTO = _mapper.Map<List<CervezaDTO>>(lst);

            foreach (var item in cervezasDTO)
            {
                Estilo _estilo = (from h in _contexto.Estilos where h.Id == item.IdEstilo select h).FirstOrDefault();
                if (_estilo != null)
                {
                    item.NombreEstilo = _estilo.Nombre;
                }

                Marca _marca = (from h in _contexto.Marcas where h.Id == item.IdMarca select h).FirstOrDefault();
                if (_marca != null)
                {
                    item.NombreMarca = _marca.Nombre;
                }

                Ciudad _ciudad = (from h in _contexto.Ciudads where h.Id == item.IdCiudad select h).FirstOrDefault();
                if (_ciudad != null)
                {
                    Pai _pais = (from h in _contexto.Pais where h.Id == _ciudad.IdPais select h).FirstOrDefault();
                    item.IdPais = _pais.Id;
                    item.NombrePais = _pais.Nombre;
                    item.NombreCiudad = _ciudad.Nombre + " (" + _pais.Nombre + ")";
                }
            }

            return Accepted(cervezasDTO);
        }
        [HttpGet("{CervezaId}")]
        public ActionResult<Cerveza> Cervezas(int CervezaId)
        {
            Cerveza cl = (from tbl in _contexto.Cervezas where tbl.Id == CervezaId select tbl).FirstOrDefault();

            if (cl == null)
            {
                return NotFound(CervezaId);
            }

            CervezaDTO item = _mapper.Map<CervezaDTO>(cl);

            Estilo _estilo = (from h in _contexto.Estilos where h.Id == item.IdEstilo select h).FirstOrDefault();
            if (_estilo != null)
            {
                item.NombreEstilo = _estilo.Nombre;
            }

            Marca _marca = (from h in _contexto.Marcas where h.Id == item.IdMarca select h).FirstOrDefault();
            if (_marca != null)
            {
                item.NombreMarca = _marca.Nombre;
            }

            Ciudad _ciudad = (from h in _contexto.Ciudads where h.Id == item.IdCiudad select h).FirstOrDefault();
            if (_ciudad != null)
            {
                Pai _pais = (from h in _contexto.Pais where h.Id == _ciudad.IdPais select h).FirstOrDefault();
                item.IdPais = _pais.Id;
                item.NombrePais = _pais.Nombre;
                item.NombreCiudad = _ciudad.Nombre + " (" + _pais.Nombre + ")";
            }

            return Accepted(item);
        }

        [HttpPost("nuevo")]
        public ActionResult nuevo([FromForm] CervezaDTO nuevo)
        {
            Cerveza _cerveza = _mapper.Map<Cerveza>(nuevo);

            _contexto.Cervezas.Add(_cerveza);
            _contexto.SaveChanges();

            nuevo.Id = _cerveza.Id;

            return Accepted(nuevo);
        }

        [HttpPut("actualizar")]
        public ActionResult actualizar([FromForm] CervezaDTO actualiza)
        {

            Cerveza _cerveza = (from h in _contexto.Cervezas where h.Id == actualiza.Id select h).FirstOrDefault();

            if (_cerveza == null)
            {
                return NotFound(actualiza);
            }

            _cerveza.Nombre = actualiza.Nombre;
            _cerveza.Ibu = actualiza.Ibu;
            _cerveza.Alcohol = actualiza.Alcohol;
            _cerveza.Contenido = actualiza.Contenido;
            _cerveza.Observaciones = actualiza.Observaciones;
            _cerveza.IdCiudad = actualiza.IdCiudad;
            _cerveza.IdEstilo = actualiza.IdEstilo;
            _cerveza.IdMarca = actualiza.IdMarca;
          
            _contexto.Cervezas.Update(_cerveza);
            _contexto.SaveChanges();

            return Accepted(actualiza);
        }
        [HttpDelete("eliminar/{CervezaId}")]
        public ActionResult eliminar(int CervezaId)
        {
            Cerveza _cerveza = (from h in _contexto.Cervezas where h.Id == CervezaId select h).FirstOrDefault();

            if (_cerveza == null)
            {
                return NotFound(CervezaId);
            }

            _contexto.Cervezas.Remove(_cerveza);
            _contexto.SaveChanges();

            return Accepted(CervezaId);
        }
    }
}