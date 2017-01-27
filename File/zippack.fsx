// archive zip for release package
#r "Ionic.Zip.dll"

open System
open System.IO
open System.Text.RegularExpressions
open Ionic.Zip

let rootDir = (new DirectoryInfo(__SOURCE_DIRECTORY__)).Parent
let pass p = Path.Combine(rootDir.FullName , p)

let files = 
    ["File\ReadMe.txt";
     "ChainingAssertion\ChainingAssertion.MSTest.cs";
     "ChainingAssertion.MbUnit\ChainingAssertion.MbUnit.cs";
     "ChainingAssertion.xUnit\ChainingAssertion.xUnit.cs";]
    |> Seq.map pass

// compress
do
    use zip = new ZipFile()
    zip.AddFiles(files, "")
    Path.Combine(__SOURCE_DIRECTORY__, "archive.zip") |> zip.Save