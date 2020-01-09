using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Model
{
    public interface IDeleteModel
    {
        bool _IsDeleted { get; set; }
        string _DeletedUser { get; set; }
        DateTime _DeleteDateTime { get; set; }
        
    }
}
