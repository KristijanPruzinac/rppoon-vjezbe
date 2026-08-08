using System;
using System.Collections.Generic;

namespace Rppoon.Zadaci.Promatrac.B2
{
    // ============ REFERENTNO RJESENJE - Promatrac B2 ============

    public interface IFlowObserver
    {
        void OnFlowChanged(string sensorName, double flow);
    }

    public interface IFlowSubject
    {
        void Subscribe(IFlowObserver observer);
        void Unsubscribe(IFlowObserver observer);
        void Notify();
    }

    public class FlowSensor : IFlowSubject
    {
        private readonly List<IFlowObserver> observers = new List<IFlowObserver>();

        public FlowSensor(string name)
        {
            this.Name = name;
        }

        public string Name { get; private set; }

        public double Flow { get; private set; }

        public void SetFlow(double flow)
        {
            this.Flow = flow;
            this.Notify();
        }

        public void Subscribe(IFlowObserver observer)
        {
            this.observers.Add(observer);
        }

        public void Unsubscribe(IFlowObserver observer)
        {
            this.observers.Remove(observer);
        }

        public void Notify()
        {
            foreach (IFlowObserver observer in this.observers)
            {
                observer.OnFlowChanged(this.Name, this.Flow);
            }
        }
    }

    public class MailNotifier : IFlowObserver
    {
        public string LastMessage { get; private set; }

        public void OnFlowChanged(string sensorName, double flow)
        {
            this.LastMessage = sensorName + ": " + flow;
        }
    }

    public class FlowRecorder : IFlowObserver
    {
        public int Received { get; private set; }

        public void OnFlowChanged(string sensorName, double flow)
        {
            this.Received = this.Received + 1;
        }
    }
}
