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
    
    public partial class BIL_CONSUMPTION
    {
        public BIL_CONSUMPTION()
        {
            this.BIL_CONSUMPTION_DETAIL = new HashSet<BIL_CONSUMPTION_DETAIL>();
        }
    
        public string BILL_REGN_CODE { get; set; }
        public int BILL_SERV_FILE_NO { get; set; }
        public short BILL_BILL_NO { get; set; }
        public string BMTR_METR_TARF_TYPE { get; set; }
        public string BMTR_DAY_TYPE { get; set; }
        public short BMTR_BMSP_RWNO { get; set; }
        public short BMTR_BMSP_RECT_CODE { get; set; }
        public int PRVS_READ { get; set; }
        public string ARCH_STAT { get; set; }
        public string TRAN_STAT { get; set; }
        public string CRET_BY { get; set; }
        public System.DateTime CRET_DATE { get; set; }
        public Nullable<int> CRNT_READ { get; set; }
        public Nullable<int> CONS { get; set; }
        public Nullable<long> POWR_AMNT { get; set; }
        public Nullable<decimal> CYCL_AVRG { get; set; }
        public string MDFY_BY { get; set; }
        public Nullable<System.DateTime> MDFY_DATE { get; set; }
    
        public virtual BILL BILL { get; set; }
        public virtual ICollection<BIL_CONSUMPTION_DETAIL> BIL_CONSUMPTION_DETAIL { get; set; }
        public virtual BASE_METER_TARIFF BASE_METER_TARIFF { get; set; }
    }
}
