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
using DevExpress.XtraTreeList.Nodes;
using DevExpress.XtraTreeList;

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
            var gropexpn = GropBs2.Position;
            GropBs2.DataSource = iScsc.Group_Expenses;
            GropBs2.Position = gropexpn;
            requery = false;
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
            Gexp_tre.PostEditor();
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
               int gexp = GropBs2.Position;
               GropBs2.DataSource = iScsc.Group_Expenses;
               GropBs2.Position = gexp;
               requery = false;
            }
         }
      }

      private void AddSubGrop_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var upgrop = GropBs2.Current as Data.Group_Expense;
            if (upgrop == null) return;

            var grop = GropBs2.AddNew() as Data.Group_Expense;
            if (grop == null) return;
            grop.GEXP_CODE = upgrop.CODE;

            iScsc.Group_Expenses.InsertOnSubmit(grop);
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }

      private void Gexp_tre_MouseDown(object sender, MouseEventArgs e)
      {
         if (e.Button == System.Windows.Forms.MouseButtons.Left)
         {
            var tlhi = Gexp_tre.CalcHitInfo(e.Location);
            if (tlhi.Node != null)
            {
               Gexp_tre.DoDragDrop(tlhi.Node, DragDropEffects.Move);
            }
         }  
      }

      private void Gexp_tre_DragOver(object sender, DragEventArgs e)
      {
         if (!e.Data.GetDataPresent("Text"))
         { // check file drop  
            e.Effect = DragDropEffects.None;
            return;
         }
         else
            e.Effect = DragDropEffects.Copy;  
      }

      private void Gexp_tre_DragDrop(object sender, DragEventArgs e)
      {
         if (e.Data.GetDataPresent(DataFormats.UnicodeText, true))
         {
            var data = e.Data.GetData(DataFormats.UnicodeText, true);
            if (data != null)
            {
               if (data is string)
               {
                  var dataText = data as string; // done!
                  try
                  {
                     TreeListNode dragNode, targetNode;
                     TreeList tl = sender as TreeList;
                     Point p = tl.PointToClient(new Point(e.X, e.Y));

                     dragNode = e.Data.GetData(typeof(TreeListNode)) as TreeListNode;
                     targetNode = tl.CalcHitInfo(p).Node;

                     iScsc.ExecuteCommand("BEGIN DECLARE @list NVARCHAR(MAX) = {0}; INSERT INTO dbo.Group_Expense(GEXP_CODE ,CODE , ORDR,GROP_DESC ) SELECT {1}, dbo.GNRT_NVID_U(), ROW_NUMBER() OVER (ORDER BY t.id), LTRIM(t.Item) FROM dbo.SplitString(@list, CHAR(10)) t WHERE NOT EXISTS (SELECT * FROM dbo.Group_Expense ge1 WHERE ge1.Gexp_Code = {1} AND ge1.Grop_Desc = LTRIM(t.Item)); END;", dataText, (long?)targetNode.GetValue("CODE"));
                     requery = true;
                     e.Effect = DragDropEffects.None;
                  }
                  catch (Exception exc) { MessageBox.Show(exc.Message); }
                  finally
                  {
                     if (requery)
                     {
                        var crnt = GropBs2.Current as Data.Group_Expense;

                        _DefaultGateway.Gateway(
                           new Job(SendType.External, "localhost", "Wall", 22 /* Execute SetSystemNotification */, SendType.SelfToUserInterface)
                           {
                              Input =
                                 new List<object>
                                 {
                                    ToolTipIcon.Info,
                                    string.Format("زیر گروه های جدید برای  گروه {0} اضافه شد", crnt.GROP_DESC),
                                    "اطلاعات گروه بروزرسانی شد",
                                    100
                                 }
                           }
                        );
                     }
                  }
               }
            }
         }         
      }

      private void DelGrop_Tsm_Click(object sender, EventArgs e)
      {
         try
         {
            var crnt = GropBs2.Current as Data.Group_Expense;
            if (crnt == null) return;

            iScsc.ExecuteCommand("DELETE dbo.Group_Expense WHERE Code = {0}", crnt.CODE);

            _DefaultGateway.Gateway(
               new Job(SendType.External, "localhost", "Wall", 22 /* Execute SetSystemNotification */, SendType.SelfToUserInterface)
               {
                  Input =
                     new List<object>
                     {
                        ToolTipIcon.Info,
                        string.Format("گروه {0} حذف شد", crnt.GROP_DESC),
                        "اطلاعات گروه حذف شد",
                        100
                     }
               }
            );
         }
         catch { }
      }

   }
}
