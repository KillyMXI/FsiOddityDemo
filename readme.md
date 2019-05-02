# The issue

## The solution (starting from the end)

[The issue](https://github.com/Microsoft/visualfsharp/issues/6662) as described below does happen on .NET Core 2.2 (most up to date release at the moment of writing this update).

The fix is currently available in the preview versions of .NET Core and will get into releases of 2.1 and 2.2 some day (and 3.0 of course). No changes in code  needed, `dotnet.exe` just have to be the fresh one.

And for current version of .NET Core 2.2 there is one way to get it working, described below.

## Referencing library dll from a script (library project depends on another library project) - Debug build

The script `script.fsx` trying to make use of the library `B.dll`.

The library `B.dll`, in turn, uses a function from `A.dll` (`B.fsproj` has a reference to `A.fsproj`).

Trying to run the `script.fsx` leads to an error:

```powershell

PS C:\...\FsiOddityDemo
[6] > dotnet fsi .\script.fsx
System.IO.FileNotFoundException: Could not load file or assembly 'A, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null'. The system cannot find the file specified.
File name: 'A, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null'
   at B.Worker.doWork(Int32 x)
   at <StartupCode$FSI_0002>.$FSI_0002.main@()


Stopped due to error
PS C:\...\FsiOddityDemo
[7] >

```

Solution had been built beforehand, IDE shows autocompletion for the script without issues.

Referencing `A.dll` in the script explicitly before `B.dll` doesn't change anything.

Switching between `fsi.exe` from following locations doesn't change anything:

* `C:\Program Files\dotnet\sdk\2.2.202\FSharp`
* `C:\Program Files (x86)\Microsoft Visual Studio\2019\Community\Common7\IDE\CommonExtensions\Microsoft\FSharp`

(Both are shown as `Microsoft (R) F# Interactive version 10.4.0 for F# 4.6`.)

## Referencing library dll from a script (library project depends on another library dll) - Debug build

The script `script3.fsx` trying to make use of the library `C.dll`.

The library `C.dll`, in turn, uses a function from `A.dll`. (`C.fsproj` has a reference to `A.dll`).

Trying to run the `script3.fsx` leads to the same as above error:

```powershell
PS C:\...\FsiOddityDemo
[2] > dotnet fsi .\script3.fsx
System.IO.FileNotFoundException: Could not load file or assembly 'A, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null'. The system cannot find the file specified.
File name: 'A, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null'
   at C.Doer.doStuff(Int32 x)
   at <StartupCode$FSI_0001>.$FSI_0001.main@()


Stopped due to error
PS C:\...\FsiOddityDemo
[3] >
```

## Release builds of above two options

It isn't clear how to choose the release build from GUI tools available in vscode so I went with what's available.

Now let's check it specifically.

```powershell
> dotnet build .\B\B.fsproj -c Release
> dotnet build .\C\C.fsproj -c Release

> dotnet fsi .\script.release.fsx
5 ^2 *2 = 50
> dotnet fsi .\script3.release.fsx
System.IO.FileNotFoundException: Could not load file or assembly 'A, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null'. The system cannot find the file specified.
File name: 'A, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null'
   at C.Doer.doStuff(Int32 x)
   at <StartupCode$FSI_0001>.$FSI_0001.main@()


Stopped due to error
```

This means the bug is absent in the release build when the reference is made by project, not dll.

## Loading source files into the script

The script `script2.fsx` works around the issue by loading all source files directly:

```powershell

[13] > dotnet fsi .\script2.fsx
5 ^2 *2 = 50
[14] >

```

This is not a feasible way in case of bigger projects with many files. This will require manual tracking of code structure changes.

## Checking that library dependencies are solved when used from elsewhere

### Referencing a project

Added `B.Tests` project that calls a function from `B` that calls a function from `A`. The test works, dependencies are resolved.

I got another [issue](https://github.com/ionide/ionide-vscode-fsharp/issues/924) in the process, but that's irrelevant here.

### Referencing a dll

Added `B.DllTests` project to do the same while referencing dll instead of fsproj. This works too, after a small fix.

I added the dll reference using [DCE](https://marketplace.visualstudio.com/items?itemName=kishoreithadi.dotnet-core-essentials) "Add DLL Reference" feature. The reference in the fsproj file looked like this:

```xml
    <ItemGroup>
        <Reference Include="B">
            <HintPath>FsiOddityDemo\B\bin\Debug\netcoreapp2.2\B.dll</HintPath>
        </Reference>
    </ItemGroup>
```

Which is wrong. Changing path to `..\B\bin\Debug\netcoreapp2.2\B.dll` manually made it to work as intended. Again, this might be something to look into later, but it is irrelevant to the FSI issue above.
