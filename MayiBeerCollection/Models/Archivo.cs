using System;
using System.Collections.Generic;

namespace MayiBeerCollection.Models;

public partial class Archivo
{
    public int Id { get; set; }

    public byte[]? Archivo1 { get; set; }

    public virtual ICollection<Cerveza> Cerveza { get; } = new List<Cerveza>();

    public virtual ICollection<Estilo> Estilo { get; } = new List<Estilo>();

    public virtual ICollection<Marca> Marca { get; } = new List<Marca>();

    public virtual ICollection<Pais> Pais { get; } = new List<Pais>();
}
