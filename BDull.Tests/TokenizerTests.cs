namespace BDull.Tests;

[TestClass]
public class TokenizerTests
{
   [TestMethod]
   public void NsClass() {
      var tk = new Tokenizer();
      var (ns, cl, nx) = tk.GetClass("App.User", 0);
      assert(ns.Value == "App");
      assert(cl.Value == "User");
   }

   [TestMethod]
   public void WParam() {
      var tk = new Tokenizer();
      var tks = tk.Tokenize("App.Auth.User(S First)");
      assert(tks[0] is Namespace ns && ns.Value == "App.Auth");
      assert(tks[1] is Class cl && cl.Value == "User");
      assert(tks[2] is OpenParen);
      assert(tks[3] is ParamType pt && pt.BdType == "S");
      assert(tks[4] is ParamName pn && pn.Value == "First");
   }

   [TestMethod]
   public void DateType() {
      var tk = new Tokenizer();
      var tks = tk.Tokenize("App.User(+D Birth)");
      assert(tks[0] is Namespace ns && ns.Value == "App");
      assert(tks[1] is Class cl && cl.Value == "User");
      assert(tks[2] is OpenParen);
      assert(tks[3] is Accessor a && a.Value == "+");
      assert(tks[4] is ParamType pt && pt.BdType == "D");
      assert(tks[5] is ParamName pn && pn.Value == "Birth");
   }

   [TestMethod]
   public void Concats() {
      var str = "👉First 👉Last";
      var tk = new Tokenizer();
      var list = tk.GetConcats(str);
      assert(list[0].t == ConcatType.Variable);
      assert(list[0].v == "First");
      assert(list[1].t == ConcatType.Literal);
      assert(list[1].v == " ");
      assert(list[2].t == ConcatType.Variable);
      assert(list[2].v == "Last");
   }

   [TestMethod]
   public void Concats2() {
      var str = "Hello 👉First, How are you?";
      var tk = new Tokenizer();
      var list = tk.GetConcats(str);
      assert(list[0].t == ConcatType.Literal);
      assert(list[0].v == "Hello ");
      assert(list[1].t == ConcatType.Variable);
      assert(list[1].v == "First");
      assert(list[2].t == ConcatType.Literal);
      assert(list[2].v == ", How are you?");
   }

   void assert(bool condition) {
        Assert.IsTrue(condition);
    }
}