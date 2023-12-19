using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL
{
    public class Departamento
    {
        public static ML.Result GetAll()
        {
            ML.Result result = new ML.Result();
            try
            {
                using (DL.LramirezProyectoNcapasIdentityCoreContext context = new DL.LramirezProyectoNcapasIdentityCoreContext())
                {
                    var query = (from departamento in context.Departamentos
                                 
                                 select new
                                 {
                                     IdDepartamentos = departamento.IdDepartamento,
                                     Nombre = departamento.Nombre

                                 }).ToList();

                    if (query.Count > 0)
                    {
                        result.Objects = new List<object>();
                        foreach (var productoQuery in query)
                        {
                            ML.Departamento departamento = new ML.Departamento();
                            departamento.IdDepartamento = productoQuery.IdDepartamentos;
                            departamento.Nombre = productoQuery.Nombre;


                            result.Objects.Add(departamento);

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

        public static ML.Result GetByIdArea(int IdArea)
        {
            ML.Result result = new ML.Result();
            try
            {
                using (DL.LramirezProyectoNcapasIdentityCoreContext context = new DL.LramirezProyectoNcapasIdentityCoreContext())
                {
                    var query = (from departamento in context.Departamentos
                                 join area in context.Areas on departamento.IdArea equals area.IdArea where departamento.IdArea == IdArea
                                 select new
                                 {
                                     IdDepartamentos = departamento.IdDepartamento,
                                     Nombre = departamento.Nombre

                                 }).ToList();

                    if (query.Count > 0)
                    {
                        result.Objects = new List<object>();
                        foreach (var productoQuery in query)
                        {
                            ML.Departamento departamento = new ML.Departamento();
                            departamento.IdDepartamento = productoQuery.IdDepartamentos;
                            departamento.Nombre = productoQuery.Nombre;


                            result.Objects.Add(departamento);

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
