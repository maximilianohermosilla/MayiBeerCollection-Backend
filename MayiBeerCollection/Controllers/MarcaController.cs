﻿using AutoMapper;
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
        public ActionResult<IEnumerable<MarcaDTO>> Marcas()
        {
            var lst = (from tbl in _contexto.Marcas where tbl.Id > 0 select new Marca() { Id = tbl.Id, Nombre = tbl.Nombre, IdArchivo = tbl.IdArchivo }).ToList();

            List<MarcaDTO> marcasDTO = _mapper.Map<List<MarcaDTO>>(lst);

            foreach (var item in marcasDTO)
            {
                Archivo _archivo = (from h in _contexto.Archivos where h.Id == item.IdArchivo select h).FirstOrDefault();
                if (_archivo != null)
                {
                    string stringArchivo = Encoding.ASCII.GetString(_archivo.Archivo1);
                    item.Imagen = stringArchivo;
                }
            }

            return Accepted(marcasDTO);
        }
        [HttpGet("buscar/{MarcaId}")]
        public ActionResult<MarcaDTO> Marcas(int MarcaId)
        {
            Marca cl = (from tbl in _contexto.Marcas where tbl.Id == MarcaId select new Marca() { Id = tbl.Id, Nombre = tbl.Nombre, IdArchivo = tbl.IdArchivo }).FirstOrDefault();
            if (cl == null)
            {
                return NotFound(MarcaId);
            }

            MarcaDTO item = _mapper.Map<MarcaDTO>(cl);

            Archivo _archivo = (from h in _contexto.Archivos where h.Id == item.IdArchivo select h).FirstOrDefault();

            if (_archivo != null)
            {
                string stringArchivo = Encoding.ASCII.GetString(_archivo.Archivo1);
                item.Imagen = stringArchivo;
            }

            return Accepted(item);
        }

        [HttpPost("nuevo")]
        public ActionResult nuevo(MarcaDTO nuevo)
        {
            try
            {
                Marca _marca = _mapper.Map<Marca>(nuevo);

                if (nuevo.Imagen != null)
                {
                    byte[] bytes = Encoding.ASCII.GetBytes(nuevo.Imagen);
                    Archivo newArch = new Archivo() { Archivo1 = bytes };
                    _contexto.Archivos.Add(newArch);
                    _contexto.SaveChanges();
                    _marca.IdArchivo = newArch.Id;
                }

                _contexto.Marcas.Add(_marca);
                _contexto.SaveChanges();

                nuevo.Id = _marca.Id;

                return Accepted(_marca);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }     
        }

        [HttpPut("actualizar")]
        public ActionResult actualizar(MarcaDTO actualiza)
        {
            try
            {
                Marca _marca = (from h in _contexto.Marcas where h.Id == actualiza.Id select h).FirstOrDefault();

                if (_marca == null)
                {
                    return NotFound(actualiza);
                }
                _marca.Nombre = actualiza.Nombre;

                if (actualiza.Imagen != null)
                {
                    byte[] bytes = Encoding.ASCII.GetBytes(actualiza.Imagen);
                    Archivo arch = (from a in _contexto.Archivos where a.Id == _marca.IdArchivo select a).FirstOrDefault();

                    if (arch == null)
                    {
                        Archivo newArch = new Archivo() { Archivo1 = bytes };
                        _contexto.Archivos.Add(newArch);
                        _contexto.SaveChanges();
                        _marca.IdArchivo = newArch.Id;
                    }
                    else
                    {
                        arch.Archivo1 = bytes;
                        _contexto.Archivos.Update(arch);
                        _contexto.SaveChanges();
                    }
                }

                _contexto.Marcas.Update(_marca);
                _contexto.SaveChanges();

                return Accepted(actualiza);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("eliminar/{MarcaId}")]
        public ActionResult eliminar(int MarcaId)
        {
            Marca _marca = (from h in _contexto.Marcas where h.Id == MarcaId select h).FirstOrDefault();

            if (_marca == null)
            {
                return NotFound(MarcaId);
            }

            Archivo arch = (from a in _contexto.Archivos where a.Id == _marca.IdArchivo select a).FirstOrDefault();

            if (arch != null)
            {
                _contexto.Archivos.Remove(arch);
                _contexto.SaveChanges();
            }

            _contexto.Marcas.Remove(_marca);
            _contexto.SaveChanges();

            return Accepted(MarcaId);
        }
    }
}