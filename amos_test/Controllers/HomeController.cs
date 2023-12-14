using amos_test.Interfaces;
using amos_test.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace amos_test.Controllers;

public class HomeController : Controller
{
  private readonly ILogger<HomeController> _logger;
  private readonly IDesignService _designService;

  public HomeController(ILogger<HomeController> logger, IDesignService designService)
  {
    _logger = logger;
    _designService = designService;
  }

  public IActionResult Index()
  {
    var designs = _designService.GetDesignsFiltered(null);
    ViewData["designs"] = designs;
    return View();
  }

  public IActionResult Privacy()
  {
    return View();
  }
}