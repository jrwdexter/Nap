namespace Nap

type FinalizableType<'T> = 
    | Continuing of 'T
    | Final of 'T

module Finalizable =
    let bind f finalizable =
        match finalizable with
        | Continuing(c) -> f c
        | final -> final

    let inline map f finalizable =
        match finalizable with
        | Continuing(c) -> f c |> Continuing
        | final -> final

    let get finalizable =
        match finalizable with
        | Continuing(v) -> v
        | Final(v) -> v

    let reset finalizable =
        match finalizable with
        | Final(v) -> Continuing(v)
        | x -> x

    let toAsync (finalizable:FinalizableType<'T>) =
        async {
            return finalizable
        }

    let asAsync finalizable =
        async {
            match finalizable with
            | Continuing(x) ->
                let! res = x
                return Continuing(res)
            | Final(x) -> 
                let! res = x
                return Final(res)
        }

    let inline (>>=) finalizable f = bind f finalizable
    let inline (<!>) finalizable f = map f finalizable

module AsyncFinalizable =
    let map f finalizableAsync =
        async {
            let! finalizable = finalizableAsync
            match finalizable with
            | Continuing(v) -> return f v |> Continuing
            | Final(v) -> return Final(v)
        }
    
    let bindAsync f finalizableAsync =
        async {
            let! finalizable = finalizableAsync
            match finalizable with
            | Continuing(v) -> 
                let! result = f v
                return result |> Continuing
            | Final(v) -> return Final(v)
        }

    let get finalizableAsync =
        async {
            let o = Some(true)
            let! finalizable = finalizableAsync
            return finalizable |> Finalizable.get
        }
        

    let inline (<!!>) finalizableAsync f = map f finalizableAsync

