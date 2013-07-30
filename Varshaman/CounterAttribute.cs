using System;
using PostSharp.Aspects;
using PostSharp.Extensibility;
using StatsdClient;

namespace Varshaman
{
    [Serializable]
    [MulticastAttributeUsage(MulticastTargets.Method)]
    public sealed class CounterAttribute : OnMethodBoundaryAspect
    {
        private string _name;
        private readonly int _value;
        private readonly double _sampleRate;

        public CounterAttribute(string name, int value = 1, double sampleRate = 1)
        {
            _name = name;
            _value = value;
            _sampleRate = sampleRate;
        }

        public override void OnEntry(MethodExecutionArgs args)
        {
            if (string.IsNullOrEmpty(_name))
                _name = args.Method.Name;

            base.OnEntry(args);
        }

        public override void OnExit(MethodExecutionArgs args)
        {
            Metrics.Counter(_name, _value, _sampleRate);
            base.OnExit(args);
        }

        public override void OnException(MethodExecutionArgs args)
        {
            Metrics.Counter(string.Format("{0}.error", _name));
            base.OnException(args);
        }
    }
}
