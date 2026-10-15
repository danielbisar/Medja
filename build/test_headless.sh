#!/usr/bin/env bash
set -euo pipefail

root="$(cd -- "$(dirname -- "${BASH_SOURCE[0]}")/.." && pwd)"
cd -- "$root"

# CPU-only suites; OpenTK/Skia and .NET Framework runtime tests remain legacy.
for project in Medja.Test Medja.Utils.Test Medja.Properties.Test Medja.ProtoBuf.Test Medja.ZeroMQ.Test; do
    dotnet test "src/$project/$project.csproj" --configuration Release --framework net10.0 "$@"
done
