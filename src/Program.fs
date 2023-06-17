open System.IO

let updateFile path = task {
    let! bytes = File.ReadAllBytesAsync path
    let mutable updated = false

    let newBytes =
        bytes
        |> Array.pairwise
        |> Array.map (fun (b1, b2) ->
            if not updated && b1 = 28uy then
                updated <- true
                1uy
            else
                b2
        )
        |> Array.append [| bytes |> Array.head |]

    do! File.WriteAllBytesAsync (path, newBytes)
}

[<EntryPoint>]
let main args =
    match args |> Array.tryItem 0 with
    | Some filePath ->
        match File.Exists filePath with
        | false ->
            failwith "File doesn't exist"
            2
        | true ->
            printfn "Patching file ..."
            updateFile(filePath).Result
            printfn "File patched ! ⭐"

            0
    | None ->
        failwith "No path in arguments"
        1