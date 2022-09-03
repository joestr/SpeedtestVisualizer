using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using SpeedtestVisualizer.Misc.Contexts;
using SpeedtestVisualizer.Models;
using SpeedtestVisualizer.Models.Home;

namespace SpeedtestVisualizer.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly SpeedtestVisualizerContext _context;

    public HomeController(ILogger<HomeController> logger, SpeedtestVisualizerContext context)
    {
        _logger = logger;
        _context = context;
    }

    public IActionResult Index()
    {
        var view = new IndexViewModel();
        view.SpeedtestResults = _context.SpeedtestResults.Where(x => x.MeasuringTimestamp > DateTime.Now.AddDays(-1)).ToList();
        return View(view);
    }
}