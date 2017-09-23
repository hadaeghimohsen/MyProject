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

namespace System.DataGuard.SecPolicy.Share.Ui
{
   public partial class ActiveCyclData : UserControl
   {
      public ActiveCyclData()
      {
         InitializeComponent();
      }

      private bool requery = false;

      private void Execute_Query()
      {
         iProject = new Data.iProjectDataContext(ConnectionString);
         UserBs1.DataSource = iProject.Users.Where(u => u.IsVisible);
         DatSBs1.DataSource = iProject.DataSources.Where(d => d.IsVisible && d.Database_Alias != null);
         AudsBs1.DataSource = iProject.Access_User_Datasources;
      }

      private void Actv_Butn_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
      {
         try
         {
            var crnt = AudsBs1.Current as Data.Access_User_Datasource;
            switch (e.Button.Index)
            {
               case 0:
                  crnt.STAT = crnt.STAT == "001" ? "002" : "001";
                  break;
               case 1:
                  if (crnt.ID > 0 && MessageBox.Show(this, "آیا با پاک کردن اطلاعات دوره کاربر موافق هستید؟", "حذف دسترسی به دوره اطلاعات", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;
                  iProject.Access_User_Datasources.DeleteOnSubmit(crnt);
                  break;
            }            

            AudsBs1.EndEdit();

            iProject.SubmitChanges();
            requery = true;
         }
         catch(Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
         finally
         {
            if(requery)
            {
               Execute_Query();
               requery = false;
            }
         }
      }

      private void Btn_Back_Click(object sender, EventArgs e)
      {
         _DefaultGateway.Gateway(
            new Job(SendType.External, "localhost", GetType().Name, 00 /* Execute ProcessCmdKey */, SendType.SelfToUserInterface) { Input = Keys.Escape }
         );
      }

      private void AddAuds_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var crntuser = UserBs1.Current as Data.User;
            var crntds = DatSBs1.Current as Data.DataSource;

            if (crntuser == null) return;
            if (crntds == null) return;

            //if (AudsBs1.List.OfType<Data.Access_User_Datasource>().Any(a => a.User == crntuser && a.DataSource == crntds)) return;

            AudsBs1.AddNew();
            var crntaud = AudsBs1.Current as Data.Access_User_Datasource;

            crntaud.DataSource = crntds;
            crntaud.User = crntuser;
            crntaud.STAT = "002";

            iProject.Access_User_Datasources.InsertOnSubmit(crntaud);
            
            AudsBs1.EndEdit();

            iProject.SubmitChanges();

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
               requery = false;
            }
         }
      }
   }
}
