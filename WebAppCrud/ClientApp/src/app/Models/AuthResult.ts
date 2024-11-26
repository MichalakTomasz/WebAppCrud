export class AuthResult
{
  userId? : string
  token? : string
  roles? : Array<string>
  isAuthorized : boolean = false
}
