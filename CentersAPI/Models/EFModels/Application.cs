//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace CentersAPI.Models.EFModels
{
    using System;
    using System.Collections.Generic;
    
    public partial class Application
    {
        public int Id { get; set; }
        public int Userid { get; set; }
        public int CourseId { get; set; }
        public bool isPaid { get; set; }
        public bool IsIndividual { get; set; }
        public int ApplicantCount { get; set; }
        public string ApplicationStatus { get; set; }
    
        public virtual Course Course { get; set; }
        public virtual EndUser EndUser { get; set; }
    }
}
