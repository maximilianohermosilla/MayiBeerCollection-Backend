using System;
using System.Collections.Generic;

namespace MayiBeerCollection.Models;

public partial class Usuario
{
    public int Id { get; set; }

    public string Nombre { get; set; } = null!;

    public string Login { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string? Correo { get; set; }

    public bool? Habilitado { get; set; }

    public int? IdPerfil { get; set; }

    public virtual Perfil? IdPerfilNavigation { get; set; }
}
