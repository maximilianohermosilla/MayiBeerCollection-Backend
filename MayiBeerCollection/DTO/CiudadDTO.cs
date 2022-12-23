using Microsoft.Extensions.Hosting;

namespace MayiBeerCollection.DTO
{
    #nullable disable
    public class CiudadDTO
    {        
        public int Id { get; set; }

        public string Nombre { get; set; } = null!;

        public int IdPais { get; set; }

        public string PaisNombre { get; set; }

        public List<CervezaDTO> Cervezas { get; set; }
    }
}
