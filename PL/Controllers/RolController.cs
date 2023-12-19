using Microsoft.AspNetCore.Mvc;

namespace PL.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using ML;
    using System.ComponentModel.DataAnnotations;

    namespace PLIdentity.Controllers
    {
        [Authorize(Roles = "Administrador")]
        public class RolController : Controller
        {
            private RoleManager<IdentityRole> roleManager;
            public RolController(RoleManager<IdentityRole> roleMgr)
            {
                roleManager = roleMgr;
            }

            [HttpGet]
            public ActionResult GetAll()
            {
                var Roles = roleManager.Roles.ToList();
                return View(Roles);
            }

            [HttpGet]
            //public ActionResult Asignar(Guid IdRole, string Name)
            public ActionResult Asignar()
            {
                ML.Result result = BL.IdentityUser.GetAll();
                ML.UserIdentity user = new ML.UserIdentity();
                if (result.Correct)
                {
                    user.IdentityUsers = result.Objects;
                }
                user.Rol = new ML.Rol();
                ML.Result resultRol = BL.Rol.GetAll();
                user.Rol.Roles = resultRol.Objects;
                //user.Rol.Name = Name;
                // user.Rol.RoleId = IdRole;

                return View(user);
            }

            [HttpPost]
            public IActionResult Asignar(ML.UserIdentity user)
            {
                ML.Result result = BL.IdentityUser.Asignar(user);
                if (result.Correct)
                {
                    ViewBag.Message = "Todo bien";

                }
                else
                {
                    ViewBag.Message = result.ErrorMessage;
                }

                return PartialView("Modal");
            }

            [HttpPost]
            public async Task<IActionResult> Form([Required] Microsoft.AspNetCore.Identity.IdentityRole rol)
            {

                if (ModelState.IsValid)
                {
                    IdentityRole role = await roleManager.FindByIdAsync(rol.Id.ToString());
                    //Add o Insert
                    if (role == null)
                    {
                        IdentityResult result = await roleManager.CreateAsync(new IdentityRole(rol.Name));
                        if (result.Succeeded)
                        {
                            return RedirectToAction("GetAll");
                        }
                        else
                        {

                        }
                    }
                    else //Update
                    {
                        role.Id = await roleManager.GetRoleIdAsync(rol);
                        role.Name = await roleManager.GetRoleNameAsync(rol);

                        IdentityResult result = await roleManager.UpdateAsync(role);
                        if (result.Succeeded)
                        {
                            return RedirectToAction("GetAll");
                        }
                    }
                }
                return View(rol);
            }

            [HttpGet]
            public IActionResult Form(Guid IdRole, string Name)
            {
                IdentityRole role = new IdentityRole();
                if (Name != null)
                {
                    role.Name = Name;
                    role.Id = IdRole.ToString();
                    return View(role);
                }
                else
                {
                    return View(role);
                }

            }

            public async Task<IActionResult> Delete(Guid IdRole)
            {
                IdentityRole role = await roleManager.FindByIdAsync(IdRole.ToString());
                if (role != null)
                {
                    IdentityResult result = await roleManager.DeleteAsync(role);
                    if (result.Succeeded)
                        return RedirectToAction("GetAll");
                    //else
                    //    Errors(result);
                }
                else
                    ModelState.AddModelError("", "No role found");
                return View("GetAll", roleManager.Roles);
            }

        }
        }
    }