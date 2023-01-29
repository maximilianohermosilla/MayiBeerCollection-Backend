using System;
using System.Collections.Generic;

namespace MayiBeerCollection.Models;

public partial class Pais
{
    public int Id { get; set; }

    public string Nombre { get; set; } = null!;

    public int? IdArchivo { get; set; }

    public virtual ICollection<Ciudad> Ciudad { get; } = new List<Ciudad>();

    public virtual Archivo? IdArchivoNavigation { get; set; }
}
