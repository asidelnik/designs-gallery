using amos_test.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace amos_test.Controllers;

public class HomeController : Controller
{
  private readonly ILogger<HomeController> _logger;

  public HomeController(ILogger<HomeController> logger)
  {
    _logger = logger;
  }

  public IActionResult Index()
  {
    var designs = ReadAndDeserializeJson();
    ViewData["Designs"] = designs;
    return View();
  }

  public IActionResult Privacy()
  {
    return View();
  }

  private List<DesignModel>? ReadAndDeserializeJson()
  {
    var jsonFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "FakeData", "data.json");
    var json = System.IO.File.ReadAllText(jsonFilePath);
    var data = JsonConvert.DeserializeObject<DesignsModel>(json);
    return data?.Designs;
  }
}
