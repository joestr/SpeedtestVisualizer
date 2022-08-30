﻿using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using SpeedtestVisualizer.Misc.Contexts;
using SpeedtestVisualizer.Models;
using SpeedtestVisualizer.Models.Home;

namespace SpeedtestVisualizer.Controllers;

public class ErrorController : Controller
{
    private readonly ILogger<ErrorController> _logger;
    
    public ErrorController(ILogger<ErrorController> logger)
    {
        _logger = logger;
    }
    
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Index()
    {
        return View(new ErrorViewModel {RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier});
    }
}