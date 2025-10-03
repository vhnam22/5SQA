using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _5S_QA_Entities.Models;
public class Result
{
    public string Name { get; set; }

    public int? Sample { get; set; }

    public int? Cavity { get; set; }

    public object Actual { get; set; }
}
