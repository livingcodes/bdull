namespace BDull;

public class Program
{
   public static void Main() {
      // var root = @"c:\code\bdull\1 param\";
      // var path = root + @"1param.bd";

      var root = @"c:\code\bdull\2 param\";
      var path = root + @"2param.bd";
      var code = File.ReadAllText(path);
      //var code = "App.Auth.User(+S First, S Last, -I age)";
      var tokens = new Tokenizer().Tokenize(code);
      var (il, ns) = new Compiler().Compile(tokens);

      Console.WriteLine("Write to window (w) or file (f)?");
      var input = Console.ReadLine();
      if (input == "w")
         Console.WriteLine(il);
      else if (input == "f") {
         File.WriteAllText($"{root}{ns}.il", il);
         Console.WriteLine($"Wrote file {root}{ns}.il");
      }
      else
         Console.WriteLine("Unknown input");
      Console.WriteLine("Press any key to exit");
      Console.ReadLine();
   }
}