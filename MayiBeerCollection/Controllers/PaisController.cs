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
    public class PaisController : ControllerBase
    {
        private MayiBeerCollectionContext _contexto;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;

        public PaisController(MayiBeerCollectionContext context, IConfiguration configuration, IMapper mapper)
        {
            _contexto = context;
            _configuration = configuration;
            _mapper = mapper;
        }

        [HttpGet(Name = "Pais")]
        public ActionResult<IEnumerable<Pai>> Pais()
        {
            var lst = (from tbl in _contexto.Pais where tbl.Id > 0 select new Pai() { Id = tbl.Id, Nombre = tbl.Nombre }).ToList();

            return lst;
        }
        [HttpGet("{PaisId}")]
        public ActionResult<Pai> Pais(int PaisId)
        {
            var cl = (from tbl in _contexto.Pais where tbl.Id == PaisId select new Pai() { Id = tbl.Id, Nombre = tbl.Nombre }).FirstOrDefault();
            if (cl == null)
            {
                return NotFound(PaisId);
            }
            return _mapper.Map<Pai>(cl);
        }

        [HttpPost("nuevo")]
        public ActionResult nuevo([FromForm] PaisDTO nuevoPais)
        {
            Pai _pais = _mapper.Map<Pai>(nuevoPais);

            _contexto.Pais.Add(_pais);
            _contexto.SaveChanges();

            nuevoPais.Id = _pais.Id;

            return Accepted(nuevoPais);
        }

        [HttpPut("actualizar")]
        public ActionResult actualizar([FromForm] PaisDTO actualizaPais)
        {

            Pai _pais = (from h in _contexto.Pais where h.Id == actualizaPais.Id select h).FirstOrDefault();

            if (_pais == null)
            {
                return NotFound(actualizaPais);
            }
            _pais.Nombre = actualizaPais.Nombre;

            _contexto.Pais.Update(_pais);
            _contexto.SaveChanges();

            return Accepted(actualizaPais);
        }
        [HttpDelete("eliminar/{PaisId}")]
        public ActionResult eliminar(int PaisId)
        {
            Pai _pais = (from h in _contexto.Pais where h.Id == PaisId select h).FirstOrDefault();

            if (_pais == null)
            {
                return NotFound(PaisId);
            }

            _contexto.Pais.Remove(_pais);
            _contexto.SaveChanges();

            return Accepted(PaisId);
        }
    }
}