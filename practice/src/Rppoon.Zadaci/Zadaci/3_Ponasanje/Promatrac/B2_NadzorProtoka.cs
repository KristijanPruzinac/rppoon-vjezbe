using System;

namespace Rppoon.Zadaci.Promatrac.B2
{
    // ============================================================
    //  PROMATRAC - razina B (sastavi), zadatak 2: Nadzor protoka plina
    // ============================================================
    //  Dobivas samo sucelja i potpise.
    //
    //  Razlika u odnosu na B1: ovdje postoji VISE subjekata. Isti
    //  pratitelj mora se moci pretplatiti na dva razlicita mjeraca i
    //  primati obavijesti od oba.
    //
    //  Trazi se:
    //    FlowSensor(name)         mjerac s vlastitim imenom
    //    SetFlow(double)          postavi protok i obavijesti pretplacene
    //    MailNotifier             pamti zadnju poruku "ime: vrijednost"
    //    FlowRecorder             broji primljene obavijesti
    //
    //  Obavijest nosi i IME mjeraca, inace pratitelj ne bi znao tko
    //  ga je obavijestio. To je "push" nacin obavjescivanja.
    // ============================================================

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
        public FlowSensor(string name)
        {
            throw new NotImplementedException("FlowSensor konstruktor");
        }

        public string Name
        {
            get { throw new NotImplementedException("FlowSensor.Name"); }
        }

        public double Flow
        {
            get { throw new NotImplementedException("FlowSensor.Flow"); }
        }

        /// <summary>Postavlja protok i obavjescuje sve pretplacene.</summary>
        public void SetFlow(double flow)
        {
            throw new NotImplementedException("FlowSensor.SetFlow");
        }

        public void Subscribe(IFlowObserver observer)
        {
            throw new NotImplementedException("FlowSensor.Subscribe");
        }

        public void Unsubscribe(IFlowObserver observer)
        {
            throw new NotImplementedException("FlowSensor.Unsubscribe");
        }

        public void Notify()
        {
            throw new NotImplementedException("FlowSensor.Notify");
        }
    }

    public class MailNotifier : IFlowObserver
    {
        /// <summary>Zadnja poruka u obliku "ime: vrijednost" (npr. "Zapad: 12.5").</summary>
        public string LastMessage
        {
            get { throw new NotImplementedException("MailNotifier.LastMessage"); }
        }

        public void OnFlowChanged(string sensorName, double flow)
        {
            throw new NotImplementedException("MailNotifier.OnFlowChanged");
        }
    }

    public class FlowRecorder : IFlowObserver
    {
        public int Received
        {
            get { throw new NotImplementedException("FlowRecorder.Received"); }
        }

        public void OnFlowChanged(string sensorName, double flow)
        {
            throw new NotImplementedException("FlowRecorder.OnFlowChanged");
        }
    }
}
