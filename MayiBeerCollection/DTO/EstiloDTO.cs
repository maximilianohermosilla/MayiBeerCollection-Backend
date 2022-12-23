namespace MayiBeerCollection.DTO
{
    #nullable disable
    public class EstiloDTO
    {
        public int Id { get; set; }

        public string Nombre { get; set; } = null!;

        public List<CervezaDTO> Cervezas { get; set; }
    }
}
