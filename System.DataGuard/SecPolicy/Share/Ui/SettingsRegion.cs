﻿using System;
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
using System.IO;
using System.Drawing.Imaging;
using System.Xml.Linq;
using System.Web;
using System.Net;

namespace System.DataGuard.SecPolicy.Share.Ui
{
   public partial class SettingsRegion : UserControl
   {
      public SettingsRegion()
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
         UserBs.DataSource = iProject.Users.Where(u => u.USERDB.ToUpper() == CurrentUser.ToUpper());
      }

      private void UserBs_ListChanged(object sender, ListChangedEventArgs e)
      {
         SubmitChange_Butn.Visible = true;
      }

      private void SubmitChange_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            UserBs.EndEdit();
            iProject.SubmitChanges();
            SubmitChange_Butn.Visible = false;
         }
         catch (Exception)
         {}         
      }

      private void UserBs_CurrentChanged(object sender, EventArgs e)
      {
         var user = UserBs.Current as Data.User;
         if (user == null) return;

         // Set Region Language
         user.REGN_LANG = user.REGN_LANG == null ? "001" : user.REGN_LANG;

         // Set Right To Left
         user.RTL_STAT = user.RTL_STAT == null ? "001" : user.RTL_STAT;
      }

      private void Reverse_Btn_Click(object sender, EventArgs e)
      {
         // Swap translation mode
         string from = (string)this._comboFrom.SelectedItem;
         string to = (string)this._comboTo.SelectedItem;
         this._comboFrom.SelectedItem = to;
         this._comboTo.SelectedItem = from;

         // Reset text
         this._editSourceText.Text = this._editTarget.Text;
         this._editTarget.Text = string.Empty;
         this.Update();
         this._translationSpeakUrl = string.Empty;
      }

      #region Fields

      /// <summary>
      /// The URL used to speak the translation.
      /// </summary>
      private string _translationSpeakUrl;

      #endregion

      /// <summary>
      /// Handles the Click event of the _btnTranslate control.
      /// </summary>
      /// <param name="sender">The source of the event.</param>
      /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
      private void _btnTranslate_Click(object sender, EventArgs e)
      {
         // Initialize the translator
         Translator t = new Translator();

         this._editTarget.Text = string.Empty;
         this._editTarget.Update();
         this._translationSpeakUrl = null;

         // Translate the text
         try
         {
            this.Cursor = Cursors.WaitCursor;
            this._lblStatus.Text = "Translating...";
            this._lblStatus.Update();
            this._editTarget.Text = t.Translate(this._editSourceText.Text.Trim(), (string)this._comboFrom.SelectedItem, (string)this._comboTo.SelectedItem);
            if (t.Error == null)
            {
               this._editTarget.Update();
               this._translationSpeakUrl = t.TranslationSpeechUrl;
            }
            else
            {
               MessageBox.Show(t.Error.Message, this.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }

         }
         catch (Exception ex)
         {
            MessageBox.Show(ex.Message, this.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
         }
         finally
         {
            this._lblStatus.Text = string.Format("Translated in {0} mSec", (int)t.TranslationTime.TotalMilliseconds);
            this.Cursor = Cursors.Default;
         }
      }

      /// <summary>
      /// Handles the Click event of the _btnSpeak control.
      /// </summary>
      /// <param name="sender">The source of the event.</param>
      /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
      private void _btnSpeak_Click
          (object sender,
           EventArgs e)
      {
         if (!string.IsNullOrEmpty(this._translationSpeakUrl))
         {
            this._webBrowserCtrl.Navigate(this._translationSpeakUrl);
         }
      }
   }

   /// <summary>
   /// Translates text using Google's online language tools.
   /// </summary>
   public class Translator
   {
      #region Properties

      /// <summary>
      /// Gets the supported languages.
      /// </summary>
      public static IEnumerable<string> Languages
      {
         get
         {
            Translator.EnsureInitialized();
            return Translator._languageModeMap.Keys.OrderBy(p => p);
         }
      }

      /// <summary>
      /// Gets the time taken to perform the translation.
      /// </summary>
      public TimeSpan TranslationTime
      {
         get;
         private set;
      }

      /// <summary>
      /// Gets the url used to speak the translation.
      /// </summary>
      /// <value>The url used to speak the translation.</value>
      public string TranslationSpeechUrl
      {
         get;
         private set;
      }

      /// <summary>
      /// Gets the error.
      /// </summary>
      public Exception Error
      {
         get;
         private set;
      }

      #endregion

      #region Public methods

      /// <summary>
      /// Translates the specified source text.
      /// </summary>
      /// <param name="sourceText">The source text.</param>
      /// <param name="sourceLanguage">The source language.</param>
      /// <param name="targetLanguage">The target language.</param>
      /// <returns>The translation.</returns>
      public string Translate
          (string sourceText,
           string sourceLanguage,
           string targetLanguage)
      {
         // Initialize
         this.Error = null;
         this.TranslationSpeechUrl = null;
         this.TranslationTime = TimeSpan.Zero;
         DateTime tmStart = DateTime.Now;
         string translation = string.Empty;

         try
         {
            // Download translation
            string url = string.Format("https://translate.googleapis.com/translate_a/single?client=gtx&sl={0}&tl={1}&dt=t&q={2}",
                                        Translator.LanguageEnumToIdentifier(sourceLanguage),
                                        Translator.LanguageEnumToIdentifier(targetLanguage),
                                        HttpUtility.UrlEncode(sourceText));
            string outputFile = Path.GetTempFileName();
            using (WebClient wc = new WebClient())
            {
               wc.Headers.Add("user-agent", "Mozilla/5.0 (Windows NT 6.1) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/41.0.2228.0 Safari/537.36");
               wc.DownloadFile(url, outputFile);
            }

            // Get translated text
            if (File.Exists(outputFile))
            {

               // Get phrase collection
               string text = File.ReadAllText(outputFile);
               int index = text.IndexOf(string.Format(",,\"{0}\"", Translator.LanguageEnumToIdentifier(sourceLanguage)));
               if (index == -1)
               {
                  // Translation of single word
                  int startQuote = text.IndexOf('\"');
                  if (startQuote != -1)
                  {
                     int endQuote = text.IndexOf('\"', startQuote + 1);
                     if (endQuote != -1)
                     {
                        translation = text.Substring(startQuote + 1, endQuote - startQuote - 1);
                     }
                  }
               }
               else
               {
                  // Translation of phrase
                  text = text.Substring(0, index);
                  text = text.Replace("],[", ",");
                  text = text.Replace("]", string.Empty);
                  text = text.Replace("[", string.Empty);
                  text = text.Replace("\",\"", "\"");

                  // Get translated phrases
                  string[] phrases = text.Split(new[] { '\"' }, StringSplitOptions.RemoveEmptyEntries);
                  for (int i = 0; (i < phrases.Count()); i += 2)
                  {
                     string translatedPhrase = phrases[i];
                     if (translatedPhrase.StartsWith(",,"))
                     {
                        i--;
                        continue;
                     }
                     translation += translatedPhrase + "  ";
                  }
               }

               // Fix up translation
               translation = translation.Trim();
               translation = translation.Replace(" ?", "?");
               translation = translation.Replace(" !", "!");
               translation = translation.Replace(" ,", ",");
               translation = translation.Replace(" .", ".");
               translation = translation.Replace(" ;", ";");

               // And translation speech URL
               this.TranslationSpeechUrl = string.Format("https://translate.googleapis.com/translate_tts?ie=UTF-8&q={0}&tl={1}&total=1&idx=0&textlen={2}&client=gtx",
                                                          HttpUtility.UrlEncode(translation), Translator.LanguageEnumToIdentifier(targetLanguage), translation.Length);
            }
         }
         catch (Exception ex)
         {
            this.Error = ex;
         }

         // Return result
         this.TranslationTime = DateTime.Now - tmStart;
         return translation;
      }

      #endregion

      #region Private methods

      /// <summary>
      /// Converts a language to its identifier.
      /// </summary>
      /// <param name="language">The language."</param>
      /// <returns>The identifier or <see cref="string.Empty"/> if none.</returns>
      private static string LanguageEnumToIdentifier
          (string language)
      {
         string mode = string.Empty;
         Translator.EnsureInitialized();
         Translator._languageModeMap.TryGetValue(language, out mode);
         return mode;
      }

      /// <summary>
      /// Ensures the translator has been initialized.
      /// </summary>
      private static void EnsureInitialized()
      {
         if (Translator._languageModeMap == null)
         {
            Translator._languageModeMap = new Dictionary<string, string>();
            Translator._languageModeMap.Add("Afrikaans", "af");
            Translator._languageModeMap.Add("Albanian", "sq");
            Translator._languageModeMap.Add("Arabic", "ar");
            Translator._languageModeMap.Add("Armenian", "hy");
            Translator._languageModeMap.Add("Azerbaijani", "az");
            Translator._languageModeMap.Add("Basque", "eu");
            Translator._languageModeMap.Add("Belarusian", "be");
            Translator._languageModeMap.Add("Bengali", "bn");
            Translator._languageModeMap.Add("Bulgarian", "bg");
            Translator._languageModeMap.Add("Catalan", "ca");
            Translator._languageModeMap.Add("Chinese", "zh-CN");
            Translator._languageModeMap.Add("Croatian", "hr");
            Translator._languageModeMap.Add("Czech", "cs");
            Translator._languageModeMap.Add("Danish", "da");
            Translator._languageModeMap.Add("Dutch", "nl");
            Translator._languageModeMap.Add("English", "en");
            Translator._languageModeMap.Add("Esperanto", "eo");
            Translator._languageModeMap.Add("Estonian", "et");
            Translator._languageModeMap.Add("Filipino", "tl");
            Translator._languageModeMap.Add("Finnish", "fi");
            Translator._languageModeMap.Add("French", "fr");
            Translator._languageModeMap.Add("Galician", "gl");
            Translator._languageModeMap.Add("German", "de");
            Translator._languageModeMap.Add("Georgian", "ka");
            Translator._languageModeMap.Add("Greek", "el");
            Translator._languageModeMap.Add("Haitian Creole", "ht");
            Translator._languageModeMap.Add("Hebrew", "iw");
            Translator._languageModeMap.Add("Hindi", "hi");
            Translator._languageModeMap.Add("Hungarian", "hu");
            Translator._languageModeMap.Add("Icelandic", "is");
            Translator._languageModeMap.Add("Indonesian", "id");
            Translator._languageModeMap.Add("Irish", "ga");
            Translator._languageModeMap.Add("Italian", "it");
            Translator._languageModeMap.Add("Japanese", "ja");
            Translator._languageModeMap.Add("Korean", "ko");
            Translator._languageModeMap.Add("Lao", "lo");
            Translator._languageModeMap.Add("Latin", "la");
            Translator._languageModeMap.Add("Latvian", "lv");
            Translator._languageModeMap.Add("Lithuanian", "lt");
            Translator._languageModeMap.Add("Macedonian", "mk");
            Translator._languageModeMap.Add("Malay", "ms");
            Translator._languageModeMap.Add("Maltese", "mt");
            Translator._languageModeMap.Add("Norwegian", "no");
            Translator._languageModeMap.Add("Persian", "fa");
            Translator._languageModeMap.Add("Polish", "pl");
            Translator._languageModeMap.Add("Portuguese", "pt");
            Translator._languageModeMap.Add("Romanian", "ro");
            Translator._languageModeMap.Add("Russian", "ru");
            Translator._languageModeMap.Add("Serbian", "sr");
            Translator._languageModeMap.Add("Slovak", "sk");
            Translator._languageModeMap.Add("Slovenian", "sl");
            Translator._languageModeMap.Add("Spanish", "es");
            Translator._languageModeMap.Add("Swahili", "sw");
            Translator._languageModeMap.Add("Swedish", "sv");
            Translator._languageModeMap.Add("Tamil", "ta");
            Translator._languageModeMap.Add("Telugu", "te");
            Translator._languageModeMap.Add("Thai", "th");
            Translator._languageModeMap.Add("Turkish", "tr");
            Translator._languageModeMap.Add("Ukrainian", "uk");
            Translator._languageModeMap.Add("Urdu", "ur");
            Translator._languageModeMap.Add("Vietnamese", "vi");
            Translator._languageModeMap.Add("Welsh", "cy");
            Translator._languageModeMap.Add("Yiddish", "yi");
         }
      }

      #endregion

      #region Fields

      /// <summary>
      /// The language to translation mode map.
      /// </summary>
      private static Dictionary<string, string> _languageModeMap;

      #endregion
   }
}
