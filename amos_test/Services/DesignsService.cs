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

          if (DoGeneratedElementsContainFilter(dataNew?.frontPage?.generatedElementsValues, filter)) return true;
          if (DoElementsContainFilter(dataNew?.frontPage?.elements, filter)) return true;

          if (DoGeneratedElementsContainFilter(dataNew?.innerPage?.generatedElementsValues, filter)) return true;
          if (DoElementsContainFilter(dataNew?.innerPage?.elements, filter)) return true;

          if (DoGeneratedElementsContainFilter(dataNew?.backPage?.generatedElementsValues, filter)) return true;
          if (DoElementsContainFilter(dataNew?.backPage?.elements, filter)) return true;

          return false;
        }).ToList() ?? [];
      }
      return designs;
    }

    private bool DoGeneratedElementsContainFilter(Dictionary<string, PageText>? generatedElements, string filter)
    {
      if (generatedElements != null)
      {
        foreach (KeyValuePair<string, PageText> elem in generatedElements)
        {
          var searchFound = DoesTextContentContainFilter(elem.Value.text, filter);
          if (searchFound) return true;
        }
      }
      return false;
    }

    private bool DoElementsContainFilter(List<Element>? elements, string filter)
    {
      if (elements != null)
      {
        foreach (var elem in elements)
        {
          var searchFound = DoesTextContentContainFilter(elem?.textProps?.text, filter);
          if (searchFound) return true;
        }
      }
      return false;
    }

    private bool DoesTextContentContainFilter(TextContent? root, string filter)
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
          if (DoesTextContentContainFilter(node, filter))
          {
            return true;
          }
        }
      }

      return false;
    }

  }
}