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
    
    public partial class Mailbox
    {
        public int Id { get; set; }
        public string Subject { get; set; }
        public string Message { get; set; }
        public System.DateTime DateTime { get; set; }
        public bool isDelevred { get; set; }
        public bool isFromCenter { get; set; }
        public int CenterId { get; set; }
    
        public virtual Center Center { get; set; }
    }
}