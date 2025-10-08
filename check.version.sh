#!/bin/sh

dotnet tool update altemiq.semanticversioning --global
dotnet build
dotnet semver diff solution \
  --output BreakingChanges \
  --direct-download \
  --version-suffix personal
  --force
