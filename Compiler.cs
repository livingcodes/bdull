namespace BDull;
class Compiler {
   List<IToken> tokens = null;
   int c = 0; // cursor of tokens
   int Next() => c = c + 1;

   string t = "   "; // tab
   string il;
   void Line(string line, int tabs = 0) {
      for (var i=0; i<tabs; i++)
         il += t;
      il += line + "\r\n";
   }
   void Append(string text) => il += text;

   public (string il, string ns) Compile(List<IToken> tokens) {
      this.tokens = tokens;
      if (tokens[c] is EndOfFile)
         return ("", "");

      while (tokens[c] is Comment cm) {
         Line($"// {cm.Value}");
         Next();
      }

      string ns = null;
      if (tokens[c] is Namespace n)
         ns = n.Value;
      else
         throwExpected<Namespace>();

      Next();

      string cl = null;
      if (tokens[c] is Class cls)
         cl = cls.Value;
      else
         throwExpected<Class>();

      Next();

      Line(@"// IL from BDull
.assembly extern System.Runtime {
   .publickeytoken = (B0 3F 5F 7F 11 D5 0A 3A )
   .ver 7:0:0:0
}");
      Line($".assembly {ns} {{}}");
      Line($".module {ns}.dll");
      Line($".class public auto ansi beforefieldinit {ns}.{cl}");
      Line("{");

      if (tokens[c] is EndOfFile) {
         Line("}");
         return (il, ns);
      }

      if (tokens[c] is OpenParen) {
         Next();
         var parameters = new List<(string t, string n)>();
         var initializedFields = new List<(string t, string n, string v)>();
         do {
            if (tokens[c] is CloseParen) {
               Next();
               break;
            }
            if (!(tokens[c] is Accessor or ParamType))
               throwExpected<ParamType>();

            string ac = "";
            if (tokens[c] is Accessor a) {
               ac = a.Value;
               Next();
            }
            string pt = "";
            if (tokens[c] is ParamType p) {
               pt = p.Value;
               Next();
            }
            else
               throwExpected<ParamType>();
            
            string pn = "";
            if (tokens[c] is ParamName pp) {
               pn = pp.Value;
               Next();
            }
            else
               throwExpected<ParamName>();

            pt = ConvertType(pt);
            ac = ConvertAccessor(ac);
            parameters.Add((pt, pn));
            Line($".field {ac} initonly {pt} {pn}", 1);

            if (tokens[c] is Comma)
               Next();

         } while (true);

         Line("", 1);

         // fields
         do {
            Comments();

            if (!(tokens[c] is Accessor or ParamType))
               throwExpected<ParamType>();

            string ac2 = "";
            if (tokens[c] is Accessor a2) {
               ac2 = a2.Value;
               Next();
            }
            string pt2 = "";
            if (tokens[c] is ParamType p2) {
               pt2 = p2.Value;
               Next();
            } else
               throwExpected<ParamType>();
            string pn2 = "";
            if (tokens[c] is ParamName pp2) {
               pn2 = pp2.Value;
               Next();
            } else
               throwExpected<ParamName>();

            pt2 = ConvertType(pt2);
            ac2 = ConvertAccessor(ac2);
            Line($".field {ac2} {pt2} {pn2}", 1);

            if (tokens[c] is Equal) {
               Next();

               if (tokens[c] is IntegerValue iv) {
                  initializedFields.Add((pt2, pn2, iv.Value.ToString()));
                  Next();
               }
               else if (tokens[c] is StringValue sv) {
                  initializedFields.Add((pt2, pn2, sv.Value));
                  Next();
               }
               else {
                  throw new CompilerEx($"Expected initialized value. Actual {tokens[c].GetType()}");
               }
            }

            if (tokens.Count <= c || tokens[c] is EndOfFile)
               break;
         } while (true);

         Line("", 1);
         Line($".method public hidebysig specialname rtspecialname instance void", 1);
         Line($".ctor(", 1);

         var i = 0;
         foreach (var p in parameters) {
            i += 1;
            var comma = (i < parameters.Count) ? "," : "";
            Line($"{p.t} {p.n}{comma}", 2);
         }
         Line($") cil managed {{", 1);
         Line($".maxstack 4", 2);
         i = 0;
         foreach (var p in parameters) {
            i += 1;
            Line($"ldarg.0 // this", 2);
            Line($"ldarg.{i}", 2);
            Line($"stfld {p.t} {ns}.{cl}::{p.n}", 2);
         }
         foreach (var f in initializedFields) {
            Line($"ldarg.0 // this", 2);
            if (f.t == "int32") {
               Line($"ldc.i4.{f.v}", 2);
               Line($"stfld {f.t} {ns}.{cl}::{f.n}", 2);
            }
            else if (f.t == "string") {
               Line($"ldstr \"{f.v}\"", 2);
               Line($"stfld {f.t} {ns}.{cl}::{f.n}", 2);
            }
            else {
               throw new CompilerEx($"Unknown type of initialized field. Actual {f.t}");
            }
         }
         Line($"nop", 2);
         Line($"ret", 2);
         Line("}", 1);
      }
      Line("");
      
      Line("}");
      return (il, ns);
   }

   void Comments() {
      Comment cm = null;
      do {
         if (tokens[c] is Comment) {
            cm = (Comment)tokens[c];
            Line($"// {cm.Value}");
            Next();
         } else
            cm = null;
      } while (cm != null);
   }

   string ConvertType(string bdType) {
      var csType = 
         bdType == "S"
            ? "string"
         : bdType == "I"
            ? "int32"
         : throw new Exception($"Type not recognized: {bdType}");
      return csType;
   }

   string ConvertAccessor(string bdAccessor) {
      var csAccessor =
         bdAccessor == ""
            ? "public"
         : bdAccessor == "+"
            ? "public"
         : bdAccessor == "-"
            ? "private"
         : throw new CompilerEx($"Accessor not recognized: {bdAccessor}");
      return csAccessor;
   }

   void throwExpected<T>() =>
      throw new CompilerEx($"Expected {nameof(T)}.");
}