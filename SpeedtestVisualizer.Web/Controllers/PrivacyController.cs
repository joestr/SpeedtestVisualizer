using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using SpeedtestVisualizer.Misc.Contexts;
using SpeedtestVisualizer.Models;
using SpeedtestVisualizer.Models.Home;

namespace SpeedtestVisualizer.Controllers;

public class PrivacyController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public PrivacyController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        return View();
    }
}