﻿//------------------------------------------------------------------------------
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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class SasContext : DbContext
    {
        public SasContext()
            : base("name=SasContext")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public DbSet<BASE_METER_SPEC> BASE_METER_SPEC { get; set; }
        public DbSet<METER_SPEC> METER_SPEC { get; set; }
        public DbSet<REGION> REGION { get; set; }
        public DbSet<REQUEST> REQUEST { get; set; }
        public DbSet<REQUEST_ROW> REQUEST_ROW { get; set; }
        public DbSet<REQUEST_TYPE> REQUEST_TYPE { get; set; }
        public DbSet<REQUESTER_TYPE> REQUESTER_TYPE { get; set; }
        public DbSet<SERVICE> SERVICE { get; set; }
        public DbSet<CG_REF_CODES> CG_REF_CODES { get; set; }
        public DbSet<STEP_HISTORY_DETAIL> STEP_HISTORY_DETAIL { get; set; }
        public DbSet<STEP_HISTORY_SUMMERY> STEP_HISTORY_SUMMERY { get; set; }
        public DbSet<BIL_CONSUMPTION> BIL_CONSUMPTION { get; set; }
        public DbSet<BILL> BILL { get; set; }
        public DbSet<BIL_PREVENT_CODE> BIL_PREVENT_CODE { get; set; }
        public DbSet<BIL_BILL_DEBIT> BIL_BILL_DEBIT { get; set; }
        public DbSet<BIL_BILL_AMOUNT> BIL_BILL_AMOUNT { get; set; }
        public DbSet<BIL_CONSUMPTION_DETAIL> BIL_CONSUMPTION_DETAIL { get; set; }
        public DbSet<SAS_PUBLIC> SAS_PUBLIC { get; set; }
        public DbSet<BASE_METER_TARIFF> BASE_METER_TARIFF { get; set; }
        public DbSet<ADM_CATEGORY> ADM_CATEGORY { get; set; }
        public DbSet<BIL_FIRST_INFORMATION> BIL_FIRST_INFORMATION { get; set; }
        public DbSet<BIL_TARIFF> BIL_TARIFF { get; set; }
        public DbSet<SERVICE_TARIFF> SERVICE_TARIFF { get; set; }
        public DbSet<BIL_DEBIT> BIL_DEBIT { get; set; }
        public DbSet<BIL_ACCOUNT> BIL_ACCOUNT { get; set; }
        public DbSet<BIL_ACCOUNT_DETAIL> BIL_ACCOUNT_DETAIL { get; set; }
        public DbSet<BIL_RCPT_ROW_ANNOUNCEMENT> BIL_RCPT_ROW_ANNOUNCEMENT { get; set; }
        public DbSet<SAS_REC_TYPE> SAS_REC_TYPE { get; set; }
        public DbSet<ADM_TRANSFER_SPEC> ADM_TRANSFER_SPEC { get; set; }
        public DbSet<CYCLE> CYCLE { get; set; }
        public DbSet<EXPENSE> EXPENSE { get; set; }
        public DbSet<EXPENSE_TYPE> EXPENSE_TYPE { get; set; }
        public DbSet<REGULATION> REGULATION { get; set; }
        public DbSet<REGULATION_ROW> REGULATION_ROW { get; set; }
        public DbSet<SAS_DOCUMENT_SPEC> SAS_DOCUMENT_SPEC { get; set; }
        public DbSet<SAS_EXPENSE_ITEM> SAS_EXPENSE_ITEM { get; set; }
        public DbSet<SAS_REQUEST_DOCUMENT> SAS_REQUEST_DOCUMENT { get; set; }
        public DbSet<SAS_REQUEST_REQUESTER> SAS_REQUEST_REQUESTER { get; set; }
        public DbSet<ADM_REGION_EXPENSE_ACOUNT> ADM_REGION_EXPENSE_ACOUNT { get; set; }
        public DbSet<BANK> BANK { get; set; }
        public DbSet<BANK_ACCOUNT> BANK_ACCOUNT { get; set; }
        public DbSet<BANK_BRANCH> BANK_BRANCH { get; set; }
    }
}
