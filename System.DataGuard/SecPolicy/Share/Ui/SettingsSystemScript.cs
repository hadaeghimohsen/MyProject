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
using DevExpress.XtraEditors;
using System.Globalization;
using System.Xml.Linq;
using System.IO;
using System.Security.Cryptography;
using System.Data.OleDb;
using Microsoft.Office.Interop.Excel;
using System.Runtime.InteropServices;

namespace System.DataGuard.SecPolicy.Share.Ui
{
   public partial class SettingsSystemScript : UserControl
   {
      public SettingsSystemScript()
      {
         InitializeComponent();
      }

      private bool requery = false;
      private int subsys;

      private void Back_Butn_Click(object sender, EventArgs e)
      {
         // Close Start Drawer
         _DefaultGateway.Gateway(
            new Job(SendType.External, "localhost", GetType().Name, 00 /* Execute DoWork4SettingsDrawer */, SendType.SelfToUserInterface) { Input = Keys.Escape }
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
         iProject = new Data.iProjectDataContext(ConnectionString);
         if(Tb_Master.SelectedTab == tp_001)
         {
            int scp = ScrpBs.Position;
            SubSysBs.DataSource = iProject.Sub_Systems.Where(s => s.STAT == "002" && s.SUB_SYS == subsys);
            ScrpBs.Position = scp;
         }
         requery = false;
      }

      private void AddScript_Butn_Click(object sender, EventArgs e)
      {
         var subsys = SubSysBs.Current as Data.Sub_System;
         if (subsys == null) return;
         if (ScrpBs.List.OfType<Data.Script>().Any(s => s.CODE == 0)) return;

         ScrpBs.AddNew();
         var scrp = ScrpBs.Current as Data.Script;
         scrp.SUB_SYS = subsys.SUB_SYS;

         Scrp_Gv.SelectRow(Scrp_Gv.RowCount - 1);

         iProject.Scripts.InsertOnSubmit(scrp);
      }

      private void DelScript_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var scrp = ScrpBs.Current as Data.Script;
            if (scrp == null) return;

            if (MessageBox.Show(this, "حذف", "آیا با حذف رکورد موافق هستید؟", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) != DialogResult.Yes) return;

            var rows = Scrp_Gv.GetSelectedRows();

            foreach (var r in rows)
            {
               var row = (Data.Script)Scrp_Gv.GetRow(r);
               iProject.Scripts.DeleteOnSubmit(row);
            }

            iProject.SubmitChanges();

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

      private void SaveScript_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            ScrpBs.EndEdit();

            iProject.SubmitChanges();

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

      private void AddScriptParameter_Butn_Click(object sender, EventArgs e)
      {
         var scrp = ScrpBs.Current as Data.Script;
         if (scrp == null) return;
         if (ScppBs.List.OfType<Data.Script_Parameter>().Any(s => s.CODE == 0)) return;

         ScppBs.AddNew();
         var scpp = ScppBs.Current as Data.Script_Parameter;
         scpp.SCRP_CODE = scrp.CODE;

         Scpp_Gv.SelectRow(Scpp_Gv.RowCount - 1);

         iProject.Script_Parameters.InsertOnSubmit(scpp);
      }

      private void DelScriptParameter_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var scpp = ScppBs.Current as Data.Script_Parameter;
            if (scpp == null) return;

            if (MessageBox.Show(this, "حذف", "آیا با حذف رکورد موافق هستید؟", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) != DialogResult.Yes) return;

            var rows = Scpp_Gv.GetSelectedRows();

            foreach (var r in rows)
            {
               var row = (Data.Script_Parameter)Scrp_Gv.GetRow(r);
               iProject.Script_Parameters.DeleteOnSubmit(row);
            }

            iProject.SubmitChanges();

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

      private void SaveScriptParameter_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            ScppBs.EndEdit();

            iProject.SubmitChanges();

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

      private void Execute_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            SaveScript_Butn_Click(null, null);
            var script = ScrpBs.Current as Data.Script;
            if (script == null) return;

            var cmnd = script.CMND;            

            switch (script.PARM_TYPE)
            {
               case "000":
               case "001":
                  // Static * Before Run Script Set Value Parameters
                  ScppBs.List.OfType<Data.Script_Parameter>().ToList().ForEach(sp => cmnd = cmnd.Replace(":" + sp.NAME, sp.INIT_VALU));
                  if(ExecuteType_Tbc.Value == 1)
                  {
                     // Execute None Query
                     _DefaultGateway.Gateway(
                        new Job(SendType.External, "localhost",
                           new List<Job>
                           {
                              new Job(SendType.SelfToUserInterface, GetType().Name, 100 /* Execute ExecuteNoneQuery */)
                              {
                                 Input = cmnd,
                                 Executive = ExecutiveType.Asynchronous,
                                 AfterChangedOutput = 
                                    new Action<object>((output) => 
                                    {
                                       if(InvokeRequired)
                                       {
                                          Invoke(new Action(() => { Result_Lb.Text = output.ToString(); })); 
                                       }
                                       else
                                       {
                                          Result_Lb.Text = output.ToString();
                                       }
                                    })
                              }
                           }
                        )
                     );
                  }
                  else
                  {
                     // Execute Data Adapter
                     _DefaultGateway.Gateway(
                        new Job(SendType.External, "localhost",
                           new List<Job>
                           {
                              new Job(SendType.SelfToUserInterface, GetType().Name, 101 /* Execute ExecuteDataAdapter */)
                              {
                                 Input = cmnd,
                                 Executive = ExecutiveType.Asynchronous,
                                 AfterChangedOutput = 
                                    new Action<object>((output) => 
                                    {
                                       if (InvokeRequired)
                                       {
                                          Invoke(new Action(() => 
                                          { 
                                             Result_Dgv.DataSource = (output as DataSet).Tables[0];
                                             Result_Lb.Text = Result_Dgv.RowCount.ToString();
                                          }));
                                       }
                                       else
                                       {
                                          Result_Dgv.DataSource = (output as DataSet).Tables[0];
                                          Result_Lb.Text = Result_Dgv.RowCount.ToString();
                                       }
                                    })
                              }
                           }
                        )
                     );
                  }
                  break;
               case "002":
                  // Dynamic * RunTime Set Value Parameters From Datasource (e.g. Excell File)
                  if (script.PARM_SORC == "004")
                  {
                     //string confile = string.Format(
                     //  @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source={0};" +
                     //  @"Extended Properties='Excel 8.0;HDR=Yes;'", FilePath_Txt.Text);
                     //using (OleDbConnection connection = new OleDbConnection(confile))
                     //{
                     //   connection.Open();
                     //   OleDbCommand command = new OleDbCommand(string.Format("select * from [{0}$]", SheetName_Txt.Text), connection);
                     //   using (OleDbDataReader dr = command.ExecuteReader())
                     //   {
                     //      while (dr.Read())
                     //      {
                     //         var row1Col0 = dr[0];
                     //         MessageBox.Show(row1Col0.ToString());
                     //      }
                     //   }
                     //}
                     Microsoft.Office.Interop.Excel.Application xlApp = new Microsoft.Office.Interop.Excel.Application();
                     Microsoft.Office.Interop.Excel.Workbook xlWorkbook = xlApp.Workbooks.Open(FilePath_Txt.Text);
                     Microsoft.Office.Interop.Excel._Worksheet xlWorksheet = xlWorkbook.Sheets[SheetName_Txt.Text];//xlWorkbook.Sheets[1];
                     Microsoft.Office.Interop.Excel.Range xlRange = xlWorksheet.UsedRange;

                     int rowCount = xlRange.Rows.Count;
                     int colCount = xlRange.Columns.Count;
                     int progressCount = (int)(100/ rowCount);
                     SourceInfoRows_Dgv.ColumnCount = colCount;
                     SourceInfoRows_Dgv.RowCount = rowCount - 1;
                     ExecuteRows_Pgb.Visible = true;

                     for (int i = 1; i == 1; i++)
                     {
                        for (int j = 1; j <= colCount; j++)
                        {
                           SourceInfoRows_Dgv.Columns[j - 1].Name = xlRange.Cells[i, j].Value2.ToString();
                        }
                     }

                     for (int i = 2; i <= rowCount; i++)
                     {
                        for (int j = 1; j <= colCount; j++)
                        {
                           //write the value to the Grid  
                           if (xlRange.Cells[i, j] != null && xlRange.Cells[i, j].Value2 != null)
                           {                              
                              SourceInfoRows_Dgv.Rows[i - 2].Cells[j - 1].Value = xlRange.Cells[i, j].Value2.ToString();
                           }
                           // Console.Write(xlRange.Cells[i, j].Value2.ToString() + "\t");  
                           //add useful things here!     
                        }
                        cmnd = script.CMND;
                        ScppBs.List.OfType<Data.Script_Parameter>()
                           .ToList()
                           .ForEach(sp =>
                              {
                                 var indx = SourceInfoRows_Dgv.Columns[sp.NAME].Index;
                                 cmnd = cmnd.Replace(":" + sp.NAME, SourceInfoRows_Dgv.Rows[i - 2].Cells[indx].Value.ToString() ); 
                              }
                           );
                        if (ExecuteType_Tbc.Value == 1)
                        {
                           // Execute None Query
                           _DefaultGateway.Gateway(
                              new Job(SendType.External, "localhost",
                                 new List<Job>
                                 {
                                    new Job(SendType.SelfToUserInterface, GetType().Name, 100 /* Execute ExecuteNoneQuery */)
                                    {
                                       Input = cmnd,
                                       AfterChangedOutput = 
                                          new Action<object>((output) => 
                                          {
                                             if(InvokeRequired)
                                             {
                                                Invoke(new Action(() => { Result_Lb.Text = output.ToString(); })); 
                                             }
                                             else
                                             {
                                                Result_Lb.Text = output.ToString();
                                             }
                                          })
                                    }
                                 }
                              )
                           );
                        }
                        else
                        {
                           // Execute Data Adapter
                           _DefaultGateway.Gateway(
                              new Job(SendType.External, "localhost",
                                 new List<Job>
                                 {
                                    new Job(SendType.SelfToUserInterface, GetType().Name, 101 /* Execute ExecuteDataAdapter */)
                                    {
                                       Input = cmnd,
                                       AfterChangedOutput = 
                                          new Action<object>((output) => 
                                          {
                                             if (InvokeRequired)
                                             {
                                                Invoke(new Action(() => 
                                                { 
                                                   Result_Dgv.DataSource = (output as DataSet).Tables[0];
                                                   Result_Lb.Text = Result_Dgv.RowCount.ToString();
                                                }));
                                             }
                                             else
                                             {
                                                Result_Dgv.DataSource = (output as DataSet).Tables[0];
                                                Result_Lb.Text = Result_Dgv.RowCount.ToString();
                                             }
                                          })
                                    }
                                 }
                              )
                           );
                        }
                        if (i % progressCount == 0)
                           ExecuteRows_Pgb.Position += progressCount;                           
                     }
                     ExecuteRows_Pgb.Position = 100; ;
                     ExecuteRows_Pgb.Visible = false;
                     //cleanup  
                     GC.Collect();
                     GC.WaitForPendingFinalizers();

                     //rule of thumb for releasing com objects:  
                     //  never use two dots, all COM objects must be referenced and released individually  
                     //  ex: [somthing].[something].[something] is bad  

                     //release com objects to fully kill excel process from running in the background  
                     Marshal.ReleaseComObject(xlRange);
                     Marshal.ReleaseComObject(xlWorksheet);

                     //close and release  
                     xlWorkbook.Close();
                     Marshal.ReleaseComObject(xlWorkbook);

                     //quit and release  
                     xlApp.Quit();
                     Marshal.ReleaseComObject(xlApp);  
                  }
                  break;
               default:
                  break;
            }
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }

      private void FilePath_Txt_ButtonPressed(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
      {
         try
         {
            if (SelectFile_Ofd.ShowDialog() != DialogResult.OK) return;

            FilePath_Txt.Text = SelectFile_Ofd.FileName;
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }
   }
}
