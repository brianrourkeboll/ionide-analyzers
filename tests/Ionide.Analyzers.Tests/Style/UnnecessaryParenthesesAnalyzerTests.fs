module Ionide.Analyzers.Tests.Style.UnnecessaryParenthesesAnalyzerTests

open FSharp.Analyzers.SDK
open FSharp.Analyzers.SDK.Testing
open FSharp.Compiler.CodeAnalysis
open Ionide.Analyzers.Style.UnnecessaryParenthesesAnalyzer
open NUnit.Framework

let mutable private projectOptions = FSharpProjectOptions.zero

[<SetUp>]
let setUp () =
    task {
        let! opts = mkOptionsFromProject "net8.0" []
        projectOptions <- opts
    }

[<Test>]
let ``Unnecessary parentheses should produce diagnostic`` () =
    async {
        let source =
            """
            let x = (3)
            """

        let ctx = getContext projectOptions source
        let! msgs = unnecessaryParenthesesAnalyzer ctx
        Assert.That(msgs, Has.Exactly(1).Items)
        let msg = msgs[0]
        Assert.That(msg.Code, Is.EqualTo "FS3583")
        Assert.That(msg.Message, Is.EqualTo "Parentheses can be removed")
        Assert.That(msg.Severity, Is.EqualTo Severity.Info)
    }

[<Test>]
let ``No unnecessary parentheses should produce no diagnostics`` () =
    async {
        let source =
            """
            let x = (2 + 2) * 3
            """

        let ctx = getContext projectOptions source
        let! msgs = unnecessaryParenthesesAnalyzer ctx
        Assert.That(msgs, Is.Empty)
    }
