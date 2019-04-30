# The issue

## Referencing library dll from a script

The script trying to make use of the library `B.dll`.

The library `B.dll`, in turn, uses a function from `A.dll`.

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

Referencing `A.dll` in the script doesn't change anything.

This is the main issue. Following are some experiments to narrow it down.

## Loading source files into the script

`script2.fsx` works around the issue by loading all source files directly:

```powershell

[13] > dotnet fsi .\script2.fsx
5 ^2 *2 = 50
[14] >

```

This is not be a feasible way in case of bigger projects with many files. This will require manual tracking of code structure changes.

## Checking that library dependencies are solved when used from elsewhere

### Referencing a project

Added `B.Tests` project that calls a function from `B` that calls a function from `A`. The test works, dependencies are resolved.

I got another [issue](https://github.com/ionide/ionide-vscode-fsharp/issues/924) in the process, but that's irrelevant here.

### Referencing a dll

Added `B.DllTests` project to do the same while referencing dll instead of fsproj. This works too, but after small fix.

I added the dll reference using [DCE](https://marketplace.visualstudio.com/items?itemName=kishoreithadi.dotnet-core-essentials) Add DLL Reference feature. The reference in the fsproj file looked like this:

```xml
    <ItemGroup>
        <Reference Include="B">
            <HintPath>FsiOddityDemo\B\bin\Debug\netcoreapp2.2\B.dll</HintPath>
        </Reference>
    </ItemGroup>
```

Which is wrong. Changing path to `..\B\bin\Debug\netcoreapp2.2\B.dll` manually made it to work as intended. Again, this might be something to look into later, but it seems to be irrelevant to the FSI issue.
