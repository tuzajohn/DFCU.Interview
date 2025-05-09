using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DFCU.Interview.Domain.Models;

/// <summary>
/// Set base model for DB tables
/// </summary>
/// <typeparam name="T">Should be a struct type: primitive as structs are value types</typeparam>
public class BaseModel<T> where T : struct
{
    public T Id { get; set; }
    public DateTime CreatedOn { get; set; }
    public DateTime UpdatedOn { get; set; }
}
