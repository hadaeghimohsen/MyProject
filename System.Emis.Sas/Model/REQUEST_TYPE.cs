//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace System.Emis.Sas.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class REQUEST_TYPE
    {
        public REQUEST_TYPE()
        {
            this.REQUEST = new HashSet<REQUEST>();
            this.REQUEST_ROW = new HashSet<REQUEST_ROW>();
            this.SAS_REQUEST_REQUESTER = new HashSet<SAS_REQUEST_REQUESTER>();
        }
    
        public short CODE { get; set; }
        public string RQTP_DESC { get; set; }
        public string SUB_SYS { get; set; }
        public string RQTP_TYPE { get; set; }
        public string TRAN_STAT { get; set; }
        public string MDFY_BY { get; set; }
        public Nullable<System.DateTime> MDFY_DATE { get; set; }
    
        public virtual ICollection<REQUEST> REQUEST { get; set; }
        public virtual ICollection<REQUEST_ROW> REQUEST_ROW { get; set; }
        public virtual ICollection<SAS_REQUEST_REQUESTER> SAS_REQUEST_REQUESTER { get; set; }
    }
}
