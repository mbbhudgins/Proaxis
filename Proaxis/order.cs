//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Proaxis
{
    using System;
    using System.Collections.Generic;
    
    public partial class order
    {
        public int orderID { get; set; }
        public string badgeID { get; set; }
        public decimal orderTotal { get; set; }
        public System.DateTime orderTimestamp { get; set; }
    
        public virtual employee employee { get; set; }
    }
}