using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PostSharp.Aspects;
using PostSharp.Extensibility;
using StatsdClient;
using Stopwatch = System.Diagnostics.Stopwatch;

namespace Varshaman
{
    [Serializable]
    [MulticastAttributeUsage(MulticastTargets.Method)]
    public sealed class TimingAttribute : OnMethodBoundaryAspect
    {
        [NonSerialized]
        private Stopwatch _stopwatch;

        private string _name;

        public TimingAttribute(string name)
        {
            _name = name;
        }

        public override void OnEntry(MethodExecutionArgs args)
        {
            if (string.IsNullOrEmpty(_name))
                _name = args.Method.Name;

            _stopwatch = Stopwatch.StartNew();
            base.OnEntry(args);
        }

        public override void OnExit(MethodExecutionArgs args)
        {
            Metrics.Timer(_name, (int)_stopwatch.Elapsed.TotalMilliseconds);
            base.OnExit(args);
        }

        public override void OnException(MethodExecutionArgs args)
        {
            Metrics.Gauge(string.Format("{0}.error", _name), (int)_stopwatch.Elapsed.TotalMilliseconds);
            base.OnException(args);
        }
    }
}
