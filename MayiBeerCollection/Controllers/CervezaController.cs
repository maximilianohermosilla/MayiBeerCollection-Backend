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
using System.Text;

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
            try
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

                    Archivo _archivo = (from h in _contexto.Archivos where h.Id == item.IdArchivo select h).FirstOrDefault();
                    if (_archivo != null)
                    {
                        string stringArchivo = Encoding.ASCII.GetString(_archivo.Archivo1);
                        item.Imagen = stringArchivo;
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
            catch (Exception ex)
            {
                return BadRequest(ex.Message);                
            }

        }
        [HttpGet("buscar/{CervezaId}")]
        public ActionResult<Cerveza> Cervezas(int CervezaId)
        {
            try
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

                Archivo _archivo = (from h in _contexto.Archivos where h.Id == item.IdArchivo select h).FirstOrDefault();
                if (_archivo != null)
                {
                    string stringArchivo = Encoding.ASCII.GetString(_archivo.Archivo1);
                    item.Imagen = stringArchivo;
                    //item.ImageFile = _archivo.Archivo1;
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
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("nuevo")]
        public ActionResult nuevo(CervezaDTO nuevo)
        {
            try
            {
                Cerveza _cerveza = _mapper.Map<Cerveza>(nuevo);

                if (nuevo.Imagen != null)
                {
                    byte[] bytes = Encoding.ASCII.GetBytes(nuevo.Imagen);
                    Archivo newArch = new Archivo() { Archivo1 = bytes };
                    _contexto.Archivos.Add(newArch);
                    _contexto.SaveChanges();
                    _cerveza.Imagen = "";
                    _cerveza.IdArchivo = newArch.Id;
                }

                _contexto.Cervezas.Add(_cerveza);
                _contexto.SaveChanges();

                nuevo.Id = _cerveza.Id;

                return Accepted(nuevo);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("actualizar")]
        public ActionResult actualizar(CervezaDTO actualiza)
        {
            try
            {
                Cerveza _cerveza = (from h in _contexto.Cervezas where h.Id == actualiza.Id select h).FirstOrDefault();

                if (_cerveza == null)
                {
                    return NotFound(actualiza);
                }

                if (actualiza.Imagen != null)
                {
                    byte[] bytes = Encoding.ASCII.GetBytes(actualiza.Imagen);
                    Archivo arch = (from a in _contexto.Archivos where a.Id == _cerveza.IdArchivo select a).FirstOrDefault();

                    if(arch == null)
                    {
                        Archivo newArch = new Archivo() { Archivo1 = bytes };
                        _contexto.Archivos.Add(newArch);
                        _contexto.SaveChanges();
                        _cerveza.IdArchivo = newArch.Id;
                    }
                    else
                    {
                        arch.Archivo1 = bytes;
                        _contexto.Archivos.Update(arch);
                        _contexto.SaveChanges();
                    }
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
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpDelete("eliminar/{CervezaId}")]
        public ActionResult eliminar(int CervezaId)
        {
            try
            {
                Cerveza _cerveza = (from h in _contexto.Cervezas where h.Id == CervezaId select h).FirstOrDefault();

                if (_cerveza == null)
                {
                    return NotFound(CervezaId);
                }

                Archivo arch = (from a in _contexto.Archivos where a.Id == _cerveza.IdArchivo select a).FirstOrDefault();

                if (arch == null)
                {
                    _contexto.Archivos.Remove(arch);
                    _contexto.SaveChanges();
                }

                _contexto.Cervezas.Remove(_cerveza);
                _contexto.SaveChanges();

                return Accepted(CervezaId);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}