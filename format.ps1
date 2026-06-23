dotnet format whitespace $PSScriptRoot --report $(Join-Path $PSScriptRoot whitespace.report.json) --verbosity detailed --no-restore
dotnet format style $PSScriptRoot --report $(Join-Path $PSScriptRoot style.report.json) --severity info --verbosity detailed --no-restore
dotnet format analyzers $PSScriptRoot --report $(Join-Path $PSScriptRoot analyzers.report.json) --severity info --verbosity detailed --exclude-diagnostics MA0182 S1133 --no-restore
