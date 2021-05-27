// ReSharper disable once CheckNamespace
namespace System.Runtime.CompilerServices
{
    // Enables extension methods in assembly that targets .NET 2.0

    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class | AttributeTargets.Method)]
    public sealed class ExtensionAttribute : Attribute
    {
    }
}