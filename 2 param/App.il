//  todo: Date type
// IL from BDull
.assembly extern System.Runtime {
   .publickeytoken = (B0 3F 5F 7F 11 D5 0A 3A )
   .ver 7:0:0:0
}
.assembly App {}
.module App.dll
.class public auto ansi beforefieldinit App.User
{
   .field public initonly string First
   .field private initonly string Last
   
   .field public string Name
   .field public int32 Age
   
   .method public hidebysig specialname rtspecialname instance void
   .ctor(
      string First,
      string Last
   ) cil managed {
      .maxstack 8
      ldarg.0 // this
      ldarg.1
      stfld string App.User::First
      ldarg.0 // this
      ldarg.2
      stfld string App.User::Last
      ldarg.0 // this
      // concats
      ldarg.0
      ldfld string App.User::First
      ldstr " "
      ldarg.0
      ldfld string App.User::Last
      call string [System.Runtime]System.String::Concat(string, string, string)
      stfld string App.User::Name

      ldarg.0 // this
      ldc.i4.3
      stfld int32 App.User::Age
      nop
      ret
   }

}
