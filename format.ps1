dotnet format whitespace $(Join-Path $PSScriptRoot src) --report $(Join-Path $PSScriptRoot whitespace.report.json) --verbosity diagnostic --no-restore
dotnet format style $(Join-Path $PSScriptRoot src)  --report $(Join-Path $PSScriptRoot style.report.json) --severity info --verbosity diagnostic --no-restore
dotnet format analyzers $(Join-Path $PSScriptRoot src) --report $(Join-Path $PSScriptRoot analyzers.report.json) --severity info --verbosity diagnostic --exclude-diagnostics S1133 --no-restore
