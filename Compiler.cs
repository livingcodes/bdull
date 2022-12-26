namespace BDull;
class Compiler
{
   public (string il, string ns) Compile(List<IToken> tokens) {
      var t = "   ";

      var output = "";
      output += @"// IL from BDull
.assembly extern System.Runtime {
   .publickeytoken = (B0 3F 5F 7F 11 D5 0A 3A )
   .ver 7:0:0:0
}";

      var ns = ((Namespace)tokens.First()).Value;
      var cl = ((Class)tokens[1]).Value;
      output += "\r\n" + $".assembly {ns} {{}}";
      output += "\r\n" + $".module {ns}.dll";
      output += "\r\n" + $".class public auto ansi beforefieldinit {ns}.{cl}";
      output += "\r\n" + "{";
      if (tokens.Count > 2 && tokens[2].GetType() == typeof(OpenParen)) {
         var nextToken = 3;
         var parameters = new List<(string t, string n)>();
         while (tokens[nextToken].GetType() == typeof(ParamType)) {
            var paramType = ((ParamType)tokens[nextToken]).Value;
            paramType = ConvertType(paramType);
            var varName = ((ParamName)tokens[nextToken+1]).Value;
            parameters.Add((paramType, varName));
            output += "\r\n" + $"{t}.field public initonly {paramType} {varName}";
            nextToken += 2;
            if (nextToken > tokens.Count-1)
               break;
            if (tokens[nextToken].GetType() == typeof(Comma))
               nextToken += 1;
         }
      
         output += "\r\n" + "";
         output += "\r\n" + $"{t}.method public hidebysig specialname rtspecialname instance void";
         output += "\r\n" + $"{t}.ctor(";
         var i = 0;
         foreach (var p in parameters) {
            output += $"\r\n{t}{t}{p.t} {p.n}";
            i += 1;
            if (i < parameters.Count)
               output += ",";
         }
         output += "\r\n" + $"{t}) cil managed {{";
         output += "\r\n" + $"{t}{t}.maxstack 4";
         i = 0;
         foreach (var p in parameters) {
            i += 1;
            output += "\r\n" + $"{t}{t}ldarg.0 // this";
            output += "\r\n" + $"{t}{t}ldarg.{i}";
            output += "\r\n" + $"{t}{t}stfld {p.t} {ns}.{cl}::{p.n}";
         }
         output += "\r\n" + $"{t}{t}nop";
         output += "\r\n" + $"{t}{t}ret";
         output += "\r\n" + $"{t}}}";
      
      }
      output += "\r\n" + "}";
      return (output, ns);
   }

   string ConvertType(string bdType) {
      var csType = bdType == "S"
         ? "string"
         : throw new Exception($"Type not recognized: {bdType}");
      return csType;
   }
}