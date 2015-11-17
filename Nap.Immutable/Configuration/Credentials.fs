namespace Nap

type Credentials = 
    {
        Username : string
        Password : string
    }
    override x.ToString() =
        sprintf "%A" x