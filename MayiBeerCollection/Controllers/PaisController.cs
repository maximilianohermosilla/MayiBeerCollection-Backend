using AutoMapper;
using MayiBeerCollection.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Numerics;
using System.Linq;
using System.Threading.Tasks;
using MayiBeerCollection.DTO;
using System.Text;
using System.Collections.Generic;

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
        public ActionResult<IEnumerable<PaisDTO>> Pais()
        {
            List<Pai> lst = (from tbl in _contexto.Pais where tbl.Id > 0 select new Pai() { Id = tbl.Id, Nombre = tbl.Nombre, IdArchivo = tbl.IdArchivo }).ToList();

            List<PaisDTO> _paises = _mapper.Map<List<PaisDTO>>(lst);

            foreach (var item in _paises)
            {
                List<Ciudad> _ciudades = (from tbl in _contexto.Ciudads where tbl.IdPais == item.Id select tbl).ToList();
                if (_ciudades != null)
                {
                    item.ciudades = _ciudades;
                }

                Archivo _archivo = (from h in _contexto.Archivos where h.Id == item.IdArchivo select h).FirstOrDefault();
                if (_archivo != null)
                {
                    string stringArchivo = Encoding.ASCII.GetString(_archivo.Archivo1);
                    item.Imagen = stringArchivo;
                }
            }

            return _paises;
        }
        [HttpGet("buscar/{PaisId}")]
        public ActionResult<PaisDTO> Pais(int PaisId)
        {

            var cl = (from tbl in _contexto.Pais where tbl.Id == PaisId select new Pai() { Id = tbl.Id, Nombre = tbl.Nombre, IdArchivo = tbl.IdArchivo }).FirstOrDefault();
            if (cl == null)
            {
                return NotFound(PaisId);
            }

            PaisDTO item = _mapper.Map<PaisDTO>(cl);

            Archivo _archivo = (from h in _contexto.Archivos where h.Id == item.IdArchivo select h).FirstOrDefault();

            if (_archivo != null)
            {
                string stringArchivo = Encoding.ASCII.GetString(_archivo.Archivo1);
                item.Imagen = stringArchivo;
            }

            return Accepted(item);
        }

        [HttpPost("nuevo")]
        public ActionResult nuevo(PaisDTO nuevoPais)
        {
            try
            {
                Pai _pais = _mapper.Map<Pai>(nuevoPais);

                if (nuevoPais.Imagen != null)
                {
                    byte[] bytes = Encoding.ASCII.GetBytes(nuevoPais.Imagen);
                    Archivo newArch = new Archivo() { Archivo1 = bytes };
                    _contexto.Archivos.Add(newArch);
                    _contexto.SaveChanges();
                    _pais.IdArchivo = newArch.Id;
                }

                _contexto.Pais.Add(_pais);
                _contexto.SaveChanges();

                nuevoPais.Id = _pais.Id;

                return Accepted(_pais);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }   
        }

        [HttpPut("actualizar")]
        public ActionResult actualizar(PaisDTO actualiza)
        {
            try
            {
                Pai _pais = (from h in _contexto.Pais where h.Id == actualiza.Id select h).FirstOrDefault();

                if (_pais == null)
                {
                    return NotFound(actualiza);
                }
                _pais.Nombre = actualiza.Nombre;

                if (actualiza.Imagen != null)
                {
                    byte[] bytes = Encoding.ASCII.GetBytes(actualiza.Imagen);
                    Archivo arch = (from a in _contexto.Archivos where a.Id == _pais.IdArchivo select a).FirstOrDefault();

                    if (arch == null)
                    {
                        Archivo newArch = new Archivo() { Archivo1 = bytes };
                        _contexto.Archivos.Add(newArch);
                        _contexto.SaveChanges();
                        _pais.IdArchivo = newArch.Id;
                    }
                    else
                    {
                        arch.Archivo1 = bytes;
                        _contexto.Archivos.Update(arch);
                        _contexto.SaveChanges();
                    }
                }

                _contexto.Pais.Update(_pais);
                _contexto.SaveChanges();

                return Accepted(actualiza);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("eliminar/{PaisId}")]
        public ActionResult eliminar(int PaisId)
        {
            Pai _pais = (from h in _contexto.Pais where h.Id == PaisId select h).FirstOrDefault();

            if (_pais == null)
            {
                return NotFound(PaisId);
            }

            Archivo arch = (from a in _contexto.Archivos where a.Id == _pais.IdArchivo select a).FirstOrDefault();

            if (arch == null)
            {
                _contexto.Archivos.Remove(arch);
                _contexto.SaveChanges();
            }

            _contexto.Pais.Remove(_pais);
            _contexto.SaveChanges();

            return Accepted(PaisId);
        }
    }
}