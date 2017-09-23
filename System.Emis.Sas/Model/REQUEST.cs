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
    
    public partial class REQUEST
    {
        public REQUEST()
        {
            this.REQUEST_ROW = new HashSet<REQUEST_ROW>();
            this.REQUEST1 = new HashSet<REQUEST>();
            this.STEP_HISTORY_SUMMERY = new HashSet<STEP_HISTORY_SUMMERY>();
        }
    
        public int RQID { get; set; }
        public string REGN_CODE_RALATE_TO { get; set; }
        public short RQTP_CODE { get; set; }
        public short RQTT_CODE { get; set; }
        public short SSTT_ROW_NO { get; set; }
        public short SSTT_MSTT_ROW_NO { get; set; }
        public string SSTT_MSTT_SUB_SYS { get; set; }
        public string CYCL_YEAR { get; set; }
        public Nullable<int> RQST_RQID { get; set; }
        public string SUNT_BUNT_CODE { get; set; }
        public Nullable<short> MTHD_CODE { get; set; }
        public string PRPI_SUB_SYS { get; set; }
        public Nullable<short> PRPI_PRSN_CODE { get; set; }
        public Nullable<short> PRPI_JOB_CODE { get; set; }
        public string PRPI_PRSN_REGN_CODE { get; set; }
        public string SUNT_BUNT_DEPT_ORGN_CODE { get; set; }
        public string SUNT_BUNT_DEPT_CODE { get; set; }
        public string SUNT_CODE { get; set; }
        public int RWNO { get; set; }
        public string SUB_SYS { get; set; }
        public System.DateTime RQST_DATE { get; set; }
        public string TIME_OUT_FLAG { get; set; }
        public string RQST_STAT { get; set; }
        public string ARCH_STAT { get; set; }
        public string TRAN_STAT { get; set; }
        public string CRET_BY { get; set; }
        public System.DateTime CRET_DATE { get; set; }
        public string REGN_CODE { get; set; }
        public Nullable<int> FILENO { get; set; }
        public string BRNC_TYPE { get; set; }
        public string NEW_OLD { get; set; }
        public string LETT_NO { get; set; }
        public Nullable<System.DateTime> LETT_DATE { get; set; }
        public string LETT_OWNR { get; set; }
        public string RECD_STAT { get; set; }
        public string RQST_DESC { get; set; }
        public string CMPS_RQST { get; set; }
        public Nullable<System.DateTime> RETN_DATE { get; set; }
        public string PLAC_ADDR { get; set; }
        public string CLMT_TYPE { get; set; }
        public string SELL_TYPE { get; set; }
        public string ADDR { get; set; }
        public string DEBT_STAT { get; set; }
        public Nullable<System.DateTime> AGRE_DATE { get; set; }
        public string AGRE_VIEW { get; set; }
        public string ORGN_IDEA { get; set; }
        public string INST_SUPR { get; set; }
        public string RQST_SITU { get; set; }
        public string PROC_STAT { get; set; }
        public string PERM_REQT_TYPE { get; set; }
        public string CITY_LIMT { get; set; }
        public string ACTV_STAT { get; set; }
        public string OLD_REGN_CODE { get; set; }
        public string OLD_WORK_DAY { get; set; }
        public string OLD_BLOCK { get; set; }
        public string NEW_REGN_CODE { get; set; }
        public string NEW_WORK_DAY { get; set; }
        public string NEW_BLOCK { get; set; }
        public string RQST_FINL_STEP { get; set; }
        public string MDFY_BY { get; set; }
        public Nullable<System.DateTime> MDFY_DATE { get; set; }
        public string FINL_STEP { get; set; }
        public Nullable<System.DateTime> SAVE_DATE { get; set; }
        public string OLD_SEQN_FROM { get; set; }
        public string OLD_SEQN_TO { get; set; }
        public string NEW_SEQN_FROM { get; set; }
        public string NEW_SEQN_TO { get; set; }
        public string NEIB_SERV_NO { get; set; }
        public string NEIB_IDNT { get; set; }
        public Nullable<int> BULD_AREA { get; set; }
        public string SRVC_TYPE { get; set; }
        public string INSP_STAT { get; set; }
        public string WEB_STAT { get; set; }
        public string FUTR_STLM_CNST { get; set; }
        public string REF_CODE { get; set; }
        public string LETT_DATE_TEMP { get; set; }
    
        public virtual REGION REGION { get; set; }
        public virtual REGION REGION1 { get; set; }
        public virtual ICollection<REQUEST_ROW> REQUEST_ROW { get; set; }
        public virtual ICollection<REQUEST> REQUEST1 { get; set; }
        public virtual REQUEST REQUEST2 { get; set; }
        public virtual REQUEST_TYPE REQUEST_TYPE { get; set; }
        public virtual REQUESTER_TYPE REQUESTER_TYPE { get; set; }
        public virtual ICollection<STEP_HISTORY_SUMMERY> STEP_HISTORY_SUMMERY { get; set; }
        public virtual CYCLE CYCLE { get; set; }
    }
}
