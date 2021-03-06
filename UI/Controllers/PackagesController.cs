﻿using System;
using Microsoft.AspNetCore.Mvc;
using TNDStudios.DataPortals.UI.Models;

namespace UI.Controllers
{
    [ApiExplorerSettings(IgnoreApi = true)] // Stop swagger from freaking out about the routing with no verbs
    [Route("/package")]
    public class PackagesController : Controller
    {
        [Route("{packageId}")]
        public IActionResult Transformations([FromRoute]Guid packageId)
        {
            // Show the view with the items needed for the page attached
            return View("Index", PackagePageVM.Create(packageId, Guid.Empty));
        }
    }
}