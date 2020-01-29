using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using System.JobRouting.Jobs;

namespace System.MessageBroadcast.Ui.SmsApp
{
   public partial class SEND_MESG_F : UserControl
   {
      public SEND_MESG_F()
      {
         InitializeComponent();
      }

      private bool requery = false;
      private int? subsys = 0;
      private long? rfid = 0;
      private string key1rfid, key2rfid;
      private string phonnumb = "";

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
         if (Tb_Master.SelectedTab == tp_004)
         {
            int sms = SmsSendedBs.Position;
            SmsSendedBs.DataSource = iProject.Sms_Message_Boxes;
            SmsSendedBs.Position = sms;

            if(xinput.Attribute("filtering") != null)
            {
               switch(xinput.Attribute("filtering").Value)
               {
                  case "phonnumb":
                     SmsSended_Gv.ActiveFilterString = string.Format("PHON_NUMB = '{0}'", xinput.Attribute("valu").Value);
                     break;
                  default:
                     SmsSended_Gv.ActiveFilterString = "";
                     break;
               }
            }
         }
         else if(Tb_Master.SelectedTab == tp_005 || Tb_Master.SelectedTab == tp_001)
         {
            int dmsg = DMsgBs.Position;
            DMsgBs.DataSource = iProject.Default_Messages;
            DMsgBs.Position = dmsg;
         }
         NewSms_Pn.Visible = false;
         requery = false;
      }

      private void Back_Butn_Click(object sender, EventArgs e)
      {
         _DefaultGateway.Gateway(
            new Job(SendType.External, "localhost", GetType().Name, 00 /* Execute ProcessCmdKey */, SendType.SelfToUserInterface) { Input = Keys.Escape }
         );
      }

      private void SubmitChange_Butn_Click(object sender, EventArgs e)
      {

      }

      private void NewSms_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            NewSms_Pn.Visible = true;
            if (SmsSendedBs.List.OfType<Data.Sms_Message_Box>().Any(s => s.MBID == 0)) return;
            var sms = SmsSendedBs.AddNew() as Data.Sms_Message_Box;
            sms.SUB_SYS = subsys;
            sms.PHON_NUMB = phonnumb;
            sms.RFID = rfid;
            sms.KEY1_RFID = key1rfid;
            sms.KEY2_RFID = key2rfid;
            sms.ACTN_DATE = DateTime.Now;
            sms.LINE_TYPE = "001";
            sms.MSGB_TYPE = "005";
            sms.STAT = "001";

            iProject.Sms_Message_Boxes.InsertOnSubmit(sms);
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }

      private void SaveSendSms_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            SmsSendedBs.EndEdit();
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

      private void CnclNewSms_Butn_Click(object sender, EventArgs e)
      {
         NewSms_Pn.Visible = false;
      }

      private void AddDmsg_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            if (DMsgBs.List.OfType<Data.Default_Message>().Any(dm => dm.CODE == 0)) return;
            var dmsg = DMsgBs.AddNew() as Data.Default_Message;

            iProject.Default_Messages.InsertOnSubmit(dmsg);            
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }

      private void DelDmsg_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var dmsg = DMsgBs.Current as Data.Default_Message;
            if (dmsg == null) return;

            if (MessageBox.Show(this, "با حذف متن پیامک مطمئن هستید؟", "هشدار", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) == DialogResult.Yes) return;

            iProject.Default_Messages.DeleteOnSubmit(dmsg);

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

      private void SaveDmsg_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            DMsgBs.EndEdit();
            Dmsg_Gv.PostEditor();            

            if (DMsgBs.List.OfType<Data.Default_Message>().Any(dm => dm.MESG_NAME == null || dm.SUB_SYS == null || dm.MESG_TEXT == null))
            {
               throw new Exception("اطلاعات ستون های زیر سیستم، عنوان پیام و متن پیام به درستی وارد نشده، لطفا اطلاعات را درست و کامل وارد کنید");
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

      private void DMsg_Lov_ButtonPressed(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
      {
         try
         {
            var dmsgid = DMsg_Lov.EditValue;
            if(e.Button.Index == 1)
            {
               if(dmsgid == null) return;
               var dmsg = DMsgBs.List.OfType<Data.Default_Message>().FirstOrDefault(d => d.CODE == (long)dmsgid);

               string source = dmsg.MESG_TEXT;

               // سلام @(نام مشتری) بابت ارسال
               if(source.IndexOf("@(") >= 0)
               {
                  string placeHolder = "";
                  do
                  {
                     placeHolder =
                        source.Substring(
                        /* Start Position */source.IndexOf("@("),
                        /* End Position */ source.IndexOf(")") - source.IndexOf("@(") + 1
                        );

                     source = source.Replace(placeHolder, Microsoft.VisualBasic.Interaction.InputBox(placeHolder, "لطفا ورودی خود را وارد کنید"));
                  } while (source.IndexOf("@(") >= 0);

                  mSGB_TEXTRichTextBox.Text = source;
               }
            }
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }
   }
}
