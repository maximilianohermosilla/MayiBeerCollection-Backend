using AutoMapper;
using MayiBeerCollection.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Numerics;
using System.Linq;
using System.Threading.Tasks;
using MayiBeerCollection.DTO;
using static System.Collections.Specialized.BitVector32;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using System.Data;

#nullable disable
namespace MayiBeerCollection.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class CiudadController : ControllerBase
    {
        private MayiBeerCollectionContext _contexto;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;

        public CiudadController(MayiBeerCollectionContext context, IConfiguration configuration, IMapper mapper)
        {
            _contexto = context;
            _configuration = configuration;
            _mapper = mapper;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Ciudad>> Ciudades()
        {
            List<Ciudad> lst = (from tbl in _contexto.Ciudads where tbl.Id > 0 select tbl).ToList();

            List<CiudadDTO> ciudadesDTO = _mapper.Map<List<CiudadDTO>>(lst);

            foreach (var item in ciudadesDTO)
            {
                Pai _pais = (from h in _contexto.Pais where h.Id == item.IdPais select h).FirstOrDefault();
                if (_pais != null)
                {
                    item.PaisNombre = _pais.Nombre;
                }
            }

            return Accepted(ciudadesDTO);
        }

        [HttpGet("buscarPais/{PaisId}")]
        public ActionResult<IEnumerable<Ciudad>> CiudadesByPais(int PaisId)
        {
            Pai _pais = (from h in _contexto.Pais where h.Id == PaisId select h).FirstOrDefault();
            List<Ciudad> lst = (from tbl in _contexto.Ciudads where tbl.IdPais == PaisId select tbl).ToList();
            List<CiudadDTO> ciudadesDTO = _mapper.Map<List<CiudadDTO>>(lst);


            if (lst == null || _pais == null)
            {
                return NotFound(PaisId);
            }


            foreach (var item in ciudadesDTO)
            {                
                if (_pais != null)
                {
                    item.PaisNombre = _pais.Nombre;
                }
            }

            return Accepted(ciudadesDTO);
        }

        [HttpGet("buscar/{CiudadId}")]
        public ActionResult<Ciudad> Ciudades(int CiudadId)
        {
            Ciudad cl = (from tbl in _contexto.Ciudads where tbl.Id == CiudadId select tbl).FirstOrDefault();

            if (cl == null)
            {
                return NotFound(CiudadId);
            }
            CiudadDTO ciudadDTO = _mapper.Map<CiudadDTO>(cl);

            Pai _pais = (from h in _contexto.Pais where h.Id == cl.IdPais select h).FirstOrDefault();
            if (_pais != null)
            {
                ciudadDTO.PaisNombre = _pais.Nombre;
            }

            return Accepted(ciudadDTO);
        }

        [HttpPost("nuevo")]
        [Authorize(Roles = "Administrador")]
        public ActionResult nuevo(CiudadDTO nuevo)
        {
            Ciudad _ciudad = _mapper.Map<Ciudad>(nuevo);

            _contexto.Ciudads.Add(_ciudad);
            _contexto.SaveChanges();

            nuevo.Id = _ciudad.Id;

            return Accepted(nuevo);
        }

        [HttpPut("actualizar")]
        [Authorize(Roles = "Administrador")]
        public ActionResult actualizar(CiudadDTO actualiza)
        {

            Ciudad _ciudad = (from h in _contexto.Ciudads where h.Id == actualiza.Id select h).FirstOrDefault();

            if (_ciudad == null)
            {
                return NotFound(actualiza);
            }

            _ciudad.Nombre = actualiza.Nombre;
            _ciudad.IdPais = actualiza.IdPais;

            _contexto.Ciudads.Update(_ciudad);
            _contexto.SaveChanges();

            return Accepted(actualiza);
        }
        [HttpDelete("eliminar/{CiudadId}")]
        [Authorize(Roles = "Administrador")]
        public ActionResult eliminar(int CiudadId)
        {
            Ciudad _ciudad = (from h in _contexto.Ciudads where h.Id == CiudadId select h).FirstOrDefault();

            if (_ciudad == null)
            {
                return NotFound(CiudadId);
            }

            List<Cerveza> _cervezas = (from tbl in _contexto.Cervezas where tbl.IdCiudad == CiudadId select tbl).ToList();
            if (_cervezas.Count() > 0)
            {
                return BadRequest("No se puede eliminar la ciudad porque tiene una o más cervezas asociadas");
            }

            _contexto.Ciudads.Remove(_ciudad);
            _contexto.SaveChanges();

            return Accepted(CiudadId);
        }
    }
}