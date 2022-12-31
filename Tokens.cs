namespace BDull;
public record Namespace(string Value) : IToken {
   public override string ToString() => Value;
}
public record Class(string Value) : IToken {
   public override string ToString() => Value;
}
public record OpenParen() : IToken;
public record CloseParen() : IToken;
public record ParamType : IToken {
   public ParamType(string bdType) {;
      BdType = BdType.FromString(bdType);
      IlType = BdType.ToIlType(bdType);
   }
   public readonly BdType BdType;
   public readonly IlType IlType;
}
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
public record StringValue(string Value, List<(ConcatType ct, string cv)> Concats) : IToken {
   public override string ToString() => Value;
}
public class DateValue : IToken {
   public DateValue(string value) {
      Value = value;
      Date = DateTime.Parse(Value);
   }
   public readonly string Value;
   public override string ToString() => Value;
   public readonly DateTime Date;
}
public record Comment(string Value) : IToken {
   public override string ToString() => Value;
}
public interface IToken {}