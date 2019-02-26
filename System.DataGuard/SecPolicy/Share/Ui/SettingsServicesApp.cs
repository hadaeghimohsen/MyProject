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
using System.Xml.Linq;

namespace System.DataGuard.SecPolicy.Share.Ui
{
   public partial class SettingsServicesApp : UserControl
   {
      public SettingsServicesApp()
      {
         InitializeComponent();
      }

      private void Back_Butn_Click(object sender, EventArgs e)
      {
         _DefaultGateway.Gateway(
            new Job(SendType.External, "localhost", GetType().Name, 00 /* Execute DoWork4Settings */, SendType.SelfToUserInterface) { Input = Keys.Escape }
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

         }
         else if(Tb_Master.SelectedTab == tp_002)
         {
            SmsConfBs.DataSource = iProject.Message_Broad_Settings;
            UserBs.DataSource = iProject.Users;
         }
      }

      private void SubmitChange_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            SmsConfBs.EndEdit();
            iProject.SubmitChanges();
            SubmitChange_Butn.Visible = false;
         }
         catch { }      
      }

      private void SmsConfBs_ListChanged(object sender, ListChangedEventArgs e)
      {
         SubmitChange_Butn.Visible = true;
      }

      private void CreditCheck_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            SubmitChange_Butn_Click(null, null);

            SmsResult_Lb.Appearance.Image = null;

            // To Do List
            _DefaultGateway.Gateway(
               new Job(SendType.External, "localhost", "Commons:Program:Msgb", 2 /* Execute Mstr_Page_F */, SendType.Self)               
            );

            _DefaultGateway.Gateway(
               new Job(SendType.External, "localhost", "Commons:Program:Msgb:MSTR_PAGE_F", 00 /* Execute ProcessCmdKey */, SendType.SelfToUserInterface)
               {
                  Input = Keys.Escape
               }
            );

            _DefaultGateway.Gateway(
               new Job(SendType.External, "localhost",
                  new List<Job>
                  {
                     new Job(SendType.External, "Commons",
                        new List<Job>
                        {
                           new Job(SendType.External, "Program",
                              new List<Job>
                              {
                                 new Job(SendType.External, "Msgb",
                                    new List<Job>
                                    {
                                       new Job(SendType.SelfToUserInterface, "MSTR_PAGE_F", 10 /* Execuet Actn_Calf_F */)
                                       {
                                          Input =
                                             new XElement("SmsConf",
                                                new XAttribute("actntype", "getcredit")
                                             ),
                                          AfterChangedOutput =
                                             new Action<object>((output) =>
                                             {
                                                var xoutput = output as XDocument;
                                                if (InvokeRequired)
                                                {
                                                   Invoke(
                                                      new Action(() => 
                                                      {
                                                         SmsResult_Lb.Appearance.Image = System.DataGuard.Properties.Resources.IMAGE_1603;
                                                         SendCreditCount_Txt.Text = xoutput.Descendants("SendCredit").FirstOrDefault().Value;
                                                         SendCreditAmnt_Txt.Text = xoutput.Descendants("SMS_SendFee").FirstOrDefault().Value;
                                                         ReceiveCreditCount_Txt.Text = xoutput.Descendants("RecieveCredit").FirstOrDefault().Value;
                                                         ReceiveCreditAmnt_Txt.Text = xoutput.Descendants("SMS_RecieveFee").FirstOrDefault().Value;
                                                      })
                                                   );
                                                }
                                                else
                                                {
                                                   SmsResult_Lb.Appearance.Image = System.DataGuard.Properties.Resources.IMAGE_1603;
                                                   SendCreditCount_Txt.Text = xoutput.Descendants("SendCredit").FirstOrDefault().Value;
                                                   SendCreditAmnt_Txt.Text = xoutput.Descendants("SMS_SendFee").FirstOrDefault().Value;
                                                   ReceiveCreditCount_Txt.Text = xoutput.Descendants("RecieveCredit").FirstOrDefault().Value;
                                                   ReceiveCreditAmnt_Txt.Text = xoutput.Descendants("SMS_RecieveFee").FirstOrDefault().Value;
                                                }
                                             })
                                       }
                                    }
                                 )
                              }
                           )
                        }
                     )
                  }
               )
            );
         }
         catch
         {
            SmsResult_Lb.Appearance.Image = System.DataGuard.Properties.Resources.IMAGE_1577;            
         }
      }

      #region Wheater
      private void ShowRespons_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            //var wthrengn = iProject.Wheater_Engines.FirstOrDefault();
            //string key = wthrengn.API_KEY; 
            //IRepository repo = new Repository(); 
            //var GetCityForecastWeatherResult = repo.GetWeatherData(key, GetBy.CityName, RegnName_Txt.Text, Days.Three); 
            //var GetByLatLongForecastWeatherResult = repo.GetWeatherDataByLatLong(key, "30.2669444", "-97.7427778", Days.Three); 
            //var GetByIPForecastWeatherResult = repo.GetWeatherDataByAutoIP(key, Days.Three); 

            //var GetCityCurrentWeatherResult = repo.GetWeatherData( key, GetBy.CityName, RegnName_Txt.Text); 
            //var GetByLatLongCurrentWeatherResult = repo.GetWeatherDataByLatLong( key, "30.2669444", "-97.7427778"); 
            //var GetByIPCurrentWeatherResult = repo.GetWeatherDataByAutoIP( key); 
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }
      #endregion
   }
}
