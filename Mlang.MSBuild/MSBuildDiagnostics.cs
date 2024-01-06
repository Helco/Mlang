using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Yoakke.SynKit.Text;

namespace Mlang.MSBuild;

internal static class MSBuildDiagnostics
{
    public static readonly DiagnosticCategory CategoryInternal = new("MSBUILD");

    public static readonly DiagnosticType TypeShaderIOError = CategoryInternal.Create(
        Severity.Error, "Could not open file: {0}");

    public static Diagnostic DiagShaderIOError(string fileName, string errorMessage)
    {
        var sourceFile = new SourceFile(fileName, TextReader.Null);
        var sourceInfo = new Location(sourceFile, default);
        return TypeShaderIOError.Create([errorMessage], [sourceInfo]);
    }

    public static readonly DiagnosticType TypeShaderSetIOError = CategoryInternal.Create(
        Severity.Error, "Failed to write shader set: {0}");

    public static Diagnostic DiagShaderSetIOError(string fileName, string errorMessage)
    {
        var sourceFile = new SourceFile(fileName, TextReader.Null);
        var sourceInfo = new Location(sourceFile, default);
        return TypeShaderIOError.Create([errorMessage], [sourceInfo]);
    }
}
