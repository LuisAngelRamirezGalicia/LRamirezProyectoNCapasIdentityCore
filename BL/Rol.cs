using Microsoft.SqlServer.Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL
{
    public class Rol
    {
        //get all rol 

        public static ML.Result GetAll()
        {
            ML.Result result = new ML.Result();
            try
            {
                using (DL.LramirezProyectoNcapasIdentityCoreContext context = new DL.LramirezProyectoNcapasIdentityCoreContext())
                {
                    var query = (from rol in context.AspNetRoles
                                 select new
                                 {
                                     Id = rol.Id,
                                     Name = rol.Name
                                 }).ToList();



                    if (query.Count > 0)
                    {
                        result.Objects = new List<object>();
                        foreach (var item in query)
                        {
                            ML.Rol rolResult = new ML.Rol();

                            rolResult.Name = item.Name;
                            rolResult.RoleId = Guid.Parse(item.Id);

                            result.Objects.Add(rolResult);
                        }

                        result.Correct = true;
                    }
                }

            }
            catch (Exception ex)
            {
                result.Correct = false;
                result.ErrorMessage = ex.InnerException.Message;
                result.Ex = ex;
            }

            return result;
        }

    }
}
