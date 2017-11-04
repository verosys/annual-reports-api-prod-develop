using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace AnnualReportsAPI.Models
{
  public class Client
  {
    private List<GfiUpload> _gfiUploads;

    public string Oib { get; set; }
    public string Name { get; set; }
    public string Place { get; set; }
    public string County { get; set; }
    public string City { get; set; }
    public IEnumerable<GfiUpload> GfiUploads
    {
      get
      {
        return _gfiUploads;
      }
      set
      {
        if (value != null)
          _gfiUploads = value.ToList<GfiUpload>();
      }
    }

    public Client()
    {
      _gfiUploads = new List<GfiUpload>();
    }

    public void Update(string name, string place, string county, string city)
    {
      Name = name;
      Place = place;
      County = county;
      City = city;
    }

    public void AddGfiUpload(GfiUpload gfi)
    {
      if (gfi == null)
        return;

      int index = this._gfiUploads.FindIndex(g => g.Year == gfi.Year);
      if (index != -1)
      {
        this._gfiUploads[index] = gfi;
      }
      else
      {
        this._gfiUploads.Add(gfi);
      }
    }
  }

  public class ClientDTO
  {
    public string Oib { get; set; }
    public string Name { get; set; }
    public string Place { get; set; }
    public string County { get; set; }
    public string City { get; set; }
    public IEnumerable<GfiUpload> GfiUploads { get; set; }
  }

  public class GfiUpload
  {
    public string Oib { get; set; }
    public int Year { get; set; }
    public string Filename { get; set; }
    public string CompanyName { get; set; }
    public string Period { get; set; }
    public string ActivityCode { get; set; }
    public string ActivityName { get; set; }
    public string SubjectTypeCode { get; set; }
    public string SubjectTypeName { get; set; }
  }
}
