namespace Nap

module Map =
    let join map1 map2 =
        Map( Seq.concat [
                map1 |> Map.toSeq;
                map2 |> Map.filter (fun k v -> not <| map1.ContainsKey k) |> Map.toSeq
            ])