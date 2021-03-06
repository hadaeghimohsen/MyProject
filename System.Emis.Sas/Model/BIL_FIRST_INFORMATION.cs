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
    
    public partial class BIL_FIRST_INFORMATION
    {
        public BIL_FIRST_INFORMATION()
        {
            this.BIL_BILL_AMOUNT = new HashSet<BIL_BILL_AMOUNT>();
            this.BIL_CONSUMPTION_DETAIL = new HashSet<BIL_CONSUMPTION_DETAIL>();
            this.BIL_FIRST_INFORMATION1 = new HashSet<BIL_FIRST_INFORMATION>();
            this.BIL_TARIFF = new HashSet<BIL_TARIFF>();
        }
    
        public string CYCL_YEAR { get; set; }
        public string PRVN_CODE { get; set; }
        public string LETT_NO { get; set; }
        public System.DateTime LETT_DATE { get; set; }
        public string ACTV_STAT { get; set; }
        public string RECD_STAT { get; set; }
        public string FINF_TYPE { get; set; }
        public System.DateTime STRT_DATE { get; set; }
        public System.DateTime END_DATE { get; set; }
        public string ARCH_STAT { get; set; }
        public string TRAN_STAT { get; set; }
        public string CRET_BY { get; set; }
        public System.DateTime CRET_DATE { get; set; }
        public string FINF_CYCL_YEAR { get; set; }
        public string FINF_PRVN_CODE { get; set; }
        public string FINF_LETT_NO { get; set; }
        public Nullable<System.DateTime> MDFY_END_DATE { get; set; }
        public string CMNT { get; set; }
    
        public virtual ICollection<BIL_BILL_AMOUNT> BIL_BILL_AMOUNT { get; set; }
        public virtual ICollection<BIL_CONSUMPTION_DETAIL> BIL_CONSUMPTION_DETAIL { get; set; }
        public virtual ICollection<BIL_FIRST_INFORMATION> BIL_FIRST_INFORMATION1 { get; set; }
        public virtual BIL_FIRST_INFORMATION BIL_FIRST_INFORMATION2 { get; set; }
        public virtual ICollection<BIL_TARIFF> BIL_TARIFF { get; set; }
        public virtual CYCLE CYCLE { get; set; }
    }
}
