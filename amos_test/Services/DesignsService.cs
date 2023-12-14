using amos_test.Interfaces;
using amos_test.Models;
using Newtonsoft.Json;

namespace amos_test.Services
{
  public class DesignsService : IDesignService
  {
    public List<DesignModel> GetDesignsFiltered(string? filter = null)
    {
      // TODO - Maybe cache initial data in memory
      var jsonFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "FakeData", "data.json");
      var json = System.IO.File.ReadAllText(jsonFilePath);
      var data = JsonConvert.DeserializeObject<DesignsModel>(json);
      List<DesignModel> designs = data?.Designs ?? [];

      if (!string.IsNullOrEmpty(filter))
      {
        designs = data?.Designs?.Where(d =>
        {
          if (d?.DataNew == null) return false;
          var dataNew = JsonConvert.DeserializeObject<DataNew>(d.DataNew);
          // TODO - perhaps check on both: generatedElementsValues & elements properties
          var frontElements = dataNew?.frontPage?.generatedElementsValues;
          if (DoesPageContainFilter(frontElements, filter)) return true;
          var innerElements = dataNew?.innerPage?.generatedElementsValues;
          if (DoesPageContainFilter(innerElements, filter)) return true;
          var backElements = dataNew?.backPage?.generatedElementsValues;
          if (DoesPageContainFilter(backElements, filter)) return true;
          return false;
        }).ToList() ?? [];
      }
      return designs;
    }

    private static bool DoesPageContainFilter(Dictionary<string, PageText>? pageElements, string filter)
    {
      if (pageElements != null)
      {
        foreach (KeyValuePair<string, PageText> elem in pageElements)
        {
          var searchFound = DoesElementContainFilter(elem.Value.text, filter);
          if (searchFound) return true;
        }
      }
      return false;
    }

    private static bool DoesElementContainFilter(TextContent? root, string filter)
    {
      if (root == null)
      {
        return false;
      }

      if (root.type == "text" && root.text?.Contains(filter, StringComparison.OrdinalIgnoreCase) == true)
      {
        return true;
      }

      if (root.content != null)
      {
        foreach (var node in root.content)
        {
          if (DoesElementContainFilter(node, filter))
          {
            return true;
          }
        }
      }

      return false;
    }

  }
}