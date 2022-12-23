using System;
using System.Collections.Generic;

namespace MayiBeerCollection.Models;

public partial class Pai
{
    public int Id { get; set; }

    public string Nombre { get; set; } = null!;

    public virtual ICollection<Ciudad> Ciudads { get; } = new List<Ciudad>();
}
