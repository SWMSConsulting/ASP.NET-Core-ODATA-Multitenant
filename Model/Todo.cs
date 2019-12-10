using System;
using System.ComponentModel.DataAnnotations;

namespace Model
{
    public class Todo: IIndexedModel
    {
        [Key]
        public int Id { get; set; }

        public string Description {get; set;}
    }
}
