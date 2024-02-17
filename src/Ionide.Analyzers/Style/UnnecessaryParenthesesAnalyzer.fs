module Ionide.Analyzers.Style.UnnecessaryParenthesesAnalyzer

open FSharp.Analyzers.SDK
open FSharp.Compiler.EditorServices
open FSharp.Compiler.Text

let unnecessaryParenthesesAnalyzer: Analyzer<CliContext> =
    fun (context: CliContext) ->
        async {
            let! unnecessaryParentheses =
                UnnecessaryParentheses.getUnnecessaryParentheses
                    (Line.toZ >> context.SourceText.GetLineString)
                    context.ParseFileResults.ParseTree

            return
                [
                    for range in unnecessaryParentheses ->
                        {
                            Type = "UnnecessaryParenthesesAnalyzer"
                            Message = "Parentheses can be removed"
                            Code = "FS3583"
                            Severity = Severity.Info
                            Range = range
                            Fixes = []
                        }
                ]
        }
