using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.JobRouting.Jobs;

namespace System.JobRouting.Routering
{
   public interface IDefaultMethods
   {
      /// <summary>
      /// Code 01
      /// </summary>
      /// <param name="job"></param>
      void ProcessCmdKey(Job job);

      /// <summary>
      /// Code 02
      /// </summary>
      /// <param name="job"></param>
      void LoadData(Job job);

      /// <summary>
      /// Code 03
      /// </summary>
      /// <param name="job"></param>
      void SetAction(Job job);

      /// <summary>
      /// Code 04
      /// </summary>
      /// <param name="job"></param>
      void Paint(Job job);

      /// <summary>
      /// Code 05
      /// </summary>
      /// <param name="job"></param>
      void UnPaint(Job job);
   }
}
