using amos_test.Interfaces;
using amos_test.Models;
using Microsoft.AspNetCore.Mvc;

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

  public IActionResult Index(string filter, string? errorMessage = null)
  {
    var designs = _designService.GetDesignsFiltered(filter);
    ViewData["filter"] = filter;
    ViewData["designs"] = designs;
    ViewData["errorMessage"] = errorMessage;
    return View();
  }

  [HttpPost]
  public IActionResult UpdateFilteredDesignsTexts(string? replaceAll, string? filter)
  {
    try
    {
      if (string.IsNullOrEmpty(replaceAll)) throw new ArgumentException("Missing parameter: replace text");
      List<DesignModel> designs = _designService.GetDesignsFiltered(filter);
      List<int> filteredDesignsIds = designs.Select(d => d.Id).ToList();
      _designService.UpdateAllFilteredDesigns(filteredDesignsIds, replaceAll);
      return RedirectToAction(nameof(Index), "Home", new { filter });
    }
    catch (Exception ex)
    {
      _logger.LogError(ex, string.Format("HomeController - UpdateFilteredDesignsTexts: {0}", string.IsNullOrEmpty(ex.Message) ? "No error message" : ex.Message));
      return RedirectToAction(nameof(Index), "Home", new { filter, errorMessage = ex.Message });
    }
  }

  [HttpPost]
  public IActionResult DeleteDesignById(int? id, string filter)
  {
    try
    {
      if (id == null) throw new ArgumentException("Missing parameter: design id");
      _designService.DeleteDesignById((int)id);
      return RedirectToAction(nameof(Index), "Home", new { filter });
    }
    catch (Exception ex)
    {
      _logger.LogError(ex, string.Format("HomeController - DeleteDesignById: {0}", string.IsNullOrEmpty(ex.Message) ? "No error message" : ex.Message));
      return RedirectToAction(nameof(Index), "Home", new { filter });
    }
  }

  [HttpPost]
  public IActionResult UpdateDesignById(int? id, string? replace, string filter)
  {
    try
    {
      if (id == null) throw new ArgumentException("Missing parameter: design id");
      if (string.IsNullOrEmpty(replace)) throw new ArgumentException("Missing parameter: replace text");

      _designService.UpdateDesignById((int)id, replace);
      return RedirectToAction(nameof(Index), "Home", new { filter });
    }
    catch (Exception ex)
    {
      _logger.LogError(ex, string.Format("HomeController - UpdateDesignById: {0}", string.IsNullOrEmpty(ex.Message) ? "No error message" : ex.Message));
      return RedirectToAction(nameof(Index), "Home", new { filter, errorMessage = ex.Message });
    }
  }

  public IActionResult Privacy()
  {
    return View();
  }
}