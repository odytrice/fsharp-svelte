#r "nuget:Fake.Core.Target, 5.23"
#r "nuget:Fake.IO.FileSystem, 5.23"

open Fake.Core
open Fake.IO

#load ".build/Helpers.fs"
open Helpers

initializeContext ()

let serverPath = Path.getFullName "Server"
let clientPath = Path.getFullName "Client"
let deployPath = Path.getFullName ".deploy"

Target.create "Clean" (fun _ ->
    Shell.cleanDir deployPath
    run npm [ "run"; "clean" ] clientPath
)

Target.create "RestoreClientDependencies" (fun _ -> run npm [ "ci" ] clientPath)

Target.create "Bundle" (fun _ ->
    runParallel [
        "server", dotnet [ "publish"; "-c"; "Release"; "-o"; deployPath ] serverPath
        "client", npm [ "run"; "build"; ] clientPath
    ]
    let publicDir = Path.combine deployPath "public"
    let clientDeployPath = Path.combine clientPath "dist"
    Shell.copyDir publicDir clientDeployPath FileFilter.allFiles)

Target.create "Build" (fun _ ->
    run dotnet [ "build" ] serverPath
    run npm [ "run"; "build" ] clientPath
    )

Target.create "Run" (fun _ ->
    [
        "server", dotnet [ "watch"; "run"; "--no-restore" ] serverPath
        "client", npm [ "run"; "dev"; ] clientPath
    ]
    |> runParallel)


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