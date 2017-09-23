using System;
using System.Collections.Generic;
using System.JobRouting.Jobs;
using System.JobRouting.Routering;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using GMap.NET;
using GMap.NET.WindowsForms;
using GMap.NET.WindowsForms.Markers;
using GMap.NET.MapProviders;

namespace MyProject.Commons.Ui
{
   partial class GMapNets : ISendRequest
   {
      public IRouter _DefaultGateway { get; set; }

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
            case 05:
            case 06:
               break;
            case 07:
               LoadData(job);
               break;
            case 10:
               ActionCallForm(job);
               break;
            default:
               break;
         }
      }

      /// <summary>
      /// Code 00
      /// </summary>
      /// <param name="job"></param>
      private void ProcessCmdKey(Job job)
      {
         Keys keyData = (Keys)job.Input;

         if (keyData == Keys.F1)
         {
            #region Key.F1
            job.Next =
               new Job(SendType.External, "Commons",
                  new List<Job>
                  {
                     new Job(SendType.Self, 03 /* Execute DoWork4HelpHandling */)
                     {
                        Input = @"<HTML>
                                    <body>
                                       <p style=""float:right"">
                                             <ol>
                                                <li><font face=""verdana"" size=""3"" color=""red"">F10</font></li>
                                                <ul>
                                                   <li><font face=""Tahoma"" size=""3"" color=""green"">خروج از سیستم</font></li>
                                                </ul>
                                                <li><font face=""verdana"" size=""3"" color=""red"">F9</font></li>
                                                <ul>
                                                   <li><font face=""Tahoma"" size=""3"" color=""green"">خروج از محیط کاربری</font></li>
                                                </ul>
                                             </ol>
                                       </p>
                                    </body>
                                    </HTML>"
                     }
                  });
            #endregion
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
      /// <param name="job"></param>
      private void Get(Job job)
      {
         job.Status = StatusType.Successful;
      }

      /// <summary>
      /// Code 02
      /// </summary>
      /// <param name="job"></param>
      private void Set(Job job)
      {
         Enabled = true;
         
         job.Status = StatusType.Successful;
      }

      /// <summary>
      /// Code 03
      /// </summary>
      /// <param name="job"></param>
      private new void Paint(Job job)
      {
         Job _Paint = new Job(SendType.External, "Desktop",
            new List<Job>
            {
               new Job(SendType.SelfToUserInterface, "Wall", 17 /* Execute ResetUi */),
               new Job(SendType.SelfToUserInterface, "Wall", 15 /* Execute Push */) {  Input = new List<object> { "Commons:" + GetType().Name, this }  },
               new Job(SendType.SelfToUserInterface, "Wall", 01 /* Execute PastManualOnWall */) {  Input = /*new List<object> {this, "cntrhrz:default"}*/ this }
            });
         _DefaultGateway.Gateway(_Paint);

         job.Status = StatusType.Successful;
      }

      /// <summary>
      /// Code 04
      /// </summary>
      /// <param name="job"></param>
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
      /// <param name="job"></param>
      private void LoadData(Job job)
      {
         button15_Click(null, null);
         button6_Click(null, null);
         button5_Click(null, null);
         checkBoxDebug.Checked = false;
         comboBoxMapType.SelectedValue = "GoogleMap";
         comboBoxMapType_DropDownClosed(null, null);
         xinput = job.Input as XElement;
         if(xinput != null)
         {
            if(Convert.ToBoolean(xinput.Attribute("initalset").Value))
            {
               textBoxLat.Text = xinput.Attribute("cordx").Value;
               textBoxLng.Text = xinput.Attribute("cordy").Value;
               trackBar1.Value = Convert.ToInt32(xinput.Attribute("zoom").Value);
               MainMap.Zoom = trackBar1.Value / 100;
               CordFind_Rb.Checked = true;
               button8_Click(null, null);
               Refresh();               
            }
            SubmitChange_Butn.Visible = false;
            
            if(MainMap.Overlays.Count > 4)
               MainMap.Overlays.Remove(MainMap.Overlays.Where(o => o.Id == "markers").First());

            switch(xinput.Attribute("requesttype").Value)
            {
               case "get":
                  SubmitChange_Butn.Visible = true;
                  break;
               case "queryonly":                  
                  break;
               case "showmarks":
                  #region Create and show marks                  
                  MainMap.SetPositionByKeywords("Iran");
                  //MainMap.ShowCenter = true;
 
                  GMapOverlay markers = new GMapOverlay("markers");

                  xinput.Descendants("Service").Where(sx => sx.Attribute("cordx").Value != "0" && sx.Attribute("cordy").Value != "0").ToList().ForEach(
                     sx=>
                     {
                        GMapMarker marker = new GMarkerGoogle(
                         new PointLatLng(Convert.ToDouble(sx.Attribute("cordx").Value), Convert.ToDouble(sx.Attribute("cordy").Value)),
                         GMarkerGoogleType.red_dot);

                        marker.ToolTipText = sx.Attribute("namednrm").Value;
                        marker.Tag = sx.Attribute("fileno").Value;
                        marker.ToolTipMode = MarkerTooltipMode.OnMouseOver;
                        
                        markers.Markers.Add(marker);
                     }
                  );
                  
                  MainMap.Overlays.Add(markers);
                  #endregion
                  break;
               case "showrelationmarks":
                  #region Create and show marks                  
                  MainMap.SetPositionByKeywords("Iran");
                  //MainMap.ShowCenter = true;
 
                  GMapOverlay relationsmarkers = new GMapOverlay("markers");

                  // Set Service Source Point
                  xinput.Descendants("Source").Where(sx => sx.Attribute("type").Value == "001").Where(sx => sx.Attribute("cordx").Value != "0" && sx.Attribute("cordy").Value != "0").ToList().ForEach(
                     sx =>
                     {
                        GMapMarker marker = new GMarkerGoogle(
                         new PointLatLng(Convert.ToDouble(sx.Attribute("cordx").Value), Convert.ToDouble(sx.Attribute("cordy").Value)),
                         GMarkerGoogleType.blue_dot);

                        marker.ToolTipText = sx.Attribute("name").Value + "\n\r" + sx.Attribute("postadrs").Value;
                        marker.Tag = sx.Attribute("code").Value;
                        marker.ToolTipMode = MarkerTooltipMode.OnMouseOver;

                        relationsmarkers.Markers.Add(marker);
                     }
                  );

                  // Set Company Source Point
                  xinput.Descendants("Source").Where(sx => sx.Attribute("type").Value == "002").Where(sx => (sx.Attribute("billcordx").Value != "0" && sx.Attribute("billcordy").Value != "0") || (sx.Attribute("shipcordx").Value != "0" && sx.Attribute("shipcordy").Value != "0")).ToList().ForEach(
                     sx =>
                     {
                        if (sx.Attribute("billcordx").Value != "0")
                        {
                           GMapMarker marker = new GMarkerGoogle(
                            new PointLatLng(Convert.ToDouble(sx.Attribute("billcordx").Value), Convert.ToDouble(sx.Attribute("billcordy").Value)),
                            GMarkerGoogleType.blue_dot);

                           marker.ToolTipText = sx.Attribute("name").Value + "\n\r" + sx.Attribute("postadrs").Value;
                           marker.Tag = sx.Attribute("code").Value;
                           marker.ToolTipMode = MarkerTooltipMode.OnMouseOver;

                           relationsmarkers.Markers.Add(marker);
                        }
                        if (sx.Attribute("shipcordx").Value != "0")
                        {
                           GMapMarker marker = new GMarkerGoogle(
                            new PointLatLng(Convert.ToDouble(sx.Attribute("shipcordx").Value), Convert.ToDouble(sx.Attribute("shipcordy").Value)),
                            GMarkerGoogleType.blue_dot);

                           marker.ToolTipText = sx.Attribute("name").Value + "\n\r" + sx.Attribute("postadrs").Value;
                           marker.Tag = sx.Attribute("code").Value;
                           marker.ToolTipMode = MarkerTooltipMode.OnMouseOver;

                           relationsmarkers.Markers.Add(marker);
                        }
                     }
                  );

                  // Service Relation
                  xinput.Descendants("Relation").Where(sx => sx.Attribute("type").Value == "001").Where(sx => sx.Attribute("cordx").Value != "0" && sx.Attribute("cordy").Value != "0").ToList().ForEach(
                     sx=>
                     {
                        GMapMarker marker = new GMarkerGoogle(
                         new PointLatLng(Convert.ToDouble(sx.Attribute("cordx").Value), Convert.ToDouble(sx.Attribute("cordy").Value)),
                         GMarkerGoogleType.red_dot);

                        marker.ToolTipText = sx.Attribute("name").Value + "\n\r" + sx.Attribute("postadrs").Value;
                        marker.Tag = sx.Attribute("code").Value;
                        marker.ToolTipMode = MarkerTooltipMode.OnMouseOver;

                        relationsmarkers.Markers.Add(marker);
                     }
                  );

                  // Company Relation
                  xinput.Descendants("Relation").Where(sx => sx.Attribute("type").Value == "002").Where(sx => (sx.Attribute("billcordx").Value != "0" && sx.Attribute("billcordy").Value != "0") || (sx.Attribute("shipcordx").Value != "0" && sx.Attribute("shipcordy").Value != "0")).ToList().ForEach(
                     sx =>
                     {
                        if (sx.Attribute("billcordx").Value != "0")
                        {
                           GMapMarker marker = new GMarkerGoogle(
                            new PointLatLng(Convert.ToDouble(sx.Attribute("billcordx").Value), Convert.ToDouble(sx.Attribute("billcordy").Value)),
                            GMarkerGoogleType.green_dot);

                           marker.ToolTipText = sx.Attribute("name").Value + "\n\r" + sx.Attribute("postadrs").Value;
                           marker.Tag = sx.Attribute("code").Value;
                           marker.ToolTipMode = MarkerTooltipMode.OnMouseOver;
                           
                           relationsmarkers.Markers.Add(marker);
                        }
                        if (sx.Attribute("shipcordx").Value != "0")
                        {
                           GMapMarker marker = new GMarkerGoogle(
                            new PointLatLng(Convert.ToDouble(sx.Attribute("shipcordx").Value), Convert.ToDouble(sx.Attribute("shipcordy").Value)),
                            GMarkerGoogleType.green_dot);

                           marker.ToolTipText = sx.Attribute("name").Value + "\n\r" + sx.Attribute("postadrs").Value;
                           marker.Tag = sx.Attribute("code").Value;
                           marker.ToolTipMode = MarkerTooltipMode.OnMouseOver;

                           relationsmarkers.Markers.Add(marker);
                        }
                        
                     }
                  );

                  MainMap.Overlays.Add(relationsmarkers);
                  #endregion
                  break;
            }
         }
         job.Status = StatusType.Successful;
      }

      /// <summary>
      /// Code 10
      /// </summary>
      /// <param name="job"></param>
      private void ActionCallForm(Job job)
      {
         xinput = job.Input as XElement;
         if(xinput != null)
         {
            switch (xinput.Attribute("requesttype").Value)
            {
               case "GetXYFromAddress":
                  GeoCoderStatusCode status = GeoCoderStatusCode.Unknow;
                  PointLatLng? pos = 
                     GMapProviders.GoogleMap.GetPoint(
                        xinput.Element("Address").Attribute("cnty").Value +
                        xinput.Element("Address").Attribute("prvn").Value +
                        xinput.Element("Address").Attribute("regn").Value +
                        xinput.Element("Address").Attribute("value").Value, 
                        out status);
                  if(pos != null && status == GeoCoderStatusCode.G_GEO_SUCCESS)
                  {
                     job.Output =
                        new XElement("Address",
                           new XAttribute("cordx", pos.Value.Lat),
                           new XAttribute("cordy", pos.Value.Lng)
                        );
                  }
                  break;
               default:
                  break;
            }
         }
         job.Status = StatusType.Successful;
      }
   }
}
