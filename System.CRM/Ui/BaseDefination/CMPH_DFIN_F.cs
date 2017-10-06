using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.JobRouting.Jobs;
using System.Xml.Linq;
using DevExpress.XtraEditors.Controls;
using System.CRM.ExceptionHandlings;
using System.IO;
using DevExpress.XtraEditors;

namespace System.CRM.Ui.BaseDefination
{
   public partial class CMPH_DFIN_F : UserControl
   {
      public CMPH_DFIN_F()
      {
         InitializeComponent();
         Img_001.ImageVisiable = Img_002.ImageVisiable = Img_004.ImageVisiable = Img_005.ImageVisiable =
         Img_006.ImageVisiable = true;
      }

      private bool requery = false;
      private string formcaller = "";
      private string fileno;

      private void Back_Butn_Click(object sender, EventArgs e)
      {
         _DefaultGateway.Gateway(
            new Job(SendType.External, "localhost", GetType().Name, 00 /* Execute ProcessCmdKey */, SendType.SelfToUserInterface) { Input = Keys.Escape }
         );
      }

      List<TabPage> listTabPages;
      private void SwitchButtonsTabPage(object sender)
      {
         try
         {
            #region Action on Buttons
            SimpleButton butn = sender as SimpleButton;
            var flowlayout = butn.Parent as FlowLayoutPanel;
            foreach (SimpleButton b in flowlayout.Controls)
            {
               b.ForeColor = Color.FromArgb(64, 64, 64);
            }
            butn.ForeColor = Color.DodgerBlue;
            #endregion
            #region Action on TabControl
            if (listTabPages == null)
               listTabPages = Tb_Master.TabPages.OfType<TabPage>().ToList();

            var selectedtabpage = listTabPages.Where(t => t.Tag == butn.Tag).First();
            Tb_Master.TabPages.Clear();
            Tb_Master.TabPages.Add(selectedtabpage);
            #endregion
         }
         catch { }
         finally { Execute_Query(); }
      }

      private void RightButns_Click(object sender, EventArgs e)
      {
         SwitchButtonsTabPage(sender);
      }

      private void Execute_Query()
      {
         iCRM = new Data.iCRMDataContext(ConnectionString);
         CmphBs.DataSource = iCRM.Companies.Where(c => c.RECD_STAT == "002" && c.HOST_STAT == "002");
         requery = false;
      }

      private void CmphBs_ListChanged(object sender, ListChangedEventArgs e)
      {
         try
         {
            if (CmphBs.List.Count > 0)
               CompListWelCome_Pnl.Visible = true;
            else
               CompListWelCome_Pnl.Visible = false;
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }

      private void NewCmph_Butn_Click(object sender, EventArgs e)
      {
         if (CmphBs.List.Count == 0 || !CmphBs.List.OfType<Data.Company>().Any(c => c.CODE == 0))
            CmphBs.AddNew();
         else if(CmphBs.List.OfType<Data.Company>().Any(c => c.CODE == 0))
            CmphBs.Position = CmphBs.IndexOf(CmphBs.List.OfType<Data.Company>().FirstOrDefault(c => c.CODE == 0));
         var cmph = CmphBs.Current as Data.Company;
         
         cmph.HOST_STAT = "002";
         cmph.RECD_STAT = "002";

         RightButns_Click(CompInfo_Butn, null);
      }

      private void SubmitChange_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            CmphBs.EndEdit();

            iCRM.SubmitChanges();
            requery = true;
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
         finally
         {
            if(requery)
            {
               Execute_Query();
            }
         }
      }    
   }
}
