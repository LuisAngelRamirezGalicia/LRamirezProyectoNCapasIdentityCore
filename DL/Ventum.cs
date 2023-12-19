using System;
using System.Collections.Generic;

namespace DL;

public partial class Ventum
{
    public int IdVenta { get; set; }

    public string? IdCliente { get; set; }

    public decimal Total { get; set; }

    public int? IdMetodoPago { get; set; }

    public DateTime Fecha { get; set; }

    public virtual AspNetUser? IdClienteNavigation { get; set; }

    public virtual MetodoPago? IdMetodoPagoNavigation { get; set; }
}
