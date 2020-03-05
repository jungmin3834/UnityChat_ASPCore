using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ServerASP.Models;
// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ServerASP.Controllers
{
    public class ChatController : Controller
    {
        [HttpPost]
        public JsonResult Index(ChatViewModel chat)
        {
            return Json(chat);
        }
    }
}
