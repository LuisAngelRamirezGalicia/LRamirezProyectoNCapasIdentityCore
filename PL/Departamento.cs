using System;
using System.Collections.Generic;

namespace PL;

public partial class Departamento
{
    public int IdDepartamento { get; set; }

    public string? Nombre { get; set; }

    public int? IdArea { get; set; }

    public virtual Area? IdAreaNavigation { get; set; }

    public virtual ICollection<Producto> Productos { get; set; } = new List<Producto>();
}
