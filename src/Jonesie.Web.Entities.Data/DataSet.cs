using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jonesie.Web.Entities.Data
{
  public class DataSet<T> where T : BaseEntity
  {
    public string SortColumn { get; private set; }
    public bool OrderDescending { get; private set; }

    public int Page { get; private set; }
    public int PageSize { get; private set; }
    public int PageCount
    {
      get
      {
        return Items.Count();
      }
    }
    public int TotalMatching { get; private set; }

    public int TotalPages
    {
      get
      {
        return Math.Max(TotalMatching / PageSize, 1) + ((TotalMatching % PageSize) != 0 && TotalMatching > PageSize ? 1 : 0);
      }
    }

    public List<T> Items { get; private set; }

    public DataSet(IEnumerable<T> items, int totalMatching, string sortColumn = null, bool orderDescending = false, int page = 1, int pageSize = 0)
    {
      Items = new List<T>(items);
      TotalMatching = totalMatching;
      Page = page;
      PageSize = pageSize;
      OrderDescending = orderDescending;
      SortColumn = sortColumn;
    }
  }
}
