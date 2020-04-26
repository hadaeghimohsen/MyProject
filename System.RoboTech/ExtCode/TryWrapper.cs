using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.RoboTech.ExtCode
{
   public class TryWrapper<T>
   {
      public TryWrapper()
      {
         this.Result = default(T);
         this.Exception = null;
      }

      protected internal T Result { get; set; }

      protected internal Exception Exception { get; set; }

      public static implicit operator T(TryWrapper<T> wrapper)
      {
         return wrapper.Result;
      }

      public static implicit operator Exception(TryWrapper<T> wrapper)
      {
         return wrapper.Exception;
      }
   }

   public class CatchWrapper<T> : TryWrapper<T>
   {
   }

}
