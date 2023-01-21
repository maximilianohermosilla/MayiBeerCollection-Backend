using System;
using System.Collections.Generic;

namespace MayiBeerCollection.Models;

public partial class Cerveza
{
    public int Id { get; set; }

    public string Nombre { get; set; } = null!;

    public double? Ibu { get; set; }

    public double? Alcohol { get; set; }

    public int IdMarca { get; set; }

    public int? IdEstilo { get; set; }

    public int? IdCiudad { get; set; }

    public string? Observaciones { get; set; }

    public int Contenido { get; set; }

    public string? Imagen { get; set; }

    public int? IdArchivo { get; set; }

    public virtual Archivo? IdArchivoNavigation { get; set; }

    public virtual Ciudad? IdCiudadNavigation { get; set; }

    public virtual Estilo? IdEstiloNavigation { get; set; }

    public virtual Marca IdMarcaNavigation { get; set; } = null!;
}
