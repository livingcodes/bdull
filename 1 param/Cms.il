// IL from BDull
.assembly extern System.Runtime {
   .publickeytoken = (B0 3F 5F 7F 11 D5 0A 3A )
   .ver 7:0:0:0
}
.assembly Cms {}
.module Cms.dll
.class public auto ansi beforefieldinit Cms.User
{
   .field public initonly string First

   .method public hidebysig specialname rtspecialname instance void
   .ctor(
      string First
   ) cil managed {
      .maxstack 4
      ldarg.0 // this
      ldarg.1
      stfld string Cms.User::First
      nop
      ret
   }
}