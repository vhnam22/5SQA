using _5S_QA_Entities.Abstracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _5S_QA_Entities.Models;

public class ProductForCbbViewModel : AuditableEntity
{
    public string Code { get; set; }

    public string Name { get; set; }

    public string CodeName { get; set; }

    public string Description { get; set; }

    public string ImageUrl { get; set; }

    public int? Sample { get; set; }

    public int? SampleMax { get; set; }

    public Guid? TemplateId { get; set; }

    public string TemplateName { get; set; }

    public int? TotalMeas { get; set; }
}