using System;
using System.Linq;
using System.Windows.Forms;
using System.JobRouting.Routering;
using System.JobRouting.Jobs;
using System.Xml.Linq;
using System.Collections.Generic;

namespace System.MessageBroadcast.Ui.SmsApp
{
    partial class WEBS_MESG_F : ISendRequest
    {
        public IRouter _DefaultGateway { get; set; }

        private Data.iProjectDataContext iProject;
        private Data.iScscDataContext iScsc;
        private string IScscConnectionString;
        private string IProjectConnectionString;
        private string CurrentUser;
        private string FormCaller;
        private XElement xinput;

        public void SendRequest(Job job)
        {
            switch (job.Method)
            {
                case 00:
                    ProcessCmdKey(job);
                    break;
                case 01:
                    Get(job);
                    break;
                case 02:
                    Set(job);
                    break;
                case 03:
                    Paint(job);
                    break;
                case 04:
                    UnPaint(job);
                    break;
                case 07:
                    LoadData(job);
                    break;
                case 10:
                    Actn_CalF_P(job);
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// Code 00
        /// </summary>
        private void ProcessCmdKey(Job job)
        {
            Keys keyData = (Keys)job.Input;

            if (keyData == Keys.Enter)
            {
                SendKeys.Send("{TAB}");
            }
            else if (keyData == Keys.Escape)
            {
                job.Next =
                    new Job(SendType.SelfToUserInterface, GetType().Name, 04 /* Execute UnPaint */);
            }

            job.Status = StatusType.Successful;
        }

        /// <summary>
        /// Code 01
        /// </summary>
        private void Get(Job job)
        {
            job.Status = StatusType.Successful;
        }

        /// <summary>
        /// Code 02
        /// </summary>
        private void Set(Job job)
        {
            var GetConnectionString =
                new Job(SendType.External, "Localhost", "Commons", 22 /* Execute GetConnectionString */, SendType.Self) { Input = "<Database>iProject</Database><Dbms>SqlServer</Dbms>" };
            _DefaultGateway.Gateway(GetConnectionString);

            IProjectConnectionString = GetConnectionString.Output.ToString();
            iProject = new Data.iProjectDataContext(GetConnectionString.Output.ToString());

            GetConnectionString =
                 new Job(SendType.External, "Localhost", "Commons", 22 /* Execute GetConnectionString */, SendType.Self) { Input = "<Database>iScsc</Database><Dbms>SqlServer</Dbms>" };
            _DefaultGateway.Gateway(GetConnectionString);

            IScscConnectionString = GetConnectionString.Output.ToString();
            iScsc = new Data.iScscDataContext(GetConnectionString.Output.ToString());


            CurrentUser = iProject.GET_CRNTUSER_U(new XElement("User", new XAttribute("actntype", "001")));
            var GetHostInfo = new Job(SendType.External, "Localhost", "Commons", 24 /* Execute DoWork4GetHosInfo */, SendType.Self);
            _DefaultGateway.Gateway(GetHostInfo);

            _DefaultGateway.Gateway(
                new Job(SendType.External, "Localhost", "Commons", 08 /* Execute LangChangToFarsi */, SendType.Self)
            );

            job.Status = StatusType.Successful;
        }

        /// <summary>
        /// Code 03
        /// </summary>
        private new void Paint(Job job)
        {
            Job _Paint = new Job(SendType.External, "Desktop",
                new List<Job>
                {
                    new Job(SendType.SelfToUserInterface, "Wall", 17 /* Execute ResetUi */),
                    new Job(SendType.SelfToUserInterface, "Wall", 15 /* Execute Push */) {  Input = new List<object> { string.Format("Msgb:{0}", this.GetType().Name), this }  },
                    new Job(SendType.SelfToUserInterface, "Wall", 01 /* Execute PastManualOnWall */) { Input = this }
                });
            _DefaultGateway.Gateway(_Paint);

            this.Enabled = true;
            job.Status = StatusType.Successful;
        }

        /// <summary>
        /// Code 04
        /// </summary>
        private void UnPaint(Job job)
        {
            job.Next =
                new Job(SendType.SelfToUserInterface, "Wall", 16 /* Execute Pop */,
                    new Job(SendType.SelfToUserInterface, "Wall", 02 /* Execute RemoveFromWall */,
                        new Job(SendType.SelfToUserInterface, "Wall", 17 /* Execute ResetUi */)) { Input = this });

            job.Status = StatusType.Successful;
        }

        /// <summary>
        /// Code 07
        /// </summary>
        private void LoadData(Job job)
        {
            // بارگذاری تنظیمات وب‌سرویس لیدوما (Serv_Type = '005') جهت استفاده در احراز هویت خودکار
            if (iProject != null)
            {
                var settings = iProject.Message_Broad_Settings.Where(m => m.SERV_TYPE == "005").ToList();
                MgbsBs.DataSource = settings;

                // مقداردهی خودکار فیلدهای تب لاگین از اولین رکورد تنظیمات
                var first = settings.FirstOrDefault();
                if (first != null)
                {
                    BaseUrl_Txt.EditValue = first.BASE_URL;
                    UserName_Txt.EditValue = first.USER_NAME;
                    Password_Txt.EditValue = first.PASS_WORD;
                }
            }

            job.Status = StatusType.Successful;
        }

        /// <summary>
        /// Code 10
        /// </summary>
        private void Actn_CalF_P(Job job)
        {
            xinput = job.Input as XElement;
            job.Status = StatusType.Successful;
        }
    }
}
