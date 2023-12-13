using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL
{
   

        public class IdentityUser
        {
            public static ML.Result GetAll()
            {
                ML.Result result = new ML.Result();
                try
                {
                    using (DL.LramirezProyectoNcapasIdentityCoreContext  context = new DL.LramirezProyectoNcapasIdentityCoreContext())
                    {
                        var query = (from user in context.AspNetUsers
                                     select new
                                     {
                                         IdUser = user.Id,
                                         UserName = user.UserName
                                     }).ToList();



                        if (query.Count > 0)
                        {
                            result.Objects = new List<object>();
                            foreach (var item in query)
                            {
                                ML.UserIdentity usuario = new ML.UserIdentity();
                                usuario.IdUsuario = item.IdUser;
                                usuario.UserName = item.UserName;
                                result.Objects.Add(usuario);
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

            public static ML.Result Asignar(ML.UserIdentity user)
            {
                ML.Result result = new ML.Result();
                try
                {
                    using (DL.LramirezProyectoNcapasIdentityCoreContext context = new DL.LramirezProyectoNcapasIdentityCoreContext())
                    {
                        var query = context.Database.ExecuteSqlRaw($"AddAspNetUserRoles '{user.IdUsuario}', '{user.Rol.RoleId}'");

                        if (query != null)
                        {
                            result.Correct = true;
                        }
                        else
                        {
                            result.Correct = false;
                            result.ErrorMessage = "Error al hacer la asignacion";
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
