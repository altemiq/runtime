dotnet tool install altemiq.semanticversioning --global --add-source https://nuget.pkg.github.com/altemiq/index.json^
  & dotnet tool update altemiq.semanticversioning --global --add-source https://nuget.pkg.github.com/altemiq/index.json
dotnet build src
dotnet semver diff solution src^
  --output BreakingChanges^
  --source https://nuget.pkg.github.com/altemiq/index.json ^
  --direct-download^
  --version-suffix personal^
  --package-id-regex Altemiq^
  --package-id-replace Mondo