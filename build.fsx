#r "nuget:Fake.Core.Target, 5.23"
#r "nuget:Fake.IO.FileSystem, 5.23"

open Fake.Core
open Fake.IO

#load ".build/Helpers.fs"
open Helpers

initializeContext ()

let serverPath = Path.getFullName "src/Server"
let clientPath = Path.getFullName "src/Client"
let deployPath = Path.getFullName ".deploy"
let serverTestsPath = Path.getFullName "test/Server.Tests"

Target.create "Clean" (fun _ ->
    Shell.cleanDir deployPath
    run dotnet ["clean"] serverPath
    run npm [ "run"; "clean" ] clientPath
)

Target.create "RestoreClientDependencies" (fun _ -> run npm [ "install" ] clientPath)

Target.create "Build" (fun _ ->
    run dotnet [ "build" ] serverPath
    run npm [ "run"; "build" ] clientPath
)

Target.create "Bundle" (fun _ ->
    let publicDir = Path.combine deployPath "public"
    let clientDeployPath = Path.combine clientPath "dist"

    run dotnet [ "publish"; "-c"; "Release"; "-o"; deployPath ] serverPath
    run npm [ "run"; "build"; ] clientPath

    Shell.copyDir publicDir clientDeployPath FileFilter.allFiles
)

Target.create "Run" (fun _ ->
    runParallel [
        "server", dotnet [ "watch"; "run"; "--no-restore" ] serverPath
        "client", npm [ "run"; "dev"; ] clientPath
    ]
)

Target.create "Test" (fun _ -> run dotnet ["test"] serverTestsPath)

Target.create "Format" (fun _ -> run dotnet [ "fantomas"; "." ] serverPath)

open Fake.Core.TargetOperators

"Clean"
==> "RestoreClientDependencies"
==> "Bundle"

"Clean"
==> "RestoreClientDependencies"
==> "Build"
==> "Run"

Target.runOrDefaultWithArguments "Build"