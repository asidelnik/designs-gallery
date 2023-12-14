using amos_test.Models;

namespace amos_test.Interfaces
{
  public interface IDesignService
  {
    public List<DesignModel> GetDesignsFiltered(string? filter = null);
  }
}