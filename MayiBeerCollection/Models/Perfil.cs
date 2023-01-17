using System;
using System.Collections.Generic;

namespace MayiBeerCollection.Models;

public partial class Perfil
{
    public int Id { get; set; }

    public string Descripcion { get; set; }

    public virtual ICollection<Usuario> Usuarios { get; } = new List<Usuario>();
}
