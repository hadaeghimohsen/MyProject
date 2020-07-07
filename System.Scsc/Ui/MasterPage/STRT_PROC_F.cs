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

namespace System.Scsc.Ui.MasterPage
{
   public partial class STRT_PROC_F : UserControl
   {
      public STRT_PROC_F()
      {
         InitializeComponent();
      }

      #region Info Region Control
      private void AddNew_Butn_MouseEnter(object sender, EventArgs e)
      {
         Info_Lb.Text = "اگر شما تا به حال درون سیستم ما ثبت نشده اید می توانید با وارد شده به قسمت ثبت نام جدید اطلاعات اولیه خودتان مانند نام، فامیلی، کدملی و موبایل ثبت نام خود را انجام دهید";
      }

      private void AddNew_Butn_MouseLeave(object sender, EventArgs e)
      {
         Info_Lb.Text = "";
      }

      private void NewExtension_Butn_MouseEnter(object sender, EventArgs e)
      {
         Info_Lb.Text = "اگر شمااز قبل درون سیستم ما ثبت شده اید می توانید با وارد شده به قسمت تمدید جدیدی برای خودتان انجام دهید با وارد کردن کد ملی به آسانی می توانید اینکار را انجام دهید";
      }

      private void NewExtension_Butn_MouseLeave(object sender, EventArgs e)
      {
         Info_Lb.Text = "";
      }

      private void Other_Butn_MouseEnter(object sender, EventArgs e)
      {
         Info_Lb.Text = "عملیات های متفرقه ای درون سیستم هست که با وارد شدن به آن می توانید انتخاب کنید";
      }

      private void Other_Butn_MouseLeave(object sender, EventArgs e)
      {
         Info_Lb.Text = "";
      }
      #endregion
   }
}
