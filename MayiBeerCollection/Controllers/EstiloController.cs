using AutoMapper;
using MayiBeerCollection.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Numerics;
using System.Linq;
using System.Threading.Tasks;
using MayiBeerCollection.DTO;

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
        public ActionResult<IEnumerable<Estilo>> Estilos()
        {
            var lst = (from tbl in _contexto.Estilos where tbl.Id > 0 select new Estilo() { Id = tbl.Id, Nombre = tbl.Nombre }).ToList();

            return lst;
        }
        [HttpGet("{EstiloId}")]
        public ActionResult<Estilo> Estilos(int EstiloId)
        {
            Estilo cl = (from tbl in _contexto.Estilos where tbl.Id == EstiloId select new Estilo() { Id = tbl.Id, Nombre = tbl.Nombre }).FirstOrDefault();
            if (cl == null)
            {
                return NotFound(EstiloId);
            }
            return _mapper.Map<Estilo>(cl);
        }

        [HttpPost("nuevo")]
        public ActionResult nuevo([FromForm] EstiloDTO nuevo)
        {
            Estilo _estilo = _mapper.Map<Estilo>(nuevo);

            _contexto.Estilos.Add(_estilo);
            _contexto.SaveChanges();

            nuevo.Id = _estilo.Id;

            return Accepted(nuevo);
        }

        [HttpPut("actualizar")]
        public ActionResult actualizar([FromForm] EstiloDTO actualiza)
        {

            Estilo _estilo = (from h in _contexto.Estilos where h.Id == actualiza.Id select h).FirstOrDefault();

            if (_estilo == null)
            {
                return NotFound(actualiza);
            }
            _estilo.Nombre = actualiza.Nombre;

            _contexto.Estilos.Update(_estilo);
            _contexto.SaveChanges();

            return Accepted(actualiza);
        }
        [HttpDelete("eliminar/{EstiloId}")]
        public ActionResult eliminar(int EstiloId)
        {
            Estilo _estilo = (from h in _contexto.Estilos where h.Id == EstiloId select h).FirstOrDefault();

            if (_estilo == null)
            {
                return NotFound(EstiloId);
            }

            _contexto.Estilos.Remove(_estilo);
            _contexto.SaveChanges();

            return Accepted(EstiloId);
        }
    }
}