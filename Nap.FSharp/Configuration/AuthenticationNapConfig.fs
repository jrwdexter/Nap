namespace Nap

type AuthenticationNapConfig =
    | Unauthenticated
    | BasicAuthentication of Credentials
    | DigestAuthentication of Credentials
    | ClientAuthentication of Credentials
    | FormsAuthentication of Credentials
    | SAMLAuthentication of Credentials // TODO
    | OAuthV1Authentication of Credentials // TODO
    | OAuthV2Authentication of Credentials // TODO
    override x.ToString() =
        sprintf "%A" x