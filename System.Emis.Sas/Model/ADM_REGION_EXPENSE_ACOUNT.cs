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
    
    public partial class ADM_REGION_EXPENSE_ACOUNT
    {
        public string REGL_CYCL_YEAR { get; set; }
        public string REGN_CODE { get; set; }
        public short REGL_CODE { get; set; }
        public int EXTP_CODE { get; set; }
        public short BNKA_BNKB_BANK_CODE { get; set; }
        public int BNKA_BNKB_CODE { get; set; }
        public string BNKA_ACNT_NUMB { get; set; }
        public short LEVL_NO { get; set; }
        public string ARCH_STAT { get; set; }
        public string TRAN_STAT { get; set; }
        public string CRET_BY { get; set; }
        public System.DateTime CRET_DATE { get; set; }
    
        public virtual BANK_ACCOUNT BANK_ACCOUNT { get; set; }
        public virtual EXPENSE_TYPE EXPENSE_TYPE { get; set; }
        public virtual REGULATION REGULATION { get; set; }
        public virtual REGION REGION { get; set; }
    }
}
