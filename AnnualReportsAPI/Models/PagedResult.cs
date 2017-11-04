using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AnnualReportsAPI.Models
{
  public class PagedResult<T>
  {
    public T Data { get; set; }
    public PagingDetails Pagination { get; set; }
  }

  public class PagingDetails
  {
    public int Count { get; set; }
    public int Page { get; set; }
    public int PerPage { get; set; }
    public int PageCount
    {
      get
      {
        int totalPage = (Count + PerPage - 1) / PerPage;

        return totalPage;
      }
    }
  }
}
