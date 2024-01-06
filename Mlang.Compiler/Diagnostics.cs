namespace Mlang;

internal static partial class Diagnostics
{
    internal static readonly DiagnosticCategory CategoryInternal = new("INTERNAL");
    internal static readonly DiagnosticType TypeInternal = CategoryInternal.CreateWithFootNote(
        Severity.InternalError, "{0}", "{1}");
    internal static Diagnostic DiagInternal(System.Exception e) =>
        TypeInternal.Create(new[] { e.Message, e.StackTrace ?? "" });

    internal static readonly DiagnosticCategory CategoryUnsupported = new DiagnosticCategory("SUPPORT");

    internal static readonly DiagnosticType TypeUnsupportedTemplates =
        CategoryUnsupported.Create(Severity.Error, "Templates are not supported yet");
    internal static Diagnostic DiagUnsupportedTemplates() =>
        TypeUnsupportedTemplates.Create();

    internal static readonly DiagnosticType TypeUnsupportedVirtualInheritance =
        CategoryUnsupported.Create(Severity.Error, "Virtual inheritance is not supported yet");
    internal static Diagnostic DiagUnsupportedVirtualInheritance() =>
        TypeUnsupportedVirtualInheritance.Create();
}
