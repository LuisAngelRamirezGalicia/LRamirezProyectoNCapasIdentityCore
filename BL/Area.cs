using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL
{
    public class Area
    {

        public static ML.Result GetAll()
        {
            ML.Result result = new ML.Result();
            try
            {
                using (DL.LramirezProyectoNcapasIdentityCoreContext context = new DL.LramirezProyectoNcapasIdentityCoreContext())
                {
                    var query = (from area in context.Areas
                                 select new
                                 {
                                     IdArea = area.IdArea,
                                     Nombre = area.Nombre,
                                     
                                 }).ToList();

                    if (query.Count > 0)
                    {
                        result.Objects = new List<object>();
                        foreach (var productoQuery in query)
                        {
                            ML.Area area = new ML.Area();
                            area.IdArea = productoQuery.IdArea;
                            area.Nombre = productoQuery.Nombre;
                          

                            result.Objects.Add(area);

                        }
                        result.Correct = true;
                    }
                }
            }
            catch (Exception e)
            {
                result.Correct = false;
                result.ErrorMessage = e.InnerException.Message;
                result.Ex = e;
            }

            return result;
        }

    }
}
