namespace BDull;
public class BdType {
   private BdType(string value) {
      Value = value;
      IlType = BdType.ToIlType(value);
   }

   public readonly string Value;
   public readonly IlType IlType;
   public override string ToString() => Value;
   public static implicit operator string(BdType bt) => bt.ToString();
   public IlType ToIlType() => ToIlType(Value);

   public static IlType ToIlType(string bdTypeName) {
      if (bdTypeName == s)
         return IlType.String;
      if (bdTypeName == i)
         return IlType.Int32;
      if (bdTypeName == d)
         return IlType.Date;
      throw new Exception($"Unknown BDull type when converting to IL type. BDull type: {bdTypeName}");
   }

   public static BdType FromString(string bdTypeName) {
      if (bdTypeName == s)
         return BdType.S;
      if (bdTypeName == i)
         return BdType.I;
      if (bdTypeName == d)
         return BdType.D;
      throw new Exception($"Unknown BDull type value: {bdTypeName}");
   }

   static readonly string s = "S";
   static readonly string i = "I";
   static readonly string d = "D";

   /// <summary>String</summary>
   public static readonly BdType S = new(s);
   /// <summary>Integer</summary>
   public static readonly BdType I = new(i);
   /// <summary>Date</summary>
   public static readonly BdType D = new(d);
}