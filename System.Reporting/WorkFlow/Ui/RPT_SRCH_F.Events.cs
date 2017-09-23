using DevExpress.XtraEditors;
using System;
using System.Collections.Generic;
using System.JobRouting.Jobs;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace System.Reporting.WorkFlow.Ui
{
   partial class RPT_SRCH_F
   {
      enum FilterType { None = 0, Title = 1, Content = 2, Creator = 3, FileSystem = 4};

      partial void be_search_EditValueChanged(object sender, EventArgs e)
      {
         if (be_search.Text == string.Empty)
         {
            sb_title.Text = "عنوان گزارشات";
            sb_filesystem.Text = "فایل گزارشات";

            _DefaultGateway.Gateway(
               new Job(SendType.External, "localhost",
                  new List<Job>
                  {
                     new Job(SendType.SelfToUserInterface, "RPT_SRPT_F", 08 /* Execute CancelFilterItem */)
                     {
                        Input = be_search.Text.Replace(" ", string.Empty),
                        Executive = ExecutiveType.Asynchronous,
                     }
                  }));
            return;
         }

         if (TitleCountOpr <= 1)
            ++TitleCountOpr;

         if (FileCountOpr <= 1)
            ++FileCountOpr;
      }

      partial void sb_filtertype_Click(object sender, EventArgs e)
      {
         Filter = (FilterType)Convert.ToInt32((sender as SimpleButton).Tag);
         be_search.Focus();
      }

      private FilterType filter = FilterType.None;
      private FilterType Filter
      {
         get { return filter; }
         set {
            filter = value;
            sb_title.Appearance.BackColor = Color.Transparent;
            sb_content.Appearance.BackColor = Color.Transparent;
            sb_creator.Appearance.BackColor = Color.Transparent;
            sb_filesystem.Appearance.BackColor = Color.Transparent;
            switch (filter)
            {
               case FilterType.None:
                  break;
               case FilterType.Title:
                  sb_title.Appearance.BackColor = Color.DarkGray;
                  be_search_EditValueChanged(sb_title, null);
                  break;
               case FilterType.Content:
                  sb_content.Appearance.BackColor = Color.DarkGray;
                  be_search_EditValueChanged(sb_content, null);
                  break;
               case FilterType.Creator:
                  sb_creator.Appearance.BackColor = Color.DarkGray;
                  be_search_EditValueChanged(sb_creator, null);
                  break;
               case FilterType.FileSystem:
                  sb_filesystem.Appearance.BackColor = Color.DarkGray;
                  be_search_EditValueChanged(sb_filesystem, null);
                  break;
            }
         }
      }

      bool titleCountFinish = true, titleFilterFinish = true;
      private int titleCountOpr = 0;
      private int TitleCountOpr
      {
         get { return titleCountOpr; }
         set
         {
            if(!(titleCountFinish && titleFilterFinish)) return;

            titleCountOpr = value;

            if (titleCountOpr == 1)
            {
               List<Job> filterList = new List<Job>();
               titleFilterOpr(filterList);
               /* اجرا کردن فرایند اجرای فیلترینگ مربوط به تعداد و یا اعمال فیلترینگ */
               _DefaultGateway.Gateway(
                  new Job(SendType.External, "Localhost", filterList));
            }
         }
      }

      private void titleFilterOpr(List<Job> filterList)
      {
         if (be_search.Text.Replace(" ", string.Empty).Length > 0)
         {
            titleCountFinish = false;
            /* درخواست اجرا کردن تعداد آیتم های که با فیلتر وارد شده تناسب دارند */            
            filterList.Add(
               new Job(SendType.SelfToUserInterface, "RPT_SRPT_F", 09 /* Execute TitleCountItem */)
               {
                  Input = be_search.Text.Replace(" ", string.Empty),
                  Executive = ExecutiveType.Asynchronous,
                  AfterChangedOutput = new Action<object>(
                     (output) =>
                        {
                           Invoke(new Action(() =>
                              {
                                 titleCountFinish = true;
                                 sb_title.Text = string.Format("عنوان گزارشات      {0}", output);
                                 --titleCountOpr;
                              }));
                        })
               });
            if (filter == FilterType.Title)
            {
               /* اجرا کردن اعمال فیلترینگ بر روی آیتم های فرم مقصد */
               filterList.Add(
                  new Job(SendType.SelfToUserInterface, "RPT_SRPT_F", 10 /* Execute TitleFilterItem */)
                  {
                     Input = be_search.Text.Replace(" ", string.Empty),
                     Executive = ExecutiveType.Asynchronous,
                     AfterChangedOutput = new Action<object>(
                        (output) =>
                        {
                           titleFilterFinish = true;
                        })
                  });
            }
            else
               titleFilterFinish = true;
         }         
      }

      bool fileCountFinish = true, fileFilterFinish = true;
      private int fileCountOpr = 0;
      private int FileCountOpr
      {
         get { return fileCountOpr; }
         set {
            if (!(fileCountFinish && fileFilterFinish)) return;

            fileCountOpr = value;

            if (fileCountOpr == 1)
            {
               List<Job> filterList = new List<Job>();
               fileFilterOpr(filterList);
               /* اجرا کردن فرایند اجرای فیلترینگ مربوط به تعداد و یا اعمال فیلترینگ */
               _DefaultGateway.Gateway(
                  new Job(SendType.External, "Localhost", filterList));
            }
         }
      }

      private void fileFilterOpr(List<Job> filterList)
      {
         if (be_search.Text.Replace(" ", string.Empty).Length > 0)
         {
            fileCountFinish = false;
            /* درخواست اجرا کردن تعداد آیتم های که با فیلتر وارد شده تناسب دارند */
            filterList.Add(
               new Job(SendType.SelfToUserInterface, "RPT_SRPT_F", 11 /* Execute FileCountItem */)
               {
                  Input = be_search.Text.Replace(" ", string.Empty),
                  Executive = ExecutiveType.Asynchronous,
                  AfterChangedOutput = new Action<object>(
                     (output) =>
                     {
                        Invoke(new Action(() =>
                        {
                           fileCountFinish = true;
                           sb_filesystem.Text = string.Format("فایل گزارشات      {0}", output);
                           --fileCountOpr;
                        }));
                     })
               });
            if (filter == FilterType.FileSystem)
            {
               /* اجرا کردن اعمال فیلترینگ بر روی آیتم های فرم مقصد */
               filterList.Add(
                  new Job(SendType.SelfToUserInterface, "RPT_SRPT_F", 12 /* Execute FileFilterItem */)
                  {
                     Input = be_search.Text.Replace(" ", string.Empty),
                     Executive = ExecutiveType.Asynchronous,
                     AfterChangedOutput = new Action<object>(
                        (output) =>
                        {
                           fileFilterFinish = true;
                        })
                  });
            }
            else
               fileFilterFinish = true;
         } 
      }
   }
}
