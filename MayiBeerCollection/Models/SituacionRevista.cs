using System;
using System.Collections.Generic;

namespace MayiBeerCollection.Models;

public partial class SituacionRevista
{
    public int Id { get; set; }

    public string Nombre { get; set; } = null!;

    public virtual ICollection<Expediente> Expediente { get; } = new List<Expediente>();
}
