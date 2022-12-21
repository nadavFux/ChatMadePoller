using OpenTracing;
using OpenTracing.Propagation;

namespace Tracing
{
    public class OpenTracer : ITracer
    {
        private readonly ITracer _tracer;

        public OpenTracer(ITracer tracer)
        {
            _tracer = tracer;
        }

        public IScopeManager ScopeManager => _tracer.ScopeManager;

        public ISpan ActiveSpan => _tracer.ActiveSpan;

        public ISpanBuilder BuildSpan(string operationName)
        {
            return _tracer.BuildSpan(operationName);
        }

        public void Inject<TCarrier>(ISpanContext spanContext, IFormat<TCarrier> format, TCarrier carrier)
        {
            _tracer.Inject(spanContext, format, carrier);
        }

        public ISpanContext Extract<TCarrier>(IFormat<TCarrier> format, TCarrier carrier)
        {
            return _tracer.Extract(format, carrier);
        }

        public void SetState(string key, string value)
        {
            _tracer.ActiveSpan.SetTag(key, value);
        }

        public void SetState(string key, bool value)
        {
            _tracer.ActiveSpan.SetTag(key, value);
        }

        public void SetState(string key, int value)
        {
            _tracer.ActiveSpan.SetTag(key, value);
        }

        public void SetState(string key, long value)
        {
            _tracer.ActiveSpan.SetTag(key, value);
        }

        public void SetState(string key, double value)
        {
            _tracer.ActiveSpan.SetTag(key, value);
        }
    }
}