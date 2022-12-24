// read file
var root = @"c:\code\bdull\";
var path = root + "hello.bd";
var text = System.IO.File.ReadLines(path);
var line = text.First();
// parse
var dot = line.IndexOf(".");
var paren = line.IndexOf("(");
var ns = line.Substring(0, dot);
var next = dot+1;
var cl = line.Substring(next, paren-next);
next = paren+1;
var ws = line.IndexOf(' ', next);
var paramType = line.Substring(next, ws-next);
next = ws+1;
var endParen = line.IndexOf(")");
var varName = line.Substring(next, endParen-next);

string ConvertType(string bdType) {
   var csType = bdType == "S"
      ? "string"
      : throw new Exception($"Type not recognized: {bdType}");
   return csType;
}
var t = "   ";
paramType = ConvertType(paramType);

var output = "";
output += @"// IL from BDull
.assembly extern System.Runtime {
  .publickeytoken = (B0 3F 5F 7F 11 D5 0A 3A )
  .ver 7:0:0:0
}";
output += "\r\n" + $".assembly {ns} {{}}";
output += "\r\n" + $".module {ns}.dll";
output += "\r\n" + $".class public auto ansi beforefieldinit {ns}.{cl}";
output += "\r\n" + "{";
output += "\r\n" + $"{t}.field public initonly string {varName}";
output += "\r\n" + "";
output += "\r\n" + $"{t}.method public hidebysig specialname rtspecialname instance void";
output += "\r\n" + $"{t}.ctor({paramType} {varName}) cil managed {{";
output += "\r\n" + $"{t}{t}.maxstack 4";
output += "\r\n" + $"{t}{t}ldarg.0 // this";
output += "\r\n" + $"{t}{t}ldarg.1";
output += "\r\n" + $"{t}{t}stfld string {ns}.{cl}::{varName}";
output += "\r\n" + $"{t}{t}nop";
output += "\r\n" + $"{t}{t}ret";
output += "\r\n" + $"{t}}}";
output += "\r\n" + "}";

Console.WriteLine("Write to window (w) or file (f)?");
var input = Console.ReadLine();
if (input == "w")
   Console.WriteLine(output);
else if (input == "f") {
   System.IO.File.WriteAllText($"{root}{ns}.il", output);
   Console.WriteLine($"Wrote file {root}{ns}.il");
}
else
   Console.WriteLine("Unknown input");
