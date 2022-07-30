using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Vista_Login.Areas.Usuario.Pages.Account
{
    public class RegistrosModel : PageModel
    {
        private UserManager<IdentityUser> userManager;


        private static InputModel _input = null;
        //contructor para usar al modelo de la migracion
        public RegistrosModel(UserManager<IdentityUser> userManager)
        {
            this.userManager = userManager;
        }
        public void OnGet()
        {
            if (_input != null)
            {
                Input = _input;
            }
        }
        [BindProperty]
        public InputModel Input { get; set; }// "Input" permite acceder a las propiedades del modelo.
        public class InputModel
        {
            [Required(ErrorMessage = "Informacion necesaria")]//no puede ser nulo y se puede personalizar por que es una clase que tienen sobre carga de metodos
            [Display(Name = "Name")]//lo que el usuario visualiza en la pagina
            public string Name { get; set; }//campo dela base de datos

            [Required(ErrorMessage = "Informacion necesaria")]
            [EmailAddress]//propiedad que define que es uncorreo
            [Display(Name = "Mail")]
            public string Mail { get; set; }

            [Required(ErrorMessage = "Informacion necesaria")]
            [DataType(DataType.Password)]//da formato de contraseña al campo ******
            [Display(Name = "Contraseña")]
            [StringLength(100, ErrorMessage = "Muy corta", MinimumLength = 6)]//limita los caracteres que el usuario puede usar
            public string Password { get; set; }

            [Required(ErrorMessage = "Informacion necesaria")]
            [DataType(DataType.Password)]
            [Compare("Password", ErrorMessage = "Not match.")]
            public string ConfirmPassword { get; set; }

            public string ErrorMessage { get; set; }
        }
        //IActionResult retorna la pagina
        public async Task<IActionResult> OnPostAsync()//obtener informacion de la pagina usando (method post)
        {
            if(await RegisterUserAsync())
            {
                return Redirect("/Principal/Principal?area=Principal");
            }
            else
            {
                return Redirect("/Usuario/Registro");
            }
        }
        private async Task<bool> RegisterUserAsync()
        {
            var run = false;
            if (ModelState.IsValid)//Propiedad (ModelState) usa a [Required] antes definido en el model DB 
            {
                var userList = userManager.Users.Where(u => u.UserName.Equals(Input.Name)).ToList();
                if (userList.Count.Equals(0))
                {
                    var user = new IdentityUser
                    {
                        //registro del NombreUsuario y Imail
                        UserName = Input.Name,
                        Email = Input.Mail
                    };
                    //registro de la contraseña encriptada
                    var result = await userManager.CreateAsync(user, Input.Password);
                    if (result.Succeeded)
                    {
                        run = true;
                    }
                    else
                    {
                        foreach (var item in result.Errors)
                        {
                            Input = new InputModel
                            {
                                ErrorMessage = item.Description,
                                Mail=Input.Mail
                            };
                        }
                        _input = Input;
                        run = false;
                    }
                }

                else
                {
                    Input = new InputModel
                    {
                        //si la cuenta ya existe
                        ErrorMessage = $"El {Input.Mail} Ya esta registrado",
                        Mail = Input.Mail
                    };
                    _input = Input;
                    run = false;
                }
            }
            return run;
        }
    }
}
