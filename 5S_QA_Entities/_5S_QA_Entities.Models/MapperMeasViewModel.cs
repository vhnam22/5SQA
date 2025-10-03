using _5S_QA_Entities.Abstracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _5S_QA_Entities.Models;
public class MapperMeasViewModel : AuditableEntity
{
    public string MachineTypeName { get; set; }

    public string Code { get; set; }

    public string Name { get; set; }

    public string Value { get; set; }

    public string UnitName { get; set; }

    public double? Upper { get; set; }

    public double? Lower { get; set; }

    public string Formula { get; set; }

    public string Mapper { get; set; }
}
