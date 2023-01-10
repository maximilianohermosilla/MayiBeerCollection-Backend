using System;
using System.Collections.Generic;

namespace MayiBeerCollection.Models;

public partial class Estilo
{
    public int Id { get; set; }

    public string Nombre { get; set; } = null!;

    public int? IdArchivo { get; set; }

    public virtual ICollection<Cerveza> Cervezas { get; } = new List<Cerveza>();

    public virtual Archivo? IdArchivoNavigation { get; set; }
}
