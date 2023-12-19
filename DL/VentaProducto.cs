using System;
using System.Collections.Generic;

namespace DL;

public partial class VentaProducto
{
    public int IdVentaProducto { get; set; }

    public int Cantidad { get; set; }

    public int? IdProducto { get; set; }

    public virtual Producto? IdProductoNavigation { get; set; }
}
