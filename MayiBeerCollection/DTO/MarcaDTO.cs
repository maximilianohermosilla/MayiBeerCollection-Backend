namespace MayiBeerCollection.DTO
{
    #nullable disable
    public class MarcaDTO
    {
        public int Id { get; set; }

        public string Nombre { get; set; } = null!;

        public List<CervezaDTO> Cervezas { get; set; }
    }
}
