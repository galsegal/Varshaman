using System;
using PostSharp.Aspects;
using PostSharp.Extensibility;
using StatsdClient;

namespace Varshaman
{
    [Serializable]
    [MulticastAttributeUsage(MulticastTargets.Method)]
    public sealed class GaugeAttribute : OnMethodBoundaryAspect
    {
        private string _name;
        private readonly int _value;

        public GaugeAttribute(string name, int value)
        {
            _name = name;
            _value = value;
        }

        public override void OnEntry(MethodExecutionArgs args)
        {
            if (string.IsNullOrEmpty(_name))
                _name = args.Method.Name;

            base.OnEntry(args);
        }

        public override void OnExit(MethodExecutionArgs args)
        {
            Metrics.Gauge(_name, _value);
            base.OnExit(args);
        }

        public override void OnException(MethodExecutionArgs args)
        {
            Metrics.Gauge(string.Format("{0}.error", _name), _value);
            base.OnException(args);
        }
    }
}
