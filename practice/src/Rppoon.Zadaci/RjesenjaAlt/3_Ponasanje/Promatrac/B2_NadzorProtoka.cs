using System;
using System.Collections.Generic;

namespace Rppoon.Zadaci.Promatrac.B2
{
    // ===== ALTERNATIVNO RJESENJE - Promatrac B2 =====
    //
    // Ime i protok u izricitim poljima, poruka slozena interpolacijom,
    // pretplatnici u polju koje se obilazi indeksom.

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
        private readonly string name;
        private readonly List<IFlowObserver> pretplatnici = new List<IFlowObserver>();
        private double flow;

        public FlowSensor(string name)
        {
            this.name = name;
        }

        public string Name
        {
            get { return this.name; }
        }

        public double Flow
        {
            get { return this.flow; }
        }

        public void SetFlow(double flow)
        {
            this.flow = flow;
            this.Notify();
        }

        public void Subscribe(IFlowObserver observer)
        {
            if (!this.pretplatnici.Contains(observer))
            {
                this.pretplatnici.Add(observer);
            }
        }

        public void Unsubscribe(IFlowObserver observer)
        {
            this.pretplatnici.Remove(observer);
        }

        public void Notify()
        {
            for (int i = 0; i < this.pretplatnici.Count; i++)
            {
                this.pretplatnici[i].OnFlowChanged(this.name, this.flow);
            }
        }
    }

    public class MailNotifier : IFlowObserver
    {
        private string poruka;

        public string LastMessage
        {
            get { return this.poruka; }
        }

        public void OnFlowChanged(string sensorName, double flow)
        {
            this.poruka = $"{sensorName}: {flow}";
        }
    }

    public class FlowRecorder : IFlowObserver
    {
        private int primljeno;

        public int Received
        {
            get { return this.primljeno; }
        }

        public void OnFlowChanged(string sensorName, double flow)
        {
            this.primljeno++;
        }
    }
}
