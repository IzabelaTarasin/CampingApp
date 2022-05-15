using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace CampingApp_Server.Controllers
{
    [Authorize(Roles = "Business")]
    [ApiController]
    [Route("[controller]")]
    public class PlaceController : ControllerBase
    {
        [HttpPost]
		public async Task<IActionResult> AddPlace()
		{
            return Ok("Dodano place");
		}

	}
}

