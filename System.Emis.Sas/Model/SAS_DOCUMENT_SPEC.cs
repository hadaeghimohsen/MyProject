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
    
    public partial class SAS_DOCUMENT_SPEC
    {
        public SAS_DOCUMENT_SPEC()
        {
            this.SAS_REQUEST_DOCUMENT = new HashSet<SAS_REQUEST_DOCUMENT>();
        }
    
        public short RWNO { get; set; }
        public string DCMT_TYPE { get; set; }
        public string KIND { get; set; }
        public string TRAN_STAT { get; set; }
        public string CRET_BY { get; set; }
        public System.DateTime CRET_DATE { get; set; }
        public string DCMT_DESC { get; set; }
        public string ORGN { get; set; }
        public string SIGN { get; set; }
        public string MDFY_BY { get; set; }
        public Nullable<System.DateTime> MDFY_DATE { get; set; }
    
        public virtual ICollection<SAS_REQUEST_DOCUMENT> SAS_REQUEST_DOCUMENT { get; set; }
    }
}
