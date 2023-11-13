# IronSoftware.Internals

Components which can be shared between IronSoftware Libraries

## IronSoftware.Analytics

A library for sending anonymous analytics and crash data from within Iron
Software trials

Nuget Dependencies: Newtonsoft.JSON
Assembly Dependencies: System.Web (Framework Nuget)

Never Obfuscate:
IronSoftware.Analytics.Events.*
(IronSoftware.Analytics.Events.AnalyticsEvent)

Ideas:

- Look for a Class imported only in VB to report stats on "C#" vs. "VB" vs. "F#"

## Ironsoftware.Interop

A fork of NativeLibraryLoader

- Improved Linux and BSD support.  Works for .Net 4.0 and .Net Standard 2.0
- Also includes a console app to upgrade in InteropDotNet cs file to NativeLibraryLoader notation
- Supports side-by-side use of 2 different versions of the same DLL

To use:
I recommend using the **IronNativeLibraryFactory.CreateCrossPlatformNative**
method to generate a NativeLibrary

- XMLDocs Intellisense explains usage
- Creates a NativeLibrary instance that works on  Windows, Linux and Mac.
- Supports net40, netstandard >= 2.0, netcore >= 2.0 and mono without additional
configuration.
- On Linux and mac will automatically search package managers (apt-get, yum,
homebrew...) and system environment paths (LD_LIBRARY_PATH,
DYLD_LIBRARY_PATH, PATH...)  to find binaries
- On Windows will automatically load MSVC runtime if those binaries are present
in the path specified in CreateCrossPlatformNative which contains your DLLs. We
can copy these from C:\Program Files (x86)\Microsoft Visual Studio\2019\Community\VC\Redist\MSVC\14.24.28127\x64 and x86
respectively
- On older Windows OSs such as Server 2012, MSVC must always be manually
installed because the OS does not have all the prerequisites for the Resits
approach above. A `System.FileLoadException` will be thrown with bit
architecture-aware instructions on how to install MSVC on that OS.
- Will automatically add x64 or x86 to binary paths if those folders are present
and contain files

``` csharp
NativeLibrary Lept = IronNativeLibraryFactory.CreateCrossPlatformNative(new[] { "leptonica-1.78.0.dll", "liblept.so.5", "liblept.5.so", "liblept.5.dylib", "liblept.dylib" }, true, new[] { Path.GetFullPath("MyBinaryPath") });
```

Files Deployed Alongside:

``` console
MyBinaryPath/x64/leptonica-1.78.0.dll
// the rest are for MSVC
MyBinaryPath/x64/concrt140.dll
MyBinaryPath/x64/vccorlib140.dll
MyBinaryPath/x64/vcruntime140.dll
MyBinaryPath/x64/vcruntime140_1.dll
MyBinaryPath/x64/msvcp140.dll
MyBinaryPath/x64/msvcp140_1.dll
MyBinaryPath/x64/msvcp140_2.dll
MyBinaryPath/x64/msvcp140_codecvt_ids.dll

MyBinaryPath/x86/ ... same file names for x86
```

NOTE: For Linux leptonica.so.5 can also be deployed to MyBinaryPath/x64/ but
this is not smart because it contains so many OS-specific dependencies in its
build. Instead, install with apt-get or homebrew.  This works without any
configuration.
