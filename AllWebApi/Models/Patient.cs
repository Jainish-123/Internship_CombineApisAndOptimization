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
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public partial class Patient
    {
        public string Pat_Id { get; set; }
        [Required]
        public string Pat_Name { get; set; }
        public int Pat_Age { get; set; }
        [NotMapped]
        public string Doc_Name { get; set; }
    }
}
