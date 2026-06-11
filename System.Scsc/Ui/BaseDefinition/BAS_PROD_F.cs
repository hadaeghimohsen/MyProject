using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using System.JobRouting.Jobs;
using System.Xml.Linq;
using System.IO;
using System.Globalization;

namespace System.Scsc.Ui.BaseDefinition
{
   public partial class BAS_PROD_F : UserControl
   {
      public BAS_PROD_F()
      {
         InitializeComponent();
      }

      private bool requery = false;
      private long? epitcode, pydtcode;

      private void Back_Butn_Click(object sender, EventArgs e)
      {
         _DefaultGateway.Gateway(
            new Job(SendType.External, "localhost", GetType().Name, 00 /* Execute DoWork4Settings */, SendType.SelfToUserInterface) { Input = Keys.Escape }
         );
      }      

      private async void Execute_Query()
      {
         int prod = ProdBs.Position;
         long? _epitcode = epitcode;
         long? _pydtcode = pydtcode;

         var result = await Task.Run(() =>
         {
            using (var db = new Data.iScscDataContext(ConnectionString))
            {
               var epitItems = db.Expense_Items.Where(e => e.CODE == _epitcode).ToList();
               Data.Payment_Detail pydt = null;
               string servName = null;
               if (_pydtcode != null)
               {
                  pydt = db.Payment_Details.Where(pd => pd.CODE == _pydtcode).FirstOrDefault();
                  if (pydt != null)
                     servName = pydt.Request_Row.Fighter.NAME_DNRM;
               }
               return new { epitItems, pydt, servName };
            }
         });

         iScsc = new Data.iScscDataContext(ConnectionString);
         EpitBs.DataSource = result.epitItems;
         ProdBs.Position = prod;

         if (_pydtcode != null)
         {
            PydtBs.DataSource = new List<Data.Payment_Detail> { result.pydt };
            ServName_Lb.Text = result.servName;
         }
         else
            PydtBs.List.Clear();

         requery = true;
      }

      private void SaleProd_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var prod = ProdBs.Current as Data.Product;
            if (prod == null) return;

            if (!(prod.STOK_STAT == "002" /* کالا موجود باشد */&& prod.SALE_STAT == "001" /* کالا فروش نرفته باشد */))
               throw new Exception("کالا یا موجود نیست یا فروخته شده است. لطفا بررسی کنید");

            var pdcs = PdcsBs.Current as Data.Payment_Detail_Commodity_Sale;
            if (pdcs == null)
            {
               PdcsBs.AddNew();
               pdcs = PdcsBs.Current as Data.Payment_Detail_Commodity_Sale;
               pdcs.PYDT_CODE = pydtcode;
               pdcs.PROD_CODE = prod.CODE;

               iScsc.Payment_Detail_Commodity_Sales.InsertOnSubmit(pdcs);
               iScsc.SubmitChanges();
            }
            else
               iScsc.ExecuteCommand(string.Format("UPDATE Payment_Detail_Commodity_Sale SET PROD_CODE = {0} WHERE PYDT_CODE = {1};", prod.CODE, pydtcode));
            
            requery = true;
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
         finally
         {
            if (requery)
               Execute_Query();
         }
      }

      private void SavePdcs_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            SaleDate_Dt.CommitChanges();
            iScsc.SubmitChanges();
            requery = true;
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
         finally
         {
            if (requery)
               Execute_Query();
         }
      }

      private void BackSale_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var pdcs = PdcsBs.Current as Data.Payment_Detail_Commodity_Sale;
            if (pdcs == null) return;

            iScsc.ExecuteCommand("DELETE Payment_Detail_Commodity_Sale WHERE CODE = {0}", pdcs.CODE);
            requery = true;
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
         finally
         {
            if (requery)
               Execute_Query();
         }
      }

      private void AddProd_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            if (ProdBs.List.OfType<Data.Product>().Any(p => p.CODE == 0)) return;

            var prod = ProdBs.AddNew() as Data.Product;
            prod.EPIT_CODE = epitcode;
            prod.MAKE_DATE = DateTime.Now;
            prod.EXPR_DATE = DateTime.Now.AddYears(1);
            prod.STOK_STAT = "002";
            prod.SALE_STAT = "001";

            iScsc.Products.InsertOnSubmit(prod);
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }

      private void DelProd_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var prod = ProdBs.Current as Data.Product;
            if (prod == null) return;

            if(
               iScsc
               .Payment_Detail_Commodity_Sales
               .Any( 
                  pdcs => 
                     pdcs.PROD_CODE == prod.CODE &&
                     pdcs.Payment_Detail.Request_Row.Request.RQST_STAT == "002"
               )
            )
            {
               throw new Exception("از این کالا در فروش های قبلی استفاده شده است، لطفا با مدیریت هماهنگ کنید");
            }

            iScsc.Products.DeleteOnSubmit(prod);
            requery = true;
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
         finally
         {
            if (requery)
               Execute_Query();
         }
      }

      private void SaveProd_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            Prod_Gv.PostEditor();

            iScsc.SubmitChanges();
            requery = true;
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
         finally
         {
            if (requery)
               Execute_Query();
         }
      }

   }
}
