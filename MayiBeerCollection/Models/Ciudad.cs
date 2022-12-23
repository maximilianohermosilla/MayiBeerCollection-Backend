using System;
using System.Collections.Generic;

namespace MayiBeerCollection.Models;

public partial class Ciudad
{
    public int Id { get; set; }

    public string Nombre { get; set; } = null!;

    public int IdPais { get; set; }

    public virtual ICollection<Cerveza> Cervezas { get; } = new List<Cerveza>();

    public virtual Pai IdPaisNavigation { get; set; } = null!;
}
