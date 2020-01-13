using System;
using System.Collections.Generic;
using System.Text;

namespace Model
{
    public interface ISecurityModel
    {
    ICollection<SecurityEntry> SecurityEntries { get; set; }
    }

    public enum SecurityStrategyEnum
    {
        allow,
        deny
    }
}
