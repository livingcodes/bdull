namespace BDull;
public class IlType {
   private IlType(string value) {
      Value = value;
      BdType = ToBdType(value);
   }
   public readonly string Value;
   public readonly BdType BdType;
   public override string ToString() => Value;
   public static implicit operator string(IlType ilType) => ilType.Value;

   public static BdType ToBdType(string ilTypeName) {
      if (ilTypeName == s)
         return BdType.S;
      if (ilTypeName == i)
         return BdType.I;
      if (ilTypeName == d)
         return BdType.D;
      throw new Exception($"Unknown IL type when converting to BDull type. IL Type: {ilTypeName}");
   }

   public static IlType FromString(string ilTypeName) {
      if (ilTypeName == s)
         return IlType.String;
      if (ilTypeName == i)
         return IlType.Int32;
      if (ilTypeName == d)
         return IlType.Date;
      throw new Exception($"Unknown IL type value: {ilTypeName}");
   }

   static readonly string s = "string";
   static readonly string i = "int32";
   static readonly string d = "valuetype [System.Runtime]System.DateTime";

   public static readonly IlType String = new(s);
   public static readonly IlType Int32 = new(i);
   public static readonly IlType Date = new(d);
}