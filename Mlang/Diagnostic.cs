using System;
using System.Collections.Generic;
using System.Linq;
using Yoakke.SynKit.Reporting;
using Yoakke.SynKit.Text;
using SynKitSeverity = Yoakke.SynKit.Reporting.Severity;
using SynKitDiagnostics = Yoakke.SynKit.Reporting.Diagnostics;

namespace Mlang;

public enum Severity
{
    Info,
    Warning,
    Error,
    InternalError
}

public class DiagnosticCategory
{
    private readonly List<DiagnosticType> types = new();

    public string Name { get; }
    public IReadOnlyList<DiagnosticType> Types => types;

    public DiagnosticCategory(string name) => Name = name;

    internal DiagnosticType Error(string message, params string[] sourceInfoMessages) =>
        Create(Severity.Error, message, sourceInfoMessages);
    internal DiagnosticType Warning(string message, params string[] sourceInfoMessages) =>
        Create(Severity.Warning, message, sourceInfoMessages);
    internal DiagnosticType Create(Severity severity, string message, params string[] sourceInfoMessages)
    {
        var type = new DiagnosticType(this, types.Count + 1, severity, message, sourceInfoMessages, footNote: null);
        types.Add(type);
        return type;
    }

    internal DiagnosticType CreateWithFootNote(Severity severity, string message, string footNote, params string[] sourceInfoMessages)
    {
        var type = new DiagnosticType(this, types.Count + 1, severity, message, sourceInfoMessages, footNote);
        types.Add(type);
        return type;
    }
}

public class DiagnosticType
{
    public DiagnosticCategory Category { get; }
    public int CodeNumber { get; }
    public Severity Severity { get; }
    public string Message { get; }
    public IReadOnlyList<string?> SourceInfoMessages { get; }
    public string? FootNote { get; }
    public string Code => $"{Category.Name}{CodeNumber:D3}";

    public DiagnosticType(
        DiagnosticCategory category,
        int codeNumber,
        Severity severity,
        string message,
        IReadOnlyList<string?> sourceInfoMessages,
        string? footNote)
    {
        Category = category;
        CodeNumber = codeNumber;
        Severity = severity;
        Message = message;
        SourceInfoMessages = sourceInfoMessages;
        FootNote = footNote;
    }

    internal Diagnostic Create(object?[]? messageParams = null, Location[]? sourceInfos = null) =>
        new(this, messageParams ?? Array.Empty<object>(), sourceInfos ?? Array.Empty<Location>());
}

public readonly record struct Diagnostic(
    DiagnosticType Type,
    IReadOnlyList<object?> MessageParams,
    IReadOnlyList<Location> SourceInfos)
{
    public DiagnosticCategory Category => Type.Category;
    public Severity Severity => Type.Severity;
    public string FormattedMessage => string.Format(Type.Message, MessageParams.ToArray());
    public string? FormattedFootNote => Type.FootNote == null ? null : string.Format(Type.FootNote, MessageParams.ToArray());

    internal SynKitDiagnostics ConvertToSynKit()
    {
        var synkit = new SynKitDiagnostics()
            .WithSeverity(Type.Severity switch
            {
                Severity.Info => SynKitSeverity.Note,
                Severity.Warning => SynKitSeverity.Warning,
                Severity.Error => SynKitSeverity.Error,
                Severity.InternalError => SynKitSeverity.InternalError,
                _ => throw new NotImplementedException($"Unimplemented severity kind: {Type.Severity}")
            })
            .WithCode(Type.Code)
            .WithMessage(FormattedMessage);
        if (Type.FootNote != null)
            synkit = synkit.WithFootnoteInfo(FormattedFootNote!);
        var extendedMessages = Type.SourceInfoMessages.Concat(Enumerable.Repeat(null as string, SourceInfos.Count));
        foreach (var (location, message) in SourceInfos.Zip(extendedMessages))
            synkit = synkit.WithSourceInfo(location, message!);
        return synkit;
    }
}
