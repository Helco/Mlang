using System;
using System.Collections.Generic;
using System.Text;

namespace Mlang.Language;

internal class SourceDownstreamCompiler : IDownstreamCompiler
{
    public IDownstreamCompilationResult Compile(string source, TokenKind stage,
        IEnumerable<KeyValuePair<string, string>> macros,
        IEnumerable<string> extraOptions) =>
        new Result(source);

    public void Dispose() { }

    private class Result : IDownstreamCompilationResult
    {
        private readonly byte[] sourceBytes;
        public bool HasError => false;
        public IReadOnlyCollection<Diagnostic> Diagnostics => Array.Empty<Diagnostic>();
        ReadOnlySpan<byte> IDownstreamCompilationResult.Result => sourceBytes;

        public Result(string source) => sourceBytes = Encoding.UTF8.GetBytes(source);
    }
}
