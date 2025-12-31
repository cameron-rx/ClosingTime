using System;

namespace ClosingTime.Models;

public class TemplateItem
{
    public int Id {get; set;}
    public string Name {get; set;}
    public int Position {get; set;}
    public TemplateSection Section {get; set;}

}
