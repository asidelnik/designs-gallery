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
      var json = File.ReadAllText(jsonFilePath);
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

    public void DeleteDesignById(int designId)
    {
      var jsonFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "FakeData", "data.json");
      var json = File.ReadAllText(jsonFilePath);
      var data = JsonConvert.DeserializeObject<DesignsModel>(json);
      List<DesignModel> designs = data?.Designs ?? new List<DesignModel>();

      var design = designs.FirstOrDefault(d => d.Id == designId);
      if (design != null)
      {
        data?.Designs?.Remove(design);
        var newJson = JsonConvert.SerializeObject(data);
        File.WriteAllText(jsonFilePath, newJson);
      }
    }

    public void UpdateDesignById(int designId, string replace)
    {
      var jsonFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "FakeData", "data.json");
      var json = File.ReadAllText(jsonFilePath);
      var data = JsonConvert.DeserializeObject<DesignsModel>(json);
      List<DesignModel> designs = data?.Designs ?? new List<DesignModel>();

      var design = designs.FirstOrDefault(d => d.Id == designId);
      if (design != null && design.DataNew != null)
      {
        design.DataNew = UpdateDataTextsWithReplaceText(design.DataNew, replace);
        var newJson = JsonConvert.SerializeObject(data);
        File.WriteAllText(jsonFilePath, newJson);
      }
    }

    private string? UpdateDataTextsWithReplaceText(string dataNew, string replace)
    {
      var data = JsonConvert.DeserializeObject<DataNew>(dataNew);
      if (data?.frontPage == null || data?.innerPage == null || data?.backPage == null) return null;

      data.frontPage.generatedElementsValues = UpdateGeneratedElementsContentText(data.frontPage.generatedElementsValues, replace);
      data.frontPage.elements = UpdateElementsContentText(data.frontPage.elements, replace);

      data.innerPage.generatedElementsValues = UpdateGeneratedElementsContentText(data.innerPage.generatedElementsValues, replace);
      data.innerPage.elements = UpdateElementsContentText(data.innerPage.elements, replace);

      data.backPage.generatedElementsValues = UpdateGeneratedElementsContentText(data.backPage.generatedElementsValues, replace);
      data.backPage.elements = UpdateElementsContentText(data.backPage.elements, replace);

      return JsonConvert.SerializeObject(data);
    }

    private Dictionary<string, PageText>? UpdateGeneratedElementsContentText(Dictionary<string, PageText>? generatedElements, string replace)
    {
      if (generatedElements != null)
      {
        foreach (KeyValuePair<string, PageText> elem in generatedElements)
        {
          elem.Value.text = UpdateTextContentText(elem.Value.text, replace);
        }
      }
      return generatedElements;
    }

    private List<Element>? UpdateElementsContentText(List<Element>? elements, string replace)
    {
      if (elements != null)
      {
        foreach (var elem in elements)
        {
          if (elem.textProps != null)
          {
            elem.textProps.text = UpdateTextContentText(elem.textProps?.text, replace);
          }
        }
      }
      return elements;
    }

    private TextContent? UpdateTextContentText(TextContent? root, string replace)
    {
      if (root == null)
      {
        return null;
      }

      if (root.type == "text")
      {
        root.text = replace;
      }

      if (root.content != null)
      {
        foreach (var node in root.content)
        {
          node.text = UpdateTextContentText(node, replace)?.text;
        }
      }
      return root;
    }
  }
}