using System;
using System.Collections.Generic;

namespace MayiBeerCollection.Models;

public partial class Archivo
{
    public int Id { get; set; }

    public byte[]? Archivo1 { get; set; }

    public virtual ICollection<Cerveza> Cervezas { get; } = new List<Cerveza>();

    public virtual ICollection<Estilo> Estilos { get; } = new List<Estilo>();

    public virtual ICollection<Marca> Marcas { get; } = new List<Marca>();

    public virtual ICollection<Pai> Pais { get; } = new List<Pai>();
}
