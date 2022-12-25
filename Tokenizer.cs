namespace bdull;
public class Tokenizer
{
   public List<object> Tokenize(string text) {
      var line = text.Contains("\r\n") 
         ? text.Substring(0, text.IndexOf("\r\n")) 
         : text;
   
      var tokens = new List<object>();
      // tokenize
      var dot = line.IndexOf(".");
      var ns = line.Substring(0, dot);
      tokens.Add(new Namespace(ns));
   
      var next = dot+1;
      var paren = line.IndexOf("(");

      var cl = line.Substring(next, paren-next);
      tokens.Add(new Class(cl));
      tokens.Add(new OpenParen());

      next = paren+1;
      next = readParam(line, next, tokens);
      
      var nextChar = line.Substring(next, 1);
      if (nextChar == ",") {
         next += 1;
         next = skipWs(line, next);
         next = readParam(line, next, tokens);
      }
      
      tokens.Add(new CloseParen());

      return tokens;
   }

   int readParam(string line, int next, List<object> tokens) {
      var ws = line.IndexOf(' ', next);
      var paramType = line.Substring(next, ws-next);
      tokens.Add(new Type(paramType));

      next = ws+1;
      var endParen = line.IndexOf(")", next);
      var comma = line.IndexOf(',', next);
      var afterVarName = comma > -1 ? comma : endParen;
      var varName = line.Substring(next, afterVarName-next);
      tokens.Add(new VarName(varName));
      return afterVarName;
   }

   int skipWs(string line, int next) {
      while (line[next] == ' ') {
         next += 1;
      }
      return next;
   }
}