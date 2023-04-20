using Application_Dev.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System;

namespace Application_Dev.DTOs.Responses
{
    public class BookView
    {
    public int Id { get; set; }

    public string NameBook { get; set; }
    public int QuantityBook { get; set; }
    public int PriceBook { get; set; }
    public string DescriptionBook { get; set; }
    public string ImageBook { get; set; }

  }
}
