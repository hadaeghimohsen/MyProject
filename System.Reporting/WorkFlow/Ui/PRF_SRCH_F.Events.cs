using DevExpress.XtraEditors;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.JobRouting.Jobs;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.Reporting.WorkFlow.Ui
{
   partial class PRF_SRCH_F
   {
      enum FilterType { None = 0, Title = 1 };

      partial void be_search_EditValueChanged(object sender, EventArgs e)
      {
         if (be_search.Text == string.Empty)
         {
            sb_title.Text = "فرم پارامتری";

            _DefaultGateway.Gateway(
               new Job(SendType.External, "localhost",
                  new List<Job>
                  {
                     new Job(SendType.SelfToUserInterface, "PRF_SPRF_F", 08 /* Execute CancelFilterItem */)
                     {
                        Input = be_search.Text.Replace(" ", string.Empty),
                        Executive = ExecutiveType.Asynchronous,
                     }
                  }));
            return;
         }

         if (TitleCountOpr <= 1)
            ++TitleCountOpr;

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
         set
         {
            filter = value;
            sb_title.Appearance.BackColor = Color.Transparent;
            switch (filter)
            {
               case FilterType.None:
                  break;
               case FilterType.Title:
                  sb_title.Appearance.BackColor = Color.DarkGray;
                  be_search_EditValueChanged(sb_title, null);
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
            if (!(titleCountFinish && titleFilterFinish)) return;

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
               new Job(SendType.SelfToUserInterface, "PRF_SPRF_F", 09 /* Execute TitleCountItem */)
               {
                  Input = be_search.Text.Replace(" ", string.Empty),
                  Executive = ExecutiveType.Asynchronous,
                  AfterChangedOutput = new Action<object>(
                     (output) =>
                     {
                        Invoke(new Action(() =>
                        {
                           titleCountFinish = true;
                           sb_title.Text = string.Format("فرم پارامتری      {0}", output);
                           --titleCountOpr;
                        }));
                     })
               });
            if (filter == FilterType.Title)
            {
               /* اجرا کردن اعمال فیلترینگ بر روی آیتم های فرم مقصد */
               filterList.Add(
                  new Job(SendType.SelfToUserInterface, "PRF_SPRF_F", 10 /* Execute TitleFilterItem */)
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


   }
}
