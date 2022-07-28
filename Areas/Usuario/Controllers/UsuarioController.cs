using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Vista_Login.Areas.Usuario.Controllers
{
    [Area("Usuario")]
    public class UsuarioController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult ViewUsuario()
        {
            return View();
        }
    }
}
