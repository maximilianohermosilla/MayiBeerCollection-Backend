using System;
using System.Collections.Generic;

namespace MayiBeerCollection.Models;

public partial class ArchivoFilestream
{
    public int Id { get; set; }

    public Guid Idguid { get; set; }

    public string? RootDirectory { get; set; }

    public string? FileName { get; set; }

    public string? FileAttribute { get; set; }

    public DateTime? FileCreateDate { get; set; }

    public decimal? FileSize { get; set; }

    public byte[]? FileStreamCol { get; set; }
}
