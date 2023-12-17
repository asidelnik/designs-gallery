using amos_test.Models;

namespace amos_test.Interfaces
{
  public interface IDesignService
  {
    public List<DesignModel> GetDesignsFiltered(string? filter = null);
    public void UpdateAllFilteredDesigns(List<int>? filteredDesignsIds, string replaceAll);
    public void DeleteDesignById(int designId);
    public void UpdateDesignById(int designId, string replace);
  }
}