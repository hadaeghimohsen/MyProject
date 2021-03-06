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
    
    public partial class BASE_METER_TARIFF
    {
        public BASE_METER_TARIFF()
        {
            this.BIL_CONSUMPTION = new HashSet<BIL_CONSUMPTION>();
        }
    
        public int BMSP_SERV_FILE_NO { get; set; }
        public short BMSP_RECT_CODE { get; set; }
        public short BMSP_RWNO { get; set; }
        public string METR_TARF_TYPE { get; set; }
        public string DAY_TYPE { get; set; }
        public int FRST_NO { get; set; }
        public int LAST_READ_NO { get; set; }
        public int CONS_AVRG { get; set; }
        public string ARCH_STAT { get; set; }
        public string TRAN_STAT { get; set; }
        public string CRET_BY { get; set; }
        public System.DateTime CRET_DATE { get; set; }
        public Nullable<int> HOT_AVRG { get; set; }
        public Nullable<int> COLD_AVRG { get; set; }
        public Nullable<int> WARM_AVRG { get; set; }
        public string MDFY_BY { get; set; }
        public Nullable<System.DateTime> MDFY_DATE { get; set; }
    
        public virtual BASE_METER_SPEC BASE_METER_SPEC { get; set; }
        public virtual ICollection<BIL_CONSUMPTION> BIL_CONSUMPTION { get; set; }
    }
}
