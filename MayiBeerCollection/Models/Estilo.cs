using System;
using System.Collections.Generic;

namespace MayiBeerCollection.Models;

public partial class Estilo
{
    public int Id { get; set; }

    public string Nombre { get; set; } = null!;

    public virtual ICollection<Cerveza> Cervezas { get; } = new List<Cerveza>();
}
