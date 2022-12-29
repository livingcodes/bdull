namespace BDull;
public record Namespace(string Value) : IToken {
   public override string ToString() => Value;
}
public record Class(string Value) : IToken {
   public override string ToString() => Value;
}
public record OpenParen() : IToken;
public record CloseParen() : IToken;
public record ParamType(string Value) : IToken;
public record ParamName(string Value) : IToken;
public record Comma() : IToken;
public record Equal() : IToken;
public record EndOfFile() : IToken;
public record Accessor(string Value) : IToken {
   public override string ToString() => Value;
}
public record IntegerValue(int Value) : IToken {
   public override string ToString() => Value.ToString();
}
public record StringValue(string Value, List<(ConcatType, string)> Concats) : IToken {
   public override string ToString() => Value;
}
public record Comment(string Value) : IToken {
   public override string ToString() => Value;
}
public interface IToken {}