﻿using System;
using Microsoft.AspNetCore.Mvc;
using TNDStudios.DataPortals.UI.Models;

namespace TNDStudios.DataPortals.UI.Controllers
{
    [ApiExplorerSettings(IgnoreApi = true)] // Stop swagger from freaking out about the routing with no verbs
    [Route("/package/{packageId}")]
    public class DataDefinitionsController : Controller
    {
        [Route("datadefinitions/{id}")]
        public IActionResult DataDefinition([FromRoute]Guid packageId, [FromRoute]Guid id)
        {
            // Show the view with the items needed for the page attached
            return View("Index", PackagePageVM.Create(packageId, id));
        }
    }
}
