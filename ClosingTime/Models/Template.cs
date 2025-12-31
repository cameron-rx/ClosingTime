using System;
using System.ComponentModel.DataAnnotations;

namespace ClosingTime.Models;

public class Template
{
    public int Id {get; set;}

    public string Name {get; set;}

    public List<TemplateSection> Sections {get; set;} = new();

}
