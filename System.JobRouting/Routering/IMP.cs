using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

using System.JobRouting.Jobs;

namespace System.JobRouting.Routering
{
    public abstract class IMP : IRouter
    {
        public void Gateway(Job root)
        {
            root.Status = Jobs.StatusType.Waiting;
            int index = -1;
            do
            {
                var current = (from job in root.OwnerDefineWorkWith
                               where job.Status == Jobs.StatusType.InQueue
                               select job).Take(1).ElementAt(0);

                current.Back = root;

                switch (current.WhereIsInputData)
                {
                    case Jobs.WhereIsInputDataType.StepBack:
                        current.Input = root.Input;
                        //current.WhereIsInputData = Jobs.WhereIsInputDataType.Here;
                        break;
                }

                if (index >= 0)
                {
                    var backstep = root.OwnerDefineWorkWith[index];
                    if (backstep.Status == Jobs.StatusType.Failed && backstep.FaultTolerance == Jobs.FaultToleranceType.Abort)
                    {
                        root.Status = Jobs.StatusType.Failed;
                        return;
                    }
                }
                ++index;

                Manager(current);

                if (root.Status == Jobs.StatusType.Failed)
                    return;

                if (root.GenerateInputData == Jobs.GenerateDataType.Dynamic)
                    root.Input = current.Output;

            } while ((from job in root.OwnerDefineWorkWith where job.Status == Jobs.StatusType.InQueue select job).Count() > 0);

            root.Output = root.OwnerDefineWorkWith.Last().Output;
        }

        protected void Manager(Job jobs)
        {
            var current = jobs;
            var parent = jobs.Back;
            Thread worker;

            do
            {
                //current.Status = Jobs.StatusType.Waiting;
                switch (current.WhereIsInputData)
                {
                    case Jobs.WhereIsInputDataType.StepBack:
                        current.GetInputData();
                        break;
                }

                switch (current.To)
                {
                    case Jobs.SendType.Self:
                        current.Status = Jobs.StatusType.Running;
                        do
                        {

                            switch (current.Executive)
                            {
                                case ExecutiveType.Synchronize:
                                    InternalAssistance(current);
                                    break;
                                case ExecutiveType.Asynchronous:
                                    worker = new Thread(InternalAssistance);
                                    worker.Start(current);
                                    break;
                            }


                            switch (current.Status)
                            {
                                case Jobs.StatusType.Failed:
                                    if (current.FaultTolerance == Jobs.FaultToleranceType.Abort)
                                    {
                                        if (parent != null)
                                            parent.HowToCrashJob(current);
                                        //parent.Status = Jobs.StatusType.Failed;                                        
                                        return;
                                    }
                                    break;
                                case Jobs.StatusType.WaitForPreconditions:
                                    Gateway(current);
                                    switch (current.Status)
                                    {
                                        case Jobs.StatusType.Failed:
                                            if (current.FaultTolerance == Jobs.FaultToleranceType.Abort)
                                            {
                                                if (parent != null)
                                                    parent.HowToCrashJob(current);
                                                //parent.Status = Jobs.StatusType.Failed;
                                                return;
                                            }
                                            current.Status = Jobs.StatusType.SignalForPreconditions;
                                            break;
                                        case Jobs.StatusType.Successful:
                                            current.Status = Jobs.StatusType.SignalForPreconditions;
                                            break;
                                    }
                                    break;
                                case Jobs.StatusType.WaitForPostconditions:
                                    Gateway(current);
                                    switch (current.Status)
                                    {
                                        case Jobs.StatusType.Failed:
                                            if (current.FaultTolerance == Jobs.FaultToleranceType.Abort)
                                            {
                                                if (parent != null)
                                                    parent.HowToCrashJob(current);
                                                //parent.Status = Jobs.StatusType.Failed;
                                                return;
                                            }
                                            break;
                                    }
                                    break;
                            }
                            // # If status is equals by Successful, then loop is down.
                        } while (current.Status == Jobs.StatusType.SignalForPreconditions);
                        break;
                    case Jobs.SendType.SelfToUserInterface:
                        current.Status = Jobs.StatusType.Running;

                        switch (current.Executive)
                        {
                            case ExecutiveType.Synchronize:
                                RequestToUserInterface(current);
                                break;
                            case ExecutiveType.Asynchronous:
                                worker = new Thread(RequestToUserInterface);
                                worker.Start(current);
                                break;
                        }


                        switch (current.Status)
                        {
                            case Jobs.StatusType.Failed:
                                if (current.FaultTolerance == Jobs.FaultToleranceType.Abort)
                                {
                                    if (parent != null)
                                        parent.HowToCrashJob(current);
                                    //parent.Status = Jobs.StatusType.Failed;
                                    return;
                                }
                                break;
                        }
                        break;
                    case Jobs.SendType.Colapse:
                        current.Output = current.Input;
                        Manager(current.Next);

                        switch (current.Status)
                        {
                            case Jobs.StatusType.Failed:
                                if (current.FaultTolerance == Jobs.FaultToleranceType.Abort)
                                {
                                    parent.HowToCrashJob(current);
                                    //parent.Status = Jobs.StatusType.Failed;
                                    return;
                                }
                                break;
                        }
                        //if (parent != null)
                        parent.Status = Jobs.StatusType.Successful;
                        return;
                    case Jobs.SendType.External:
                        switch (current.Executive)
                        {
                            case ExecutiveType.Synchronize:
                                ExternalAssistance(current);
                                break;
                            case ExecutiveType.Asynchronous:
                                current.Status = StatusType.EnteringToGateway;
                                worker = new Thread(ExternalAssistance);
                                worker.Start(current);
                                break;
                        }
                        

                        switch (current.Status)
                        {
                            case Jobs.StatusType.Failed:
                                if (current.FaultTolerance == Jobs.FaultToleranceType.Abort)
                                {
                                    if (parent != null)
                                        parent.HowToCrashJob(current);
                                    //parent.Status = Jobs.StatusType.Failed;
                                    return;
                                }
                                break;
                        }
                        break;
                }

                if (parent != null && parent.To == Jobs.SendType.Colapse && current.Next == null)
                {
                    parent.Output = current.Output;
                    parent.Status = Jobs.StatusType.Successful;
                    return;
                }

                current = current.Next;

            } while (current != null);
            if (parent != null)
                parent.Status = Jobs.StatusType.Successful;
        }

        protected abstract void ExternalAssistance(Job jobs);
        private void ExternalAssistance(object job)
        {
            ExternalAssistance(job as Job);
        }

        protected abstract void InternalAssistance(Job job);
        private void InternalAssistance(object job)
        {
            InternalAssistance(job as Job);
        }

        protected abstract void RequestToUserInterface(Job job);
        private void RequestToUserInterface(object job)
        {
            RequestToUserInterface(job as Job);
        }
    }
}
