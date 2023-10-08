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

         int usr = UserBs1.Position;
         int dats = DatSBs1.Position;
         int auds = AudsBs1.Position;

         UserBs1.DataSource = iProject.Users.Where(u => u.IsVisible);
         DatSBs1.DataSource = iProject.DataSources.Where(d => d.IsVisible && d.Database_Alias != null && d.Sub_System.INST_STAT == "002" && d.Sub_System.STAT == "002");
         AudsBs1.DataSource = iProject.Access_User_Datasources;

         UserBs1.Position = usr;
         DatSBs1.Position = dats;
         AudsBs1.Position = auds;
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

                  foreach (var _audsid in Auds_Gv.GetSelectedRows())
                  {
                     var _auds = Auds_Gv.GetRow(_audsid) as Data.Access_User_Datasource;
                     iProject.ExecuteCommand(string.Format("DELETE Global.Access_User_Datasource WHERE Id = {0}", _auds.ID));
                  }
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
            foreach (var _userid in User_Gv.GetSelectedRows())
            {
               var _user = User_Gv.GetRow(_userid) as Data.User;

               foreach (var _databaseid in Database_Gv.GetSelectedRows())
               {
                  var _database = Database_Gv.GetRow(_databaseid) as Data.DataSource;

                  foreach (var _computerid in Computer_Gv.GetSelectedRows())
                  {
                     var _computer = Computer_Gv.GetRow(_computerid) as Data.V_Computer;

                     if (!AudsBs1.List.OfType<Data.Access_User_Datasource>().Any(a => a.USER_ID == _user.ID && a.DSRC_ID == _database.ID && a.HOST_NAME == _computer.CPU_SRNO_DNRM))
                     {
                        iProject.ExecuteCommand(
                           string.Format("INSERT INTO Global.Access_User_Datasource (USER_ID, DSRC_ID, HOST_NAME, STAT, ACES_TYPE) VALUES({0}, {1}, '{2}', '002', '001')", _user.ID, _database.ID, _computer.CPU_SRNO_DNRM)
                        );
                     }
                  }
               }
            }
            //var crntuser = UserBs1.Current as Data.User;
            //var crntds = DatSBs1.Current as Data.DataSource;

            //if (crntuser == null) return;
            //if (crntds == null) return;

            ////**** //if (AudsBs1.List.OfType<Data.Access_User_Datasource>().Any(a => a.User == crntuser && a.DataSource == crntds)) return;

            //AudsBs1.AddNew();
            //var crntaud = AudsBs1.Current as Data.Access_User_Datasource;

            //crntaud.DataSource = crntds;
            //crntaud.User = crntuser;
            //crntaud.STAT = "002";
            //crntaud.ACES_TYPE = "001";

            //iProject.Access_User_Datasources.InsertOnSubmit(crntaud);
            
            //AudsBs1.EndEdit();

            //iProject.SubmitChanges();

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

      private void DelMultiRecod_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            if (MessageBox.Show(this, "آیا با پاک کردن اطلاعات دوره کاربر موافق هستید؟", "حذف دسترسی به دوره اطلاعات", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;
            foreach (var _audsid in Auds_Gv.GetSelectedRows())
            {
               var _auds = Auds_Gv.GetRow(_audsid) as Data.Access_User_Datasource;
               iProject.ExecuteCommand(string.Format("DELETE Global.Access_User_Datasource WHERE Id = {0}", _auds.ID));
            }
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
