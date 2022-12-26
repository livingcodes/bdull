namespace BDull.Tests;

[TestClass]
public class TokenizerTests
{
    [TestMethod]
    public void NsClass() {
        var tk = new BDull.Tokenizer();
        var (ns, cl, nx) = tk.GetClass("App.User", 0);
        assert(ns.Value == "App");
        assert(cl.Value == "User");
    }

    [TestMethod]
    public void WParam() {
        var tk = new BDull.Tokenizer();
        var tks = tk.Tokenize("App.Auth.User(S First)");
        assert(tks[0] is Namespace ns && ns.Value == "App.Auth");
        assert(tks[1] is Class cl && cl.Value == "User");
        assert(tks[2] is OpenParen);
        assert(tks[3] is ParamType pt && pt.Value == "S");
        assert(tks[4] is ParamName pn && pn.Value == "First");
    }

    void assert(bool condition) {
        Assert.IsTrue(condition);
    }
}