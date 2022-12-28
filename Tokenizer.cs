namespace BDull;
public class Tokenizer
{
   public List<IToken> Tokenize(string /*bdull*/code) {
      var tokens = new List<IToken>();
      var i = 0;

      var endOfFile = GetEndOfFile(code, i);
      if (endOfFile != null) {
         tokens.Add(endOfFile);
         return tokens; }

      (var ns, var cl, i) = GetClass(code, i);
      if (ns != null)
         tokens.Add(ns);
      tokens.Add(cl);

      endOfFile = GetEndOfFile(code, i);
      if (endOfFile != null) {
         tokens.Add(endOfFile);
         return tokens;
      }
      
      if (code[i] != '(')
         throw new Exception("Expected (. Actual " + code[i]);
      else
         tokens.Add(new OpenParen());

      i+=1;
      
      (var parameters, i) = GetParams(code, i);
      foreach (var p in parameters)
         tokens.Add(p);

      if (code[i] == ')')
         tokens.Add(new CloseParen());
      else
         throw new Exception("Expected ). Actual " + code[i]);

      i+=1;

      var eof = GetEndOfFile(code, i);
      if (eof != null) {
         tokens.Add(eof);
         return tokens; }

      i = skipWs(code, i);
      (var f, i) = skipReturn(code, i);

      // +S Name
      do {
         (var accessor, i) = GetAccessor(code, i);
         if (accessor != null)
            tokens.Add(accessor);

         (var fieldType, i) = GetParamType(code, i);
         if (fieldType != null)
            tokens.Add(fieldType);
         else
            throw new Exception("Expected param type");

         (var fieldName, i) = GetFieldName(code, i);
         if (fieldName != null)
            tokens.Add(fieldName);
         else
            throw new Exception("Expected param name");

         eof = GetEndOfFile(code, i);
         if (eof != null) {
            tokens.Add(eof);
            return tokens; }

         (f, i) = skipReturn(code, i);
         if (!f)
            break;
      } while (true);
      return tokens;
   }
   
   IToken GetEndOfFile(string code, int i) {
      if (code.Length <= i)
         return new EndOfFile();
      return null;
   }
   
   public (Namespace ns, Class cl, int nx) GetClass(string code, int i) {
      var endOfFile = GetEndOfFile(code, i);
      if (endOfFile != null)
         throw new Exception("Expected class. Actual end of file");
      
      var tokens = new List<IToken>();
      var identifiers = new List<string>();
      var word = "";
      do {
         if (i >= code.Length) {
            identifiers.Add(word);
            break;
         }

         var ch = code[i];
         
         // alpha, append letter
         if (IsAlpha(code, i))
            word += ch;

         // dot, append token
         if (ch == '.') {
            identifiers.Add(word);
            word = "";
         }

         // open paren, return
         if (ch == '(') {
            identifiers.Add(word);
            break;
         }
         i++;
      } while (true);

      if (identifiers.Count == 1) {
         var cl = new Class(identifiers[0]);
         return (null, cl, i);
      }
      else {

         var ns = "";
         for (var j=0; j<identifiers.Count; j++) {
            var isLast = j == identifiers.Count-1;
            if (isLast) {
               ns = ns.Substring(0, ns.Length-1);
               var n = new Namespace(ns);
               var c = new Class(identifiers[j]);
               return (n, c, i);
            }
            ns += identifiers[j] + ".";
         }
      }
      return (null, null, i);
   }

   public (List<IToken>, int) GetParams(string code, int i) {
      var tokens = new List<IToken>();
      do {
         i = skipWs(code, i);
         (var paramAccessor, i) = GetAccessor(code, i);
         i = skipWs(code, i);
         (var paramType, i) = GetParamType(code, i);
         i = skipWs(code, i);
         (var paramName, i) = GetParamName(code, i);
         i = skipWs(code, i);
         if (paramAccessor != null)
            tokens.Add(paramAccessor);
         tokens.Add(paramType);
         tokens.Add(paramName);
         if (code[i] == ',') {
            tokens.Add(new Comma());
            i+=1;
         }
      } while(code[i] != ')');
      return (tokens, i);
   }

   (Accessor, int) GetAccessor(string code, int i) {
      i = skipWs(code, i);
      string text = "";
      if (code[i] == '+' || code[i] == '-') {
         text = code[i].ToString();
         i+=1;
      }
      var accessor = text != ""
         ? new Accessor(text)
         : null;
      return (accessor, i);
   }

   (ParamType, int) GetParamType(string code, int i) {
      i = skipWs(code, i);
      (var text, i) = ReadAlphaUntil(code, i, ' ');
      return (new ParamType(text), i);
   }

   (ParamName, int) GetParamName(string code, int i) {
      i = skipWs(code, i);
      (var text, i) = ReadAlphaUntil(code, i, ',', ')');
      return (new ParamName(text), i);
   }

   (ParamName, int) GetFieldName(string code, int i) {
      i = skipWs(code, i);
      (var text, i) = ReadAlphaUntil(code, i, ' ', '\r', '\n');
      return (new ParamName(text), i);
   }

   (string, int) ReadAlphaUntil(string code, int i, params char[] ch) {
      var word = "";
      do {
         if (code.Length <= i || ch.Contains(code[i]))
            return (word, i);
         if (!IsAlpha(code, i))
            throw new Exception("Expected alpha. Actual " + code[i]);
         word += code[i];
         i++;
      } while(true);
   }

   bool IsAlpha(string code, int i) {
      var isAlpha = System.Text.RegularExpressions.
         Regex.IsMatch(code[i].ToString(), "^[a-z|A-Z]{1}$");
      return isAlpha;
   }

   int readParam(string line, int next, List<object> tokens) {
      var ws = line.IndexOf(' ', next);
      var paramType = line.Substring(next, ws-next);
      tokens.Add(new ParamType(paramType));

      next = ws+1;
      var endParen = line.IndexOf(")", next);
      var comma = line.IndexOf(',', next);
      var afterVarName = comma > -1 ? comma : endParen;
      var varName = line.Substring(next, afterVarName-next);
      tokens.Add(new ParamName(varName));
      return afterVarName;
   }

   int skipWs(string line, int next) {
      while (line[next] == ' ')
         next += 1;
      return next;
   }

   (bool, int) skipReturn(string code, int i) {
      var found = false;
      while (code[i] == '\r' || code[i] == '\n') {
         i += 1;
         found = true;
      }
      return (found, i);
   }
}