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
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraEditors.Controls;
using System.Text.RegularExpressions;
using System.IO;
using System.Xml.Linq;
using DevExpress.XtraEditors;

namespace System.Scsc.Ui.Attendance
{
   public partial class MNGR_DRES_F : UserControl
   {
      public MNGR_DRES_F()
      {
         InitializeComponent();
      }

      private bool requery = false;

      private void Btn_Back_Click(object sender, EventArgs e)
      {
         _DefaultGateway.Gateway(
            new Job(SendType.External, "localhost", GetType().Name, 00 /* Execute ProcessCmdKey */, SendType.SelfToUserInterface) { Input = Keys.Escape }
         );
      }

      private void Execute_Query()
      {
         iScsc = new Data.iScscDataContext(ConnectionString);
         int dres = DresBs1.Position;
         ComaBs1.DataSource = iScsc.Computer_Actions;
         DresBs1.Position = dres;
         requery = false;
      }

      private void SaveChange_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            DresBs1.EndEdit();

            iScsc.SubmitChanges();

            requery = true;
         }
         catch { }
         finally
         {
            if (requery)
               Execute_Query();
         }
      }

      private void Search_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            FromAttnDate_Date.CommitChanges();
            ToAttnDate_Date.CommitChanges();
            var dres = DresBs1.Current as Data.Dresser;
            if (dres == null) return;


            FighBs1.DataSource = 
               iScsc.Attendances
               .Where(a => 
                  a.ATTN_DATE.Date >= FromAttnDate_Date.Value.Value.Date &&
                  a.ATTN_DATE.Date <= ToAttnDate_Date.Value.Value.Date &&
                  a.Dresser_Attendances.Any(da => da.DRES_CODE == dres.CODE)
               ).Select(a => a.Fighter1).Distinct();

         }
         catch (Exception)
         {
            
         }
      }

      private void DresBs1_CurrentChanged(object sender, EventArgs e)
      {
         try
         {
            Search_Butn_Click(null, null);
         }catch{}
      }

      private void FighBs1_CurrentChanged(object sender, EventArgs e)
      {
         try
         {
            var figh = FighBs1.Current as Data.Fighter;
            if (figh == null) return;

            UserProFile_Rb.ImageProfile = null;
            MemoryStream mStream = new MemoryStream();
            byte[] pData = iScsc.GET_PIMG_U(new XElement("Fighter", new XAttribute("fileno", figh.FILE_NO))).ToArray();
            mStream.Write(pData, 0, Convert.ToInt32(pData.Length));
            Bitmap bm = new Bitmap(mStream, false);
            mStream.Dispose();

            //Pb_FighImg.Visible = true;

            if (InvokeRequired)
               Invoke(new Action(() => UserProFile_Rb.ImageProfile = bm));
            else
               UserProFile_Rb.ImageProfile = bm;
         }
         catch
         { //Pb_FighImg.Visible = false;
            UserProFile_Rb.ImageProfile = global::System.Scsc.Properties.Resources.IMAGE_1482;
         }
      }

      private void cMND_SENDTextEdit_ButtonClick(object sender, ButtonPressedEventArgs e)
      {
         try
         {
            var dres = DresBs1.Current as Data.Dresser;
            if (dres == null) return;

            _DefaultGateway.Gateway(
               new Job(SendType.External, "localhost", "MAIN_PAGE_F", 10 /* Execute Actn_Calf_F */, SendType.SelfToUserInterface) 
               { 
                  Input = 
                     new XElement("OprtDres", 
                           new XAttribute("type", "sendoprtdres"),
                           new XAttribute("portname", dres.COMM_PORT),
                           new XAttribute("cmndname", dres.DRES_NUMB)
                         )
               }
            );
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }
   }
}
