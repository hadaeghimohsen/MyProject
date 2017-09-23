using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.JobRouting.Jobs;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MyProject.Programs.Ui
{
    public partial class Wall : Form
    {
        public Wall()
        {
            InitializeComponent();
            _ActiveUI = new Stack<ActiveUi>();
            _ActiveUI.Push(new ActiveUi { Name = "Master", Ui = this });
            GoFullscreen();
        }
        class ActiveUi
        {
            public string Name { get; set; }
            public Control Ui { get; set; }
        }
        private Stack<ActiveUi> _ActiveUI;

        private void GoFullscreen()
        {
           if (toggleMode)
           {
              this.WindowState = FormWindowState.Normal;
              this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
              this.Bounds = Screen.PrimaryScreen.Bounds;
           }
           else
           {
              this.WindowState = FormWindowState.Maximized;
              this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Sizable;
           }
           toggleMode = !toggleMode;
        }
        private bool toggleMode = false;

        /// <summary>
        /// Global Shortcut
        /// F12 Full Screen Toggle
        /// Ctrl+F12 Minimize Windows Form
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="keyData"></param>
        /// <returns></returns>
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {         
           if (keyData == (Keys.F12 | Keys.Control))
              WindowState = FormWindowState.Minimized;
           else if (keyData == Keys.F12)
              GoFullscreen();
           else if(keyData == (Keys.Control | Keys.Alt | Keys.F11))
           {
              _DefaultGateway.Gateway(
                 new Job(SendType.External, "Program", "Commons:Desktop", 03 /* Execute DoWork4StartDrawer */, SendType.Self) { Input = toggleMode }
              );
           }
           else if(keyData == (Keys.Control | Keys.F10))
           {
              MessageBox.Show(_ActiveUI.Peek().Name);
           }
           else if(keyData <= Keys.Help || (keyData >= Keys.LWin && keyData <= Keys.Sleep) || (keyData >= Keys.F1))
           {
              Job _ProcessCmdKey =
                  new Job(SendType.External, "Program", _ActiveUI.Peek().Name, 0 /* Execute ProcessCmdKey */, SendType.SelfToUserInterface) { Input = keyData };

              _DefaultGateway.Gateway(_ProcessCmdKey);
           }
           return base.ProcessCmdKey(ref msg, keyData);
        }

        private void Wall_FormClosing(object sender, FormClosingEventArgs e)
        {
           switch (e.CloseReason)
           {
              case CloseReason.ApplicationExitCall:
                 break;
              case CloseReason.FormOwnerClosing:
                 break;
              case CloseReason.MdiFormClosing:
                 break;
              case CloseReason.None:
                 e.Cancel = true;
                 break;
              case CloseReason.TaskManagerClosing:
                 break;
              case CloseReason.UserClosing:
                 e.Cancel = true;
                 break;
              case CloseReason.WindowsShutDown:
                 break;
              default:
                 break;
           }

           if(e.Cancel)
           {
              _DefaultGateway.Gateway(new Job(SendType.External, "Localhost", "Commons", 25 /* Execute DoWork4Shutingdown */, SendType.Self));
           }
        }
    }
}
