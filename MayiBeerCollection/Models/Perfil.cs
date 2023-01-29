using System;
using System.Collections.Generic;

namespace MayiBeerCollection.Models;

public partial class Perfil
{
    public int Id { get; set; }

    public string Descripcion { get; set; } = null!;

    public virtual ICollection<Usuario> Usuario { get; } = new List<Usuario>();
}
