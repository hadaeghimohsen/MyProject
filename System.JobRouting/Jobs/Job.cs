using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;


namespace System.JobRouting.Jobs
{
   [Serializable]
   public class Job : ISerializable
   {
      #region Constructor
      private Job()
      {
         InputTriggerActive = OutputTriggerActive = StatusTriggerActive = true;
         FaultTolerance = FaultToleranceType.Abort;
         Executive = ExecutiveType.Synchronize;
         Status = StatusType.InQueue;
         WhereIsInputData = WhereIsInputDataType.None;
         Input = null;
         RunProcess = RunProcessType.Local;
         GenerateInputData = GenerateDataType.Static;
      }

      public Job(SendType to, string gateway)
         : this()
      {
         To = to;
         Gateway = gateway;
         OwnerDefineWorkWith = new List<Job>();
      }

      public Job(SendType to, int method)
         : this()
      {
         To = to;
         Method = method;
         OwnerDefineWorkWith = new List<Job>();
      }

      public Job(SendType to, string gateway, int method)
         : this()
      {
         To = to;
         Gateway = gateway;
         Method = method;
         OwnerDefineWorkWith = new List<Job>();
      }

      public Job(SendType to, string gateway, Job next)
         : this()
      {
         To = to;
         Gateway = gateway;
         Next = next;
         Next.Back = this;
         OwnerDefineWorkWith = new List<Job>();
      }

      public Job(SendType to, int method, Job next)
         : this()
      {
         To = to;
         Method = method;
         Next = next;
         Next.Back = this;
         OwnerDefineWorkWith = new List<Job>();
      }

      public Job(SendType to, string gateway, int method, Job next)
         : this()
      {
         To = to;
         Method = method;
         Gateway = gateway;
         Next = next;
         Next.Back = this;
         OwnerDefineWorkWith = new List<Job>();
      }

      public Job(SendType to, string gateway, List<Job> ownerdefineworkwith)
         : this()
      {
         To = to;
         Gateway = gateway;
         OwnerDefineWorkWith = ownerdefineworkwith;
      }

      public Job(SendType to, string gateway, List<Job> ownerdefineworkwith, Job next)
         : this()
      {
         To = to;
         Gateway = gateway;
         OwnerDefineWorkWith = ownerdefineworkwith;
         Next = next;
         Next.Back = this;
      }

      public Job(SendType to, int method, List<Job> ownerdefineworkwith)
         : this()
      {
         To = to;
         Method = method;
         OwnerDefineWorkWith = ownerdefineworkwith;
      }

      public Job(SendType to, int method, List<Job> ownerdefineworkwith, Job next)
         : this()
      {
         To = to;
         Method = method;
         OwnerDefineWorkWith = ownerdefineworkwith;
         Next = next;
         Next.Back = this;
      }

      public Job(SendType to, string gateway, string gatewaysPath, int method, SendType lastNode)
         : this()
      {
         To = to;
         Gateway = gateway;
         OwnerDefineWorkWith = new List<Job>();
         if (gatewaysPath != "")
            CreateOwnerJobWithPath(gatewaysPath, method, lastNode);
         else
            OwnerDefineWorkWith.Add(new Jobs.Job(SendType.Self, method) { WhereIsInputData = WhereIsInputDataType.StepBack });
      }

      #endregion

      #region Private
      private Job next;
      private object input;
      private object output;
      private StatusType status;
      #endregion

      #region Properties
      public SendType To { get; set; }
      public string Gateway { get; set; }
      public int Method { get; set; }
      public Job Next { get { return next; } set { next = value; if (value != null) value.Back = this; } }
      public Job Back { get; set; }
      public List<Job> OwnerDefineWorkWith { get; set; }
      public string Message { get; set; }
      public ErrorType Error { get; set; }
      public StatusType Status
      {
         get { return status; }
         set
         {
            if (BeforeChangingStatus != null && StatusTriggerActive)
               BeforeChangingStatus(status);

            status = value;

            if (AfterChangedStatus != null && StatusTriggerActive)
               AfterChangedStatus(status);
         }
      }
      public FaultToleranceType FaultTolerance { get; set; }
      public ExecutiveType Executive { get; set; }
      public WhereIsInputDataType WhereIsInputData { get; set; }
      public GenerateDataType GenerateInputData { get; set; }
      public object Input
      {
         get { return input; }
         set
         {
            if (value != null)
            {
               // Create Triger Before Changing Input Value
               if (BeforeChangingInput != null && InputTriggerActive)
                  BeforeChangingInput(input);

               if (UpdateBeforeChangingInput != null && InputTriggerActive)
                  value = UpdateBeforeChangingInput(value);

               input = value;
               WhereIsInputData = WhereIsInputDataType.Here;

               // Create Triger After Changed Input Value
               if (UpdateAfterChangedInput != null && InputTriggerActive)
                  input = UpdateAfterChangedInput(value);

               if (AfterChangedInput != null && InputTriggerActive)
                  AfterChangedInput(input);
            }
         }
      }
      public object Output
      {
         get { return output; }
         set
         {
            if (value != null)
            {
               // Create Triger Before Changing Output Value
               if (BeforeChangingOutput != null && OutputTriggerActive)
                  BeforeChangingOutput(output);

               if (UpdateBeforeChangingOupput != null && OutputTriggerActive)
                  value = UpdateBeforeChangingOupput(value);

               output = value;

               // Create Triger After Changed Output Value
               if (UpdateAfterChangedOutput != null && OutputTriggerActive)
                  output = UpdateAfterChangedOutput(value);

               if (AfterChangedOutput != null && OutputTriggerActive)
                  AfterChangedOutput(output);              
            }
         }
      }
      public RunProcessType RunProcess { get; set; }
      public RPCJobType RPCJob { get; set; }

      public Action<Object> BeforeChangingInput { get; set; }
      public Action<Object> AfterChangedInput { get; set; }
      public Func<Object, Object> UpdateBeforeChangingInput { get; set; }
      public Func<Object, Object> UpdateAfterChangedInput { get; set; }

      public Action<Object> BeforeChangingOutput { get; set; }
      public Action<Object> AfterChangedOutput { get; set; }
      public Func<Object, Object> UpdateBeforeChangingOupput { get; set; }
      public Func<Object, Object> UpdateAfterChangedOutput { get; set; }

      public Action<StatusType> BeforeChangingStatus { get; set; }
      public Action<StatusType> AfterChangedStatus { get; set; }

      public bool InputTriggerActive { get; set; }
      public bool OutputTriggerActive { get; set; }
      public bool StatusTriggerActive { get; set; }
      #endregion

      #region Method
      private void CreateOwnerJobWithPath(string gatewaysPath, int method, SendType lastNode)
      {
         // ownerjobpath sample "Layer#:Layer#" , "Layer#"
         if (gatewaysPath.IndexOf(':') != -1)
            OwnerDefineWorkWith.Add(
                new Jobs.Job(
                                SendType.External,
                                gatewaysPath.Split(':')[0],
                                gatewaysPath.Substring(gatewaysPath.IndexOf(':') + 1),
                                method,
                                lastNode
                            ) { WhereIsInputData = WhereIsInputDataType.StepBack }
                                   );
         else
         {
            if (lastNode == SendType.Self)
               OwnerDefineWorkWith.Add(
                   new Jobs.Job(
                                   SendType.External,
                                   gatewaysPath,
                                   new List<Job>
                                        {
                                            new Jobs.Job(SendType.Self , method){WhereIsInputData = WhereIsInputDataType.StepBack}
                                        }
                               ) { WhereIsInputData = WhereIsInputDataType.StepBack });
            else if (lastNode == SendType.SelfToUserInterface)
               OwnerDefineWorkWith.Add(
                      new Jobs.Job(
                                      SendType.SelfToUserInterface,
                                      gatewaysPath,
                                      method
                                  ) { WhereIsInputData = WhereIsInputDataType.StepBack });
         }

      }
      public void GetInputData()
      {
         if (Back != null)
         {
            Input = Back.Output;
            //WhereIsInputData = WhereIsInputDataType.Here;
         }
      }
      public void AppendJobInHere(Job jobs)
      {
      }
      public void AppendJobToLastOwner(Job job)
      {
         var owners = OwnerDefineWorkWith;
         while (owners.Count != 0)
         {
            owners = owners[0].OwnerDefineWorkWith;
         }
         owners.Add(job);
      }
      public void HowToCrashJob(Job current)
      {
         Error = current.Error;
         Message = current.Message;
         Status = current.Status;
      }
      #endregion

      #region Serialization
      public Job(SerializationInfo info, StreamingContext context)
      {
         typeof(Job).GetProperties().ToList().ForEach(pro => pro.SetValue(this, info.GetValue(pro.Name, pro.PropertyType)));
      }

      public void GetObjectData(SerializationInfo info, StreamingContext context)
      {
         typeof(Job).GetProperties().ToList().ForEach(pro => info.AddValue(pro.Name, pro.GetValue(this, null), pro.PropertyType));
      }
      #endregion
   }
}