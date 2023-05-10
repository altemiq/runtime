# Run dotnet test
dotnet test $PSScriptRoot\src --no-build --results-directory "$PSScriptRoot/coverage/results"

# install the report generator
dotnet tool install -g dotnet-reportgenerator-globaltool

# run the report generator
reportGenerator -reports:"$PSScriptRoot/coverage/results/*/coverage.cobertura.xml" -targetdir:"$PSScriptRoot/coverage/reports" -reporttypes:"HtmlInline;Cobertura;MarkdownSummary" -verbosity:Verbose