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
using System.Xml.Linq;

namespace System.Scsc.Ui.Admission
{
   public partial class NEW_FNGR_F : UserControl
   {
      public NEW_FNGR_F()
      {
         InitializeComponent();
      }

      private void Btn_AdmOneMonth_Click(object sender, EventArgs e)
      {
         Job _InteractWithScsc =
            new Job(SendType.External, "Localhost",
               new List<Job>
               {
                  new Job(SendType.Self, 123 /* Execute Adm_FIGH_F */),
                  new Job(SendType.SelfToUserInterface, "ADM_FIGH_F", 10 /* Actn_CalF_P */){Input = new XElement("Request", new XAttribute("type", "fighter"), new XAttribute("enrollnumber", Lbl_FingerPrint.Text))}
               });
         _DefaultGateway.Gateway(_InteractWithScsc);
      }

      private void Btn_AdmNumbSession_Click(object sender, EventArgs e)
      {
         Job _InteractWithScsc =
            new Job(SendType.External, "Localhost",
               new List<Job>
               {
                  new Job(SendType.Self, 97 /* Execute Oic_Smor_F */),
                  new Job(SendType.SelfToUserInterface,"OIC_SMOR_F", 10 /* Execute Actn_CalF_F */){Input = new XElement("Request", new XAttribute("type", "tp_001"), new XAttribute("enrollnumber", Lbl_FingerPrint.Text))}
               });
         _DefaultGateway.Gateway(_InteractWithScsc);
      }

      private void Btn_AdmGroupSession_Click(object sender, EventArgs e)
      {
         Job _InteractWithScsc =
            new Job(SendType.External, "Localhost",
               new List<Job>
               {
                  new Job(SendType.Self, 96 /* Execute Oic_Sone_F */),
                  new Job(SendType.SelfToUserInterface,"OIC_SONE_F", 10 /* Execute Actn_CalF_F */){Input = new XElement("Request", new XAttribute("type", "tp_001"), new XAttribute("enrollnumber", Lbl_FingerPrint.Text))}
               });
         _DefaultGateway.Gateway(_InteractWithScsc);
      }

      private void Btn_ApproveOneMonth_Click(object sender, EventArgs e)
      {
         if (iScsc.Fighters.FirstOrDefault(f => f.FNGR_PRNT_DNRM == Lbl_FingerPrint.Text && (f.FGPB_TYPE_DNRM == "001" || f.FGPB_TYPE_DNRM == "005" || f.FGPB_TYPE_DNRM == "006")) == null) return;

         Job _InteractWithScsc =
            new Job(SendType.External, "Localhost",
               new List<Job>
               {
                  new Job(SendType.Self, 64 /* Execute Adm_Totl_F */),
                  new Job(SendType.SelfToUserInterface, "ADM_TOTL_F", 10 /* Actn_CalF_P */){Input = new XElement("Request", new XAttribute("type", "renewcontract"), new XAttribute("enrollnumber", Lbl_FingerPrint.Text))}
               });
         _DefaultGateway.Gateway(_InteractWithScsc);
      }

      private void Btn_IncNumbSession_Click(object sender, EventArgs e)
      {
         if (iScsc.Fighters.FirstOrDefault(f => f.FNGR_PRNT_DNRM == Lbl_FingerPrint.Text && (f.FGPB_TYPE_DNRM == "001" || f.FGPB_TYPE_DNRM == "005" || f.FGPB_TYPE_DNRM == "006")) != null) return;

         Job _InteractWithScsc =
            new Job(SendType.External, "Localhost",
               new List<Job>
               {
                  new Job(SendType.Self, 97 /* Execute Oic_Smor_F */),
                  new Job(SendType.SelfToUserInterface,"OIC_SMOR_F", 10 /* Execute Actn_CalF_F */){Input = new XElement("Request", new XAttribute("type", "tp_002"), new XAttribute("enrollnumber", Lbl_FingerPrint.Text))}
               });
         _DefaultGateway.Gateway(_InteractWithScsc);
      }

      private void Btn_CoachAdmission_Click(object sender, EventArgs e)
      {
         Job _InteractWithScsc =
            new Job(SendType.External, "Localhost",
               new List<Job>
               {
                  new Job(SendType.Self, 64 /* Execute Adm_Totl_F */),
                  new Job(SendType.SelfToUserInterface, "ADM_TOTL_F", 10 /* Actn_CalF_P */){Input = new XElement("Request", new XAttribute("type", "coach"), new XAttribute("enrollnumber", Lbl_FingerPrint.Text))}
               });
         _DefaultGateway.Gateway(_InteractWithScsc);
      }

      private void Btn_ComposeAdmission_Click(object sender, EventArgs e)
      {
         Job _InteractWithScsc =
            new Job(SendType.External, "Localhost",
               new List<Job>
               {
                  new Job(SendType.Self, 114 /* Execute Oic_Smsn_F */),
                  new Job(SendType.SelfToUserInterface,"OIC_SMSN_F", 10 /* Execute Actn_CalF_F */){Input = new XElement("Request", new XAttribute("type", "tp_001"), new XAttribute("enrollnumber", Lbl_FingerPrint.Text))}
               });
         _DefaultGateway.Gateway(_InteractWithScsc);
      }

      private void Btn_Back_Click(object sender, EventArgs e)
      {
         _DefaultGateway.Gateway(
            new Job(SendType.External, "localhost", GetType().Name, 00 /* Execute ProcessCmdKey */, SendType.SelfToUserInterface) { Input = Keys.Escape }
         );
      }

      private void Btn_ApproveExitBlocking_Click(object sender, EventArgs e)
      {
         try
         {
            if (MessageBox.Show(this, "آیا می خواهید تاریخ بلوکه شدن را لغو کنید", "لغو بلوکه شدن", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) != DialogResult.Yes) return;
            var figh = iScsc.Fighters.FirstOrDefault(f => f.FNGR_PRNT_DNRM == Lbl_FingerPrint.Text);

            iScsc.SCV_MBSP_P(
               new XElement("Process",
                  new XElement("Fighter",
                     new XAttribute("fileno", figh.FILE_NO)
                  )
               )
            );

            MessageBox.Show(this, "عضو مورد نظر از حالت بلوکه خارج شد. لطفا حضور غیاب را دوباره تکرار کنید.");

            Btn_Back_Click(null, null);
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }
   }
}
