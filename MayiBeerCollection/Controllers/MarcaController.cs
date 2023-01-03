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
    public class MarcaController : ControllerBase
    {
        private MayiBeerCollectionContext _contexto;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;

        public MarcaController(MayiBeerCollectionContext context, IConfiguration configuration, IMapper mapper)
        {
            _contexto = context;
            _configuration = configuration;
            _mapper = mapper;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Marca>> Marcas()
        {
            var lst = (from tbl in _contexto.Marcas where tbl.Id > 0 select new Marca() { Id = tbl.Id, Nombre = tbl.Nombre }).ToList();

            return lst;
        }
        [HttpGet("{MarcaId}")]
        public ActionResult<Marca> Marcas(int MarcaId)
        {
            Marca cl = (from tbl in _contexto.Marcas where tbl.Id == MarcaId select new Marca() { Id = tbl.Id, Nombre = tbl.Nombre }).FirstOrDefault();
            if (cl == null)
            {
                return NotFound(MarcaId);
            }
            return _mapper.Map<Marca>(cl);
        }

        [HttpPost("nuevo")]
        public ActionResult nuevo(MarcaDTO nuevaMarca)
        {
            Marca _marca = _mapper.Map<Marca>(nuevaMarca);

            _contexto.Marcas.Add(_marca);
            _contexto.SaveChanges();

            nuevaMarca.Id = _marca.Id;

            return Accepted(nuevaMarca);
        }

        [HttpPut("actualizar")]
        public ActionResult actualizar(MarcaDTO actualizaMarca)
        {

            Marca _marca = (from h in _contexto.Marcas where h.Id == actualizaMarca.Id select h).FirstOrDefault();

            if (_marca == null)
            {
                return NotFound(actualizaMarca);
            }
            _marca.Nombre = actualizaMarca.Nombre;

            _contexto.Marcas.Update(_marca);
            _contexto.SaveChanges();

            return Accepted(actualizaMarca);
        }
        [HttpDelete("eliminar/{MarcaId}")]
        public ActionResult eliminar(int MarcaId)
        {
            Marca _marca = (from h in _contexto.Marcas where h.Id == MarcaId select h).FirstOrDefault();

            if (_marca == null)
            {
                return NotFound(MarcaId);
            }

            _contexto.Marcas.Remove(_marca);
            _contexto.SaveChanges();

            return Accepted(MarcaId);
        }
    }
}