//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace AllWebApi.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations.Schema;

    public partial class purchase_product
    {
        public int pur_pro_id { get; set; }
        [NotMapped]
        public int pur_id { get; set; }
        public string item { get; set; }
        public int qty { get; set; }
        public double amount { get; set; }
    }
}
