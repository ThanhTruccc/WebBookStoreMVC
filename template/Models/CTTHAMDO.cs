//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace template.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class CTTHAMDO
    {
        public int MaCH { get; set; }
        public int STT { get; set; }
        public string NoiDungTraLoi { get; set; }
        public Nullable<int> SoLanBinhChon { get; set; }
    
        public virtual THAMDO THAMDO { get; set; }
    }
}
