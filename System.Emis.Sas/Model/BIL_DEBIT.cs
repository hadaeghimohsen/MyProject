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
    
    public partial class BIL_DEBIT
    {
        public BIL_DEBIT()
        {
            this.BIL_BILL_DEBIT = new HashSet<BIL_BILL_DEBIT>();
        }
    
        public int SERV_FILE_NO { get; set; }
        public string DEBT_TYPE { get; set; }
        public long AMNT { get; set; }
        public string TRAN_STAT { get; set; }
    
        public virtual ICollection<BIL_BILL_DEBIT> BIL_BILL_DEBIT { get; set; }
        public virtual SERVICE SERVICE { get; set; }
    }
}
