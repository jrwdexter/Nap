namespace Nap

open System
open System.Threading.Tasks
open System.Text
open System.Text.RegularExpressions

module Async =
    let inline awaitPlainTask (task: Task) = 
        // rethrow exception from preceding task if it fauled
        let continuation (t : Task) : unit =
            match t.IsFaulted with
            | true -> raise t.Exception
            | arg -> ()
        task.ContinueWith continuation |> Async.AwaitTask

    let map f a =
        async {
            let! value = a
            return f value
        }

module ExceptionHelpers =
    let notSupported message : 'T =
        raise <| new NotSupportedException(message)

module Operators =
    let (<~>) f1 f2 x = f1 x, f2 x
    let (|??) v1 v2 = 
        if v1 |> isNull
        then v2
        else v1

module Map =
    let join map1 map2 =
        Map( Seq.concat [
                map1 |> Map.toSeq;
                map2 |> Map.filter (fun k v -> not <| map1.ContainsKey k) |> Map.toSeq
            ])

module Text =
    let (|Prefix|_|) prefix (s:string) =
        if (s.StartsWith(prefix, StringComparison.OrdinalIgnoreCase))
        then s.Substring(prefix.Length) |> Some
        else None

    let (|Suffix|_|) suffix (s:string) =
        if (s.EndsWith(suffix, StringComparison.OrdinalIgnoreCase))
        then s.Substring(0, s.Length - suffix.Length) |> Some
        else None

    let (|Regex|_|) regex s =
        let m = Regex.Match(s, regex)
        if m.Success
        then [ for g in m.Groups -> g.Value ] |> List.tail |> Some
        else None

    let (|Split|_|) splitString (s:string) =
        match s.Contains(splitString) with
        | true -> s.Split([|splitString|], StringSplitOptions.RemoveEmptyEntries) |> Seq.toList |> Some
        | false -> None