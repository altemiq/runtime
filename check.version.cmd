dotnet tool install altavec.semanticversioning --global
  & dotnet tool update altavec.semanticversioning --global
dotnet build src
dotnet semver diff solution src^
  --output BreakingChanges^
  --source https://nuget.pkg.github.com/altavec/index.json ^
  --direct-download^
  --version-suffix personal^
  --package-id-regex Altavec^
  --package-id-replace Mondo