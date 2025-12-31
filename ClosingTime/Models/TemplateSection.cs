using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClosingTime.Models;

public class TemplateSection
{
    public int Id {get; set;}
    public string Name {get; set;}
    public int Position {get; set;}
    public int TemplateId {get;set;}
    public Template Template {get; set;}

    public List<TemplateItem> Items {get; set;} = new();

}
