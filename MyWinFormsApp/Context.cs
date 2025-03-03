using Guru.DependencyInjection;
using Guru.DependencyInjection.Attributes;

namespace MyWinFormsApp
{
    [Injectable(typeof(IContext), Lifetime.Singleton)]
    public class Context : IContext
    {
        public string Source { get; set; }

        public string Target { get; set; }
    }
}