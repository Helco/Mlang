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

    private class Result : IDownstreamCompilationResult
    {
        private readonly byte[] sourceBytes;
        public bool HasError => false;
        public IReadOnlyCollection<Diagnostic> Diagnostics => Array.Empty<Diagnostic>();
        public uint CompilerHash => 0x12AA512Cu;
        ReadOnlySpan<byte> IDownstreamCompilationResult.Result => sourceBytes;

        public Result(string source) => sourceBytes = Encoding.UTF8.GetBytes(source);
    }
}
