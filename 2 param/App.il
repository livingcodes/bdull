//  todo: set last name
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
   
   .method public hidebysig specialname rtspecialname instance void
   .ctor(
      string First,
      string Last
   ) cil managed {
      .maxstack 4
      ldarg.0 // this
      ldarg.1
      stfld string App.User::First
      ldarg.0 // this
      ldarg.2
      stfld string App.User::Last
      nop
      ret
   }

//  First + Last
   .field public string Name
   .field public int32 Age
}
