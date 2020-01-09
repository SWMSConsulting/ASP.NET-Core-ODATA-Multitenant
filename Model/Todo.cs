using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Model
{
    public class Todo: ISecurityModel, IIndexedModel, IDeleteModel
    {
        [Key]
        public int Id { get; set; }
        public string Description {get; set;}
        public ICollection<SecurityEntry> SecurityEntries { get; set; }
        bool IDeleteModel._IsDeleted { get; set; }
        string IDeleteModel._DeletedUser { get; set; }
        DateTime IDeleteModel._DeleteDateTime { get; set; }
    }
}
