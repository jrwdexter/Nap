namespace Nap

type FinalizableType<'T> = 
    | Continuing of 'T
    | Final of 'T

type Event<'T> =
    | ModifyingEvent of ('T -> FinalizableType<'T>)
    | ActionEvent of ('T -> unit)
    with
    member x.RunEvent value = 
        match x with
        | ModifyingEvent(e) -> e value
        | ActionEvent(e) -> e value; Continuing(value)

module Finalizable =
    let bind f finalizable =
        match finalizable with
        | Continuing(c) -> f c
        | final -> final

    let map f finalizable =
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

    let inline (>>=) finalizable f = bind f finalizable
    let inline (<!>) finalizable f = map f finalizable
