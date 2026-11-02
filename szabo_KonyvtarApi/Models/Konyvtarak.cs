using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace szabo_KonyvtarApi.Models;

public partial class Konyvtarak
{
    public int Id { get; set; }

    public string KonyvtarNev { get; set; } = null!;

    public int Irsz { get; set; }

    public string Cim { get; set; } = null!;

    //[JsonIgnore]
    public virtual Telepulesek? IrszNavigation { get; set; } = null!;
}
