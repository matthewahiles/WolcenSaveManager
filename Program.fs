open System
open System.IO

let savePath = "\\Saved Games\\wolcen\\savegames\\characters"
let stashPath = "\\Saved Games\\wolcen\\savegames"

let buildDirectory basePath =
    Directory.CreateDirectory(
        (basePath + savePath).Replace("wolcen", "wolcen-backup")
    ) |> ignore

let rec runner() =
    System.Threading.Thread.Sleep 1
    runner()

let currentDate() = DateTime.Now.ToString("-yyyy-MM-dd-HHmm")

let formatFile (str:string) =
    let len = str.Length
    let start = str.Substring(0, len - 5).Replace("wolcen", "wolcen-backup")
    String.Concat(start, currentDate(), ".json")

let fileWatcher (evt:FileSystemEventArgs) =
    System.IO.File.Copy(evt.FullPath, formatFile(evt.FullPath), true)
    printfn "Backed up: %A" evt.FullPath

let buildWatcher path =
    let watcher = new FileSystemWatcher()
    watcher.Path <- path
    watcher.NotifyFilter <- NotifyFilters.LastWrite
    watcher.Filter <- "*.json"
    watcher.Changed.Add fileWatcher
    watcher.EnableRaisingEvents <- true

[<EntryPoint>]
let main args =
    let basePath = System.Environment.GetEnvironmentVariable "USERPROFILE"
    buildDirectory basePath
    buildWatcher <| basePath + savePath
    buildWatcher <| basePath + stashPath
    printfn "File Watcher Started" 
    runner()
    0