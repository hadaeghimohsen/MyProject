using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OracleClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.Emis.Sas.Model
{
   public class Domain
   {
      private string ConnectionString;
      private OracleDataAdapter OraDA;
      private DataSet OraPool;
      
      public Domain(string connectionString)
      {
         ConnectionString = connectionString;
         OraDA =
               new OracleDataAdapter(
                  new OracleCommand("SELECT SYSDATE FROM DUAL",
                     new OracleConnection(ConnectionString)
                     ) { CommandType = CommandType.Text }
               );
         OraPool = new DataSet();
      }

      private DataTable Run_Qury_U(string qury)
      {
         try
         {
            OraPool.Tables.Clear();
            OraDA.SelectCommand.CommandText = qury;
            OraDA.Fill(OraPool);
            return OraPool.Tables[0];
         }
         catch
         {
            return null;
         }
      }

      /*Soft Code Domain*/
      public DataTable ACTN()
      {
         return Run_Qury_U("SELECT * FROM Cg_Ref_Codes WHERE Rv_Domain = 'D ACTN' ORDER BY Rv_Low_Value");
      }

      public DataTable BLCTN()
      {
         return Run_Qury_U("SELECT * FROM Cg_Ref_Codes WHERE Rv_Domain = 'D BLCTN' ORDER BY Rv_Low_Value");
      }

      public DataTable CLIMT()
      {
         return Run_Qury_U("SELECT * FROM Cg_Ref_Codes WHERE Rv_Domain = 'D CLIMT' ORDER BY Rv_Low_Value");
      }

      public DataTable INPLC()
      {
         return Run_Qury_U("SELECT * FROM Cg_Ref_Codes WHERE Rv_Domain = 'D INPLC' ORDER BY Rv_Low_Value");
      }

      public DataTable IRPT()
      {
         return Run_Qury_U("SELECT * FROM Cg_Ref_Codes WHERE Rv_Domain = 'D_IRPT' ORDER BY Rv_Low_Value");
      }

      public DataTable ITCH()
      {
         return Run_Qury_U("SELECT * FROM Cg_Ref_Codes WHERE Rv_Domain = 'D_ITCH' ORDER BY Rv_Low_Value");
      }

      public DataTable SEAS()
      {
         return Run_Qury_U("SELECT * FROM Cg_Ref_Codes WHERE Rv_Domain = 'D SEAS' ORDER BY Rv_Low_Value");
      }

      public DataTable STYP()
      {
         return Run_Qury_U("SELECT * FROM Cg_Ref_Codes WHERE Rv_Domain = 'D STYP' ORDER BY Rv_Low_Value");
      }

      public DataTable SUBJ()
      {
         return Run_Qury_U("SELECT * FROM Cg_Ref_Codes WHERE Rv_Domain = 'D SUBJ' ORDER BY Rv_Low_Value");
      }

      public DataTable SVTP()
      {
         return Run_Qury_U("SELECT * FROM Cg_Ref_Codes WHERE Rv_Domain = 'D SVTP' ORDER BY Rv_Low_Value");
      }

      public DataTable IDXL()
      {
         return Run_Qury_U("SELECT * FROM Cg_Ref_Codes WHERE Rv_Domain = 'D_IDXL' ORDER BY Rv_Low_Value");
      }

      public DataTable BILTP()
      {
         return Run_Qury_U("SELECT * FROM Cg_Ref_Codes WHERE Rv_Domain = 'D BILTP' ORDER BY Rv_Low_Value");
      }

      public DataTable CALC()
      {
         return Run_Qury_U("SELECT * FROM Cg_Ref_Codes WHERE Rv_Domain = 'D CALC' ORDER BY Rv_Low_Value");
      }

      public DataTable CNGTP()
      {
         return Run_Qury_U("SELECT * FROM Cg_Ref_Codes WHERE Rv_Domain = 'D CNGTP' ORDER BY Rv_Low_Value");
      }

      public DataTable DCKI()
      {
         return Run_Qury_U("SELECT * FROM Cg_Ref_Codes WHERE Rv_Domain = 'D DCKI' ORDER BY Rv_Low_Value");
      }

      public DataTable FOLD()
      {
         return Run_Qury_U("SELECT * FROM Cg_Ref_Codes WHERE Rv_Domain = 'D FOLD' ORDER BY Rv_Low_Value");
      }

      public DataTable HILO()
      {
         return Run_Qury_U("SELECT * FROM Cg_Ref_Codes WHERE Rv_Domain = 'D HILO' ORDER BY Rv_Low_Value");
      }

      public DataTable INPT()
      {
         return Run_Qury_U("SELECT * FROM Cg_Ref_Codes WHERE Rv_Domain = 'D INPT' ORDER BY Rv_Low_Value");
      }

      public DataTable INSMT()
      {
         return Run_Qury_U("SELECT * FROM Cg_Ref_Codes WHERE Rv_Domain = 'D INSMT' ORDER BY Rv_Low_Value");
      }

      public DataTable INSTYP()
      {
         return Run_Qury_U("SELECT * FROM Cg_Ref_Codes WHERE Rv_Domain = 'D INSTYP' ORDER BY Rv_Low_Value");
      }

      public DataTable MNTY()
      {
         return Run_Qury_U("SELECT * FROM Cg_Ref_Codes WHERE Rv_Domain = 'D MNTY' ORDER BY Rv_Low_Value");
      }

      public DataTable POWR()
      {
         return Run_Qury_U("SELECT * FROM Cg_Ref_Codes WHERE Rv_Domain = 'D POWR' ORDER BY Rv_Low_Value");
      }

      public DataTable RLSF()
      {
         return Run_Qury_U("SELECT * FROM Cg_Ref_Codes WHERE Rv_Domain = 'D RLSF' ORDER BY Rv_Low_Value");
      }

      public DataTable SUBT()
      {
         return Run_Qury_U("SELECT * FROM Cg_Ref_Codes WHERE Rv_Domain = 'D SUBT' ORDER BY Rv_Low_Value");
      }

      public DataTable TRANS()
      {
         return Run_Qury_U("SELECT * FROM Cg_Ref_Codes WHERE Rv_Domain = 'D TRANS' ORDER BY Rv_Low_Value");
      }

      public DataTable IDBK()
      {
         return Run_Qury_U("SELECT * FROM Cg_Ref_Codes WHERE Rv_Domain = 'D_IDBK' ORDER BY Rv_Low_Value");
      }

      public DataTable IDRP()
      {
         return Run_Qury_U("SELECT * FROM Cg_Ref_Codes WHERE Rv_Domain = 'D_IDRP' ORDER BY Rv_Low_Value");
      }

      public DataTable ACCS()
      {
         return Run_Qury_U("SELECT * FROM Cg_Ref_Codes WHERE Rv_Domain = 'D ACCS' ORDER BY Rv_Low_Value");
      }

      public DataTable BRST()
      {
         return Run_Qury_U("SELECT * FROM Cg_Ref_Codes WHERE Rv_Domain = 'D BRST' ORDER BY Rv_Low_Value");
      }

      public DataTable CNMT()
      {
         return Run_Qury_U("SELECT * FROM Cg_Ref_Codes WHERE Rv_Domain = 'D CNMT' ORDER BY Rv_Low_Value");
      }

      public DataTable CSTAT()
      {
         return Run_Qury_U("SELECT * FROM Cg_Ref_Codes WHERE Rv_Domain = 'D CSTAT' ORDER BY Rv_Low_Value");
      }

      public DataTable CYCST()
      {
         return Run_Qury_U("SELECT * FROM Cg_Ref_Codes WHERE Rv_Domain = 'D CYCST' ORDER BY Rv_Low_Value");
      }

      public DataTable DAMG()
      {
         return Run_Qury_U("SELECT * FROM Cg_Ref_Codes WHERE Rv_Domain = 'D DAMG' ORDER BY Rv_Low_Value");
      }

      public DataTable DOCT()
      {
         return Run_Qury_U("SELECT * FROM Cg_Ref_Codes WHERE Rv_Domain = 'D DOCT' ORDER BY Rv_Low_Value");
      }

      public DataTable HACT2()
      {
         return Run_Qury_U("SELECT * FROM Cg_Ref_Codes WHERE Rv_Domain = 'D HACT2' ORDER BY Rv_Low_Value");
      }

      public DataTable OFTAG()
      {
         return Run_Qury_U("SELECT * FROM Cg_Ref_Codes WHERE Rv_Domain = 'D OFTAG' ORDER BY Rv_Low_Value");
      }

      public DataTable ISTR()
      {
         return Run_Qury_U("SELECT * FROM Cg_Ref_Codes WHERE Rv_Domain = 'D_ISTR' ORDER BY Rv_Low_Value");
      }

      public DataTable RCPTP()
      {
         return Run_Qury_U("SELECT * FROM Cg_Ref_Codes WHERE Rv_Domain = 'D RCPTP' ORDER BY Rv_Low_Value");
      }

      public DataTable RCPTS()
      {
         return Run_Qury_U("SELECT * FROM Cg_Ref_Codes WHERE Rv_Domain = 'D RCPTS' ORDER BY Rv_Low_Value");
      }

      public DataTable RQRE()
      {
         return Run_Qury_U("SELECT * FROM Cg_Ref_Codes WHERE Rv_Domain = 'D RQRE' ORDER BY Rv_Low_Value");
      }

      public DataTable SELTP()
      {
         return Run_Qury_U("SELECT * FROM Cg_Ref_Codes WHERE Rv_Domain = 'D SELTP' ORDER BY Rv_Low_Value");
      }

      public DataTable WATR()
      {
         return Run_Qury_U("SELECT * FROM Cg_Ref_Codes WHERE Rv_Domain = 'D WATR' ORDER BY Rv_Low_Value");
      }

      public DataTable WRTY()
      {
         return Run_Qury_U("SELECT * FROM Cg_Ref_Codes WHERE Rv_Domain = 'D WRTY' ORDER BY Rv_Low_Value");
      }

      public DataTable ACCT()
      {
         return Run_Qury_U("SELECT * FROM Cg_Ref_Codes WHERE Rv_Domain = 'D ACCT' ORDER BY Rv_Low_Value");
      }

      public DataTable CNMP()
      {
         return Run_Qury_U("SELECT * FROM Cg_Ref_Codes WHERE Rv_Domain = 'D CNMP' ORDER BY Rv_Low_Value");
      }

      public DataTable DCTY()
      {
         return Run_Qury_U("SELECT * FROM Cg_Ref_Codes WHERE Rv_Domain = 'D DCTY' ORDER BY Rv_Low_Value");
      }

      public DataTable DISC()
      {
         return Run_Qury_U("SELECT * FROM Cg_Ref_Codes WHERE Rv_Domain = 'D DISC' ORDER BY Rv_Low_Value");
      }

      public DataTable DISD()
      {
         return Run_Qury_U("SELECT * FROM Cg_Ref_Codes WHERE Rv_Domain = 'D DISD' ORDER BY Rv_Low_Value");
      }

      public DataTable IPOWR()
      {
         return Run_Qury_U("SELECT * FROM Cg_Ref_Codes WHERE Rv_Domain = 'D IPOWR' ORDER BY Rv_Low_Value");
      }

      public DataTable LSTT()
      {
         return Run_Qury_U("SELECT * FROM Cg_Ref_Codes WHERE Rv_Domain = 'D LSTT' ORDER BY Rv_Low_Value");
      }

      public DataTable MDTY()
      {
         return Run_Qury_U("SELECT * FROM Cg_Ref_Codes WHERE Rv_Domain = 'D MDTY' ORDER BY Rv_Low_Value");
      }

      public DataTable METR()
      {
         return Run_Qury_U("SELECT * FROM Cg_Ref_Codes WHERE Rv_Domain = 'D METR' ORDER BY Rv_Low_Value");
      }

      public DataTable RGLTP()
      {
         return Run_Qury_U("SELECT * FROM Cg_Ref_Codes WHERE Rv_Domain = 'D RGLTP' ORDER BY Rv_Low_Value");
      }

      public DataTable RMETD()
      {
         return Run_Qury_U("SELECT * FROM Cg_Ref_Codes WHERE Rv_Domain = 'D RMETD' ORDER BY Rv_Low_Value");
      }

      public DataTable IGHD()
      {
         return Run_Qury_U("SELECT * FROM Cg_Ref_Codes WHERE Rv_Domain = 'D_IGHD' ORDER BY Rv_Low_Value");
      }

      public DataTable ACTC()
      {
         return Run_Qury_U("SELECT * FROM Cg_Ref_Codes WHERE Rv_Domain = 'D ACTC' ORDER BY Rv_Low_Value");
      }

      public DataTable AMACT()
      {
         return Run_Qury_U("SELECT * FROM Cg_Ref_Codes WHERE Rv_Domain = 'D AMACT' ORDER BY Rv_Low_Value");
      }

      public DataTable CHNG()
      {
         return Run_Qury_U("SELECT * FROM Cg_Ref_Codes WHERE Rv_Domain = 'D CHNG' ORDER BY Rv_Low_Value");
      }

      public DataTable CHNGR()
      {
         return Run_Qury_U("SELECT * FROM Cg_Ref_Codes WHERE Rv_Domain = 'D CHNGR' ORDER BY Rv_Low_Value");
      }

      public DataTable CLMT()
      {
         return Run_Qury_U("SELECT * FROM Cg_Ref_Codes WHERE Rv_Domain = 'D CLMT' ORDER BY Rv_Low_Value");
      }

      public DataTable EXCD()
      {
         return Run_Qury_U("SELECT * FROM Cg_Ref_Codes WHERE Rv_Domain = 'D EXCD' ORDER BY Rv_Low_Value");
      }

      public DataTable INST()
      {
         return Run_Qury_U("SELECT * FROM Cg_Ref_Codes WHERE Rv_Domain = 'D INST' ORDER BY Rv_Low_Value");
      }

      public DataTable MSGTP()
      {
         return Run_Qury_U("SELECT * FROM Cg_Ref_Codes WHERE Rv_Domain = 'D MSGTP' ORDER BY Rv_Low_Value");
      }

      public DataTable MSTK()
      {
         return Run_Qury_U("SELECT * FROM Cg_Ref_Codes WHERE Rv_Domain = 'D MSTK' ORDER BY Rv_Low_Value");
      }

      public DataTable MTARF()
      {
         return Run_Qury_U("SELECT * FROM Cg_Ref_Codes WHERE Rv_Domain = 'D MTARF' ORDER BY Rv_Low_Value");
      }

      public DataTable IPLM()
      {
         return Run_Qury_U("SELECT * FROM Cg_Ref_Codes WHERE Rv_Domain = 'D_IPLM' ORDER BY Rv_Low_Value");
      }

      public DataTable PTYP()
      {
         return Run_Qury_U("SELECT * FROM Cg_Ref_Codes WHERE Rv_Domain = 'D_PTYP' ORDER BY Rv_Low_Value");
      }

      public DataTable SLOP()
      {
         return Run_Qury_U("SELECT * FROM Cg_Ref_Codes WHERE Rv_Domain = 'D SLOP' ORDER BY Rv_Low_Value");
      }

      public DataTable ICON()
      {
         return Run_Qury_U("SELECT * FROM Cg_Ref_Codes WHERE Rv_Domain = 'D_ICON' ORDER BY Rv_Low_Value");
      }

      public DataTable IDSR()
      {
         return Run_Qury_U("SELECT * FROM Cg_Ref_Codes WHERE Rv_Domain = 'D_IDSR' ORDER BY Rv_Low_Value");
      }

      public DataTable BAMNT()
      {
         return Run_Qury_U("SELECT * FROM Cg_Ref_Codes WHERE Rv_Domain = 'D BAMNT' ORDER BY Rv_Low_Value");
      }

      public DataTable CNNC()
      {
         return Run_Qury_U("SELECT * FROM Cg_Ref_Codes WHERE Rv_Domain = 'D CNNC' ORDER BY Rv_Low_Value");
      }

      public DataTable DEBTP()
      {
         return Run_Qury_U("SELECT * FROM Cg_Ref_Codes WHERE Rv_Domain = 'D DEBTP' ORDER BY Rv_Low_Value");
      }

      public DataTable IKEY()
      {
         return Run_Qury_U("SELECT * FROM Cg_Ref_Codes WHERE Rv_Domain = 'D IKEY' ORDER BY Rv_Low_Value");
      }

      public DataTable MTYP()
      {
         return Run_Qury_U("SELECT * FROM Cg_Ref_Codes WHERE Rv_Domain = 'D MTYP' ORDER BY Rv_Low_Value");
      }

      public DataTable OBTY()
      {
         return Run_Qury_U("SELECT * FROM Cg_Ref_Codes WHERE Rv_Domain = 'D OBTY' ORDER BY Rv_Low_Value");
      }

      public DataTable ISTA()
      {
         return Run_Qury_U("SELECT * FROM Cg_Ref_Codes WHERE Rv_Domain = 'D_ISTA' ORDER BY Rv_Low_Value");
      }

      public DataTable ITYP()
      {
         return Run_Qury_U("SELECT * FROM Cg_Ref_Codes WHERE Rv_Domain = 'D_ITYP' ORDER BY Rv_Low_Value");
      }

      public DataTable RSLI()
      {
         return Run_Qury_U("SELECT * FROM Cg_Ref_Codes WHERE Rv_Domain = 'D_RSLI' ORDER BY Rv_Low_Value");
      }

      public DataTable TARF()
      {
         return Run_Qury_U("SELECT * FROM Cg_Ref_Codes WHERE Rv_Domain = 'D TARF' ORDER BY Rv_Low_Value");
      }

      public DataTable VSTTP()
      {
         return Run_Qury_U("SELECT * FROM Cg_Ref_Codes WHERE Rv_Domain = 'D VSTTP' ORDER BY Rv_Low_Value");
      }

      public DataTable IDTI()
      {
         return Run_Qury_U("SELECT * FROM Cg_Ref_Codes WHERE Rv_Domain = 'D_IDTI' ORDER BY Rv_Low_Value");
      }

      public DataTable IDUN()
      {
         return Run_Qury_U("SELECT * FROM Cg_Ref_Codes WHERE Rv_Domain = 'D_IDUN' ORDER BY Rv_Low_Value");
      }

      public DataTable ANNC()
      {
         return Run_Qury_U("SELECT * FROM Cg_Ref_Codes WHERE Rv_Domain = 'D ANNC' ORDER BY Rv_Low_Value");
      }

      public DataTable BILST()
      {
         return Run_Qury_U("SELECT * FROM Cg_Ref_Codes WHERE Rv_Domain = 'D BILST' ORDER BY Rv_Low_Value");
      }

      public DataTable CONS()
      {
         return Run_Qury_U("SELECT * FROM Cg_Ref_Codes WHERE Rv_Domain = 'D CONS' ORDER BY Rv_Low_Value");
      }

      public DataTable DLPNT()
      {
         return Run_Qury_U("SELECT * FROM Cg_Ref_Codes WHERE Rv_Domain = 'D DLPNT' ORDER BY Rv_Low_Value");
      }

      public DataTable LOSS()
      {
         return Run_Qury_U("SELECT * FROM Cg_Ref_Codes WHERE Rv_Domain = 'D LOSS' ORDER BY Rv_Low_Value");
      }

      public DataTable PARAM()
      {
         return Run_Qury_U("SELECT * FROM Cg_Ref_Codes WHERE Rv_Domain = 'D PARAM' ORDER BY Rv_Low_Value");
      }

      public DataTable POSS()
      {
         return Run_Qury_U("SELECT * FROM Cg_Ref_Codes WHERE Rv_Domain = 'D POSS' ORDER BY Rv_Low_Value");
      }

      public DataTable PREP()
      {
         return Run_Qury_U("SELECT * FROM Cg_Ref_Codes WHERE Rv_Domain = 'D PREP' ORDER BY Rv_Low_Value");
      }

      public DataTable PRTY()
      {
         return Run_Qury_U("SELECT * FROM Cg_Ref_Codes WHERE Rv_Domain = 'D PRTY' ORDER BY Rv_Low_Value");
      }

      public DataTable RESN()
      {
         return Run_Qury_U("SELECT * FROM Cg_Ref_Codes WHERE Rv_Domain = 'D RESN' ORDER BY Rv_Low_Value");
      }

      public DataTable RMTD()
      {
         return Run_Qury_U("SELECT * FROM Cg_Ref_Codes WHERE Rv_Domain = 'D RMTD' ORDER BY Rv_Low_Value");
      }

      public DataTable ICNT()
      {
         return Run_Qury_U("SELECT * FROM Cg_Ref_Codes WHERE Rv_Domain = 'D_ICNT' ORDER BY Rv_Low_Value");
      }

      public DataTable IDPL()
      {
         return Run_Qury_U("SELECT * FROM Cg_Ref_Codes WHERE Rv_Domain = 'D_IDPL' ORDER BY Rv_Low_Value");
      }

      public DataTable IDTY()
      {
         return Run_Qury_U("SELECT * FROM Cg_Ref_Codes WHERE Rv_Domain = 'D_IDTY' ORDER BY Rv_Low_Value");
      }

      public DataTable CFLT()
      {
         return Run_Qury_U("SELECT * FROM Cg_Ref_Codes WHERE Rv_Domain = 'D CFLT' ORDER BY Rv_Low_Value");
      }

      public DataTable DCMT()
      {
         return Run_Qury_U("SELECT * FROM Cg_Ref_Codes WHERE Rv_Domain = 'D DCMT' ORDER BY Rv_Low_Value");
      }

      public DataTable DCTP()
      {
         return Run_Qury_U("SELECT * FROM Cg_Ref_Codes WHERE Rv_Domain = 'D DCTP' ORDER BY Rv_Low_Value");
      }

      public DataTable DEPT()
      {
         return Run_Qury_U("SELECT * FROM Cg_Ref_Codes WHERE Rv_Domain = 'D DEPT' ORDER BY Rv_Low_Value");
      }

      public DataTable EXPTP()
      {
         return Run_Qury_U("SELECT * FROM Cg_Ref_Codes WHERE Rv_Domain = 'D EXPTP' ORDER BY Rv_Low_Value");
      }

      public DataTable FILET()
      {
         return Run_Qury_U("SELECT * FROM Cg_Ref_Codes WHERE Rv_Domain = 'D FILET' ORDER BY Rv_Low_Value");
      }

      public DataTable HSPT()
      {
         return Run_Qury_U("SELECT * FROM Cg_Ref_Codes WHERE Rv_Domain = 'D HSPT' ORDER BY Rv_Low_Value");
      }

      public DataTable METD()
      {
         return Run_Qury_U("SELECT * FROM Cg_Ref_Codes WHERE Rv_Domain = 'D METD' ORDER BY Rv_Low_Value");
      }

      public DataTable PACTP()
      {
         return Run_Qury_U("SELECT * FROM Cg_Ref_Codes WHERE Rv_Domain = 'D PACTP' ORDER BY Rv_Low_Value");
      }

      public DataTable PMTY()
      {
         return Run_Qury_U("SELECT * FROM Cg_Ref_Codes WHERE Rv_Domain = 'D PMTY' ORDER BY Rv_Low_Value");
      }

      public DataTable REDST()
      {
         return Run_Qury_U("SELECT * FROM Cg_Ref_Codes WHERE Rv_Domain = 'D REDST' ORDER BY Rv_Low_Value");
      }

      public DataTable RSNT()
      {
         return Run_Qury_U("SELECT * FROM Cg_Ref_Codes WHERE Rv_Domain = 'D RSNT' ORDER BY Rv_Low_Value");
      }

      public DataTable IKST()
      {
         return Run_Qury_U("SELECT * FROM Cg_Ref_Codes WHERE Rv_Domain = 'D_IKST' ORDER BY Rv_Low_Value");
      }

      /*Hard Code Domain*/      
      public DataTable ZONE()
      {
         return Run_Qury_U("SELECT 1 AS RV_LOW_VALUE, 'شهری' AS RV_MEANING FROM DUAL UNION SELECT 2 AS RV_LOW_VALUE, 'روستایی' AS RV_MEANING FROM DUAL");
      }

      public DataTable YESNO()
      {
         return Run_Qury_U("SELECT 1 AS RV_LOW_VALUE, 'خیر' AS RV_MEANING FROM DUAL UNION SELECT 2 AS RV_LOW_VALUE, 'بلی' AS RV_MEANING FROM DUAL");
      }

      public DataTable CONST()
      {
         return Run_Qury_U("SELECT 1 AS RV_LOW_VALUE, 'هوایی' AS RV_MEANING FROM DUAL UNION SELECT 2 AS RV_LOW_VALUE, 'زمینی' AS RV_MEANING FROM DUAL");
      }

      public DataTable OWNR()
      {
         return Run_Qury_U("SELECT 1 AS RV_LOW_VALUE, 'خصوصی' AS RV_MEANING FROM DUAL UNION SELECT 2 AS RV_LOW_VALUE, 'دولتی' AS RV_MEANING FROM DUAL");
      }

      public DataTable SERV()
      {
         return Run_Qury_U("SELECT 1 AS RV_LOW_VALUE, 'عادی' AS RV_MEANING FROM DUAL UNION SELECT 2 AS RV_LOW_VALUE, 'دیماندی' AS RV_MEANING FROM DUAL");
      }

      public DataTable FRID()
      {
         return Run_Qury_U("SELECT 1 AS RV_LOW_VALUE, 'معمولی' AS RV_MEANING FROM DUAL UNION SELECT 2 AS RV_LOW_VALUE, 'جمعه' AS RV_MEANING FROM DUAL");
      }

      public DataTable PHAS()
      {
         return Run_Qury_U("SELECT 1 AS RV_LOW_VALUE, 'تکفاز' AS RV_MEANING FROM DUAL UNION SELECT 3 AS RV_LOW_VALUE, 'سه فاز' AS RV_MEANING FROM DUAL");
      }

      public DataTable VRANG()
      {
         return Run_Qury_U("SELECT '01' AS RV_LOW_VALUE, '230000' AS RV_MEANING FROM DUAL UNION SELECT '02' AS RV_LOW_VALUE, '400000' AS RV_MEANING FROM DUAL UNION SELECT '03' AS RV_LOW_VALUE, '132000' AS RV_MEANING FROM DUAL UNION SELECT '04' AS RV_LOW_VALUE, '66000' AS RV_MEANING FROM DUAL UNION SELECT '05' AS RV_LOW_VALUE, '63000' AS RV_MEANING FROM DUAL UNION SELECT '06' AS RV_LOW_VALUE, '33000' AS RV_MEANING FROM DUAL UNION SELECT '07' AS RV_LOW_VALUE, '20000' AS RV_MEANING FROM DUAL UNION SELECT '08' AS RV_LOW_VALUE, '11000' AS RV_MEANING FROM DUAL UNION SELECT '09' AS RV_LOW_VALUE, '380' AS RV_MEANING FROM DUAL UNION SELECT '10' AS RV_LOW_VALUE, '230' AS RV_MEANING FROM DUAL");
      }

      public DataTable VOLT()
      {
         return Run_Qury_U("SELECT 1 AS RV_LOW_VALUE, 'اولیه' AS RV_MEANING FROM DUAL UNION SELECT 2 AS RV_LOW_VALUE, 'ثانویه' AS RV_MEANING FROM DUAL");
      }

      public DataTable EXIST()
      {
         return Run_Qury_U("SELECT 1 AS RV_LOW_VALUE, 'دارد' AS RV_MEANING FROM DUAL UNION SELECT 2 AS RV_LOW_VALUE, 'ندارد' AS RV_MEANING FROM DUAL");
      }

      public DataTable BRCTP()
      {
         return Run_Qury_U("SELECT 1 AS RV_LOW_VALUE, 'دائم' AS RV_MEANING FROM DUAL UNION SELECT 2 AS RV_LOW_VALUE, 'آزاد' AS RV_MEANING FROM DUAL UNION SELECT 3 AS RV_LOW_VALUE, 'حاشیه شهرها' AS RV_MEANING FROM DUAL UNION SELECT 4 AS RV_LOW_VALUE, 'آزاد به دائم' AS RV_MEANING FROM DUAL");
      }

      public DataTable MTYPE()
      {
         return Run_Qury_U("SELECT 1 AS RV_LOW_VALUE, 'دیجیتالی' AS RV_MEANING FROM DUAL UNION SELECT 2 AS RV_LOW_VALUE, 'مکانیکی' AS RV_MEANING FROM DUAL UNION SELECT 3 AS RV_LOW_VALUE, 'قرائت راه دور' AS RV_MEANING FROM DUAL UNION SELECT 4 AS RV_LOW_VALUE, 'هوشمند کشاورزی' AS RV_MEANING FROM DUAL");
      }

      public DataTable DVOLT()
      {
         return Run_Qury_U("SELECT 1 AS RV_LOW_VALUE, '230 , 400 KVOLT' AS RV_MEANING FROM DUAL UNION SELECT 2 AS RV_LOW_VALUE, '63, 66, 132 KVOLT' AS RV_MEANING FROM DUAL UNION SELECT 3 AS RV_LOW_VALUE, '11, 20, 33 KVOLT' AS RV_MEANING FROM DUAL UNION SELECT 4 AS RV_LOW_VALUE, '230, 380 VOLT' AS RV_MEANING FROM DUAL UNION SELECT 5 AS RV_LOW_VALUE, 'کلی' AS RV_MEANING FROM DUAL");
      }

      public DataTable PRANG()
      {
         return Run_Qury_U("SELECT 1 AS RV_LOW_VALUE, 'کمتراز 30 کیلو وات' AS RV_MEANING FROM DUAL UNION SELECT 2 AS RV_LOW_VALUE, 'بیشتر از 30 کیلو وات' AS RV_MEANING FROM DUAL UNION SELECT 3 AS RV_LOW_VALUE, '30 کیلو وات' AS RV_MEANING FROM DUAL");
      }

      public DataTable ITEM()
      {
         return Run_Qury_U("SELECT 0 AS RV_LOW_VALUE, 'گزینه ندارد' AS RV_MEANING FROM DUAL UNION SELECT 1 AS RV_LOW_VALUE, 'گزینه 1' AS RV_MEANING FROM DUAL UNION SELECT 2 AS RV_LOW_VALUE, 'گزینه 2' AS RV_MEANING FROM DUAL UNION SELECT 3 AS RV_LOW_VALUE, 'گزینه 3' AS RV_MEANING FROM DUAL");
      }

      public DataTable ROWN()
      {
         return Run_Qury_U("SELECT 1 AS RV_LOW_VALUE, 'رکورد متعلق به سیستم قبلی است' AS RV_MEANING FROM DUAL UNION SELECT 2 AS RV_LOW_VALUE, 'رکورد متعلق به سیستم جدید است' AS RV_MEANING FROM DUAL UNION SELECT 3 AS RV_LOW_VALUE, 'ایجاد در سیستم قدیم و ادامه در سیستم جدید' AS RV_MEANING FROM DUAL");
      }

      public DataTable REQTP()
      {
         return Run_Qury_U("SELECT 1 AS RV_LOW_VALUE, 'کاربری' AS RV_MEANING FROM DUAL UNION SELECT 2 AS RV_LOW_VALUE, 'سیستمی' AS RV_MEANING FROM DUAL");
      }

      public string RGRO(string phas, string netw_type, int fromampr, int toampr, int frompowr, long topowr)
      {
         return (phas == "1" ? "تک فاز" : phas == "3" && fromampr == 10 && toampr == 48 ? "سه فاز" : phas == "3" && frompowr == 30 && topowr == 99999 ? "اولیه" : "ثانویه");
      }

      public DataTable NWTYP()
      {
         return Run_Qury_U("SELECT 1 AS RV_LOW_VALUE, 'فشار ضعیف' AS RV_MEANING FROM DUAL UNION SELECT 2 AS RV_LOW_VALUE, 'فشار متوسط' AS RV_MEANING FROM DUAL UNION SELECT 3 AS RV_LOW_VALUE, 'فشار قوی' AS RV_MEANING FROM DUAL");
      }

      public DataTable ACTST()
      {
         return Run_Qury_U("SELECT 1 AS RV_LOW_VALUE, 'فعال' AS RV_MEANING FROM DUAL UNION SELECT 2 AS RV_LOW_VALUE, 'نیمه فعال' AS RV_MEANING FROM DUAL UNION SELECT 3 AS RV_LOW_VALUE, 'غیرفعال' AS RV_MEANING FROM DUAL");
      }
   }
}
