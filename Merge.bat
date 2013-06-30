@echo off

Libraries\ILMerge\ILMerge.exe /keyfile:Support\Key.snk /out:CSharpSyntax.dll /internalize CSharpSyntax\bin\Release\CSharpSyntax.dll CSharpSyntax\bin\Release\Antlr3.Runtime.dll
