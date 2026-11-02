using System;
using System.Collections.Generic;

namespace szabo_KonyvtarApi.Models;

public partial class Telepulesek
{
    public int Id { get; set; }

    public int Irsz { get; set; }

    public string TelepNev { get; set; } = null!;

    public int MegyeId { get; set; }

    public virtual ICollection<Konyvtarak>? Konyvtaraks { get; set; } = new List<Konyvtarak>();

    public virtual Megyek? Megye { get; set; } = null!;
}
