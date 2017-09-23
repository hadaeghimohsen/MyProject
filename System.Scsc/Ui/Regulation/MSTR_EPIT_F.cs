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

namespace System.Scsc.Ui.Regulation
{
   public partial class MSTR_EPIT_F : UserControl
   {
      public MSTR_EPIT_F()
      {
         InitializeComponent();
      }
      private bool requery = false;

      partial void expense_ItemBindingNavigatorSaveItem_Click(object sender, EventArgs e);

      private void Btn_Back_Click(object sender, EventArgs e)
      {
         _DefaultGateway.Gateway(
            new Job(SendType.External, "localhost", GetType().Name, 00 /* Execute ProcessCmdKey */, SendType.SelfToUserInterface) { Input = Keys.Escape }
         );
      }
      
      private void GropBnAReload1_Click(object sender, EventArgs e)
      {
         try
         {
            iScsc = new Data.iScscDataContext(ConnectionString);
            GropBs2.DataSource = iScsc.Group_Expenses;
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }

      private void GropBnASav1_Click(object sender, EventArgs e)
      {
         try
         {
            GropBs2.EndEdit();

            iScsc.SubmitChanges();
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
               iScsc = new Data.iScscDataContext(ConnectionString);
               GropBs2.DataSource = iScsc.Group_Expenses;
               requery = false;
            }
         }
      }

   }
}
