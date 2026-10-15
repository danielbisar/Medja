# Archived Linux setup tooling

These scripts preserve the historical Mono/MSBuild development environment.
They are not supported installation instructions for current Linux systems.
In particular, `install_mono.sh` references an obsolete Ubuntu Xenial repository,
and `build_open_tk.sh` clones an unpinned development branch.

Do not run `setup_system.sh` to provision a current workstation. Use the
[supported .NET SDK headless build/test path](../../docs/build.md) instead.
Framework-reference packages allow compiling the legacy backend without
installing Mono; GUI execution remains separately unverified.

The tracked native SkiaSharp library has now been matched by hash to the
official Linux x86-64 v1.68.0 release. See
[native provenance and redistribution notices](../../THIRD-PARTY-NOTICES.md).
