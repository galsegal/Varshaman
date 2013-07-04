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

        public CounterAttribute(string name)
        {
            _name = name;
        }

        public override void OnEntry(MethodExecutionArgs args)
        {
            if (string.IsNullOrEmpty(_name))
                _name = args.Method.Name;

            base.OnEntry(args);
        }

        public override void OnExit(MethodExecutionArgs args)
        {
            Metrics.Counter(_name);
            base.OnExit(args);
        }

        public override void OnException(MethodExecutionArgs args)
        {
            Metrics.Counter(string.Format("{0}.error", _name));
            base.OnException(args);
        }
    }
}
