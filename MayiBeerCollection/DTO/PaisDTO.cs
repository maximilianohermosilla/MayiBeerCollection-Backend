namespace MayiBeerCollection.DTO
{
    #nullable disable
    public class PaisDTO
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = null!;

        public List<Ciudad>? ciudades { get; set; }
    }
}
