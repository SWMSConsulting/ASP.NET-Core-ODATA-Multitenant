using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Model
{
    public interface IIndexedModel
    {
        int Id { get; set; }
        
    }
}
