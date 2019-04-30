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

## Loading source files into the script

`script2.fsx` works around the issue by loading all source files directly:

```powershell

[13] > dotnet fsi .\script2.fsx
5 ^2 *2 = 50
[14] >

```

But this might not be a feasible way in case of bigger projects with many files.
